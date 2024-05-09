using Juniansoft.MvvmReady;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using XmlFeedReader.Models;
using XmlFeedReader.Properties;
using XmlFeedReader.Services;

namespace XmlFeedReader.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        private readonly AssemblyService _assembly;
        private readonly DialogService _dialogService;
        private readonly ILogger _log;

        public Action<Action> SafeAction { get; set; }

        public MainViewModel()
        {
            _assembly = ServiceLocator.Current.Get<AssemblyService>();
            _dialogService = ServiceLocator.Current.Get<DialogService>();
            _log = ServiceLocator.Current.Get<ILogger>();

            AppTitle = $"{_assembly.AssemblyProduct} - v{_assembly.AssemblyVersion}";
            AppIcon = Resources.Favicon;


            SelectRootFolderCommand = ReactiveCommand.CreateFromTask(SelectRootFolderAsync);
            OpenRootFolderCommand = ReactiveCommand.Create(() => OpenFolder(OutputRootFolder));
            TestFeedCommand = ReactiveCommand.CreateFromTask(TestFeedAsync);
            SaveSettingsCommand = ReactiveCommand.CreateFromTask(SaveSettingsAsync);
            GetProductsCommand = ReactiveCommand.Create(GetProductsAsync);
            IsSavingSettingsCommand = ReactiveCommand.Create(IsSavingSettingsAsync);
        }

        private string _appTitle;
        public string AppTitle
        {
            get => _appTitle;
            set => this.RaiseAndSetIfChanged(ref _appTitle, value);
        }

        private Icon _appIcon;
        public Icon AppIcon
        {
            get => _appIcon;
            set => this.RaiseAndSetIfChanged(ref _appIcon, value);
        }

        public string FeedUrl
        {
            get => Settings.Default.FeedUrl;
            set
            {
                Settings.Default.FeedUrl = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand TestFeedCommand { get; private set; }
        private async Task TestFeedAsync()
        {
            var outputPath = Path.GetTempFileName();
            var url = FeedUrl;

            try
            {

                using (var client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(
                        new Uri(url),
                        outputPath);
                }

                var products = XElement.Load(outputPath);

                await _dialogService.ShowMessageAsync("Download and parse test success.");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(ex.ToString());
            }

            File.Delete(outputPath);
        }
        public string OutputRootFolder
        {
            get => Settings.Default.OutputFolder;
            set
            {
                Settings.Default.OutputFolder = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsPriceRounding
        {
            get => Settings.Default.IsPriceRounding;
            set
            {
                Settings.Default.IsPriceRounding = value;
                this.RaisePropertyChanged();
            }
        }

        public string DescriptionStart
        {
            get => Settings.Default.DescriptionStart;
            set
            {
                Settings.Default.DescriptionStart = value;
                this.RaisePropertyChanged();
            }
        }

        public string DescriptionEnd
        {
            get => Settings.Default.DescriptionEnd; 
            set
            {
                Settings.Default.DescriptionEnd = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand SaveSettingsCommand { get; private set; }
        private async Task SaveSettingsAsync()
        {
            Settings.Default.Save();
            await _dialogService.ShowMessageAsync("Settings saved succesfully!");
        }

        public ICommand OpenRootFolderCommand { get; private set; }
        private void OpenFolder(string path)
        {
            //_log.Information($"Opening `{path}`");
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    _log.Error(ex.ToString());
                }
            }

            Process.Start(path);
        }

        public ICommand SelectRootFolderCommand { get; private set; }
        private async Task SelectRootFolderAsync()
        {
            var selectedPath = await _dialogService.ShowFolderBrowserAsync();
            if(!string.IsNullOrWhiteSpace(selectedPath))
            {
                OutputRootFolder = selectedPath;
            }
        }

        public ReactiveCommand<Unit, Task<bool?>> IsSavingSettingsCommand { get; private set; }
        public Task<bool?> IsSavingSettingsAsync()
        {
            return _dialogService.ShowYesNoAsync("Do you want to Save your Settings?");
        }

        private bool _isProcessing = false;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                this.RaiseAndSetIfChanged(ref _isProcessing, value);
                this.RaisePropertyChanged(nameof(IsNotProcessing));
            }
        }

        public bool IsNotProcessing
            => !_isProcessing;

        CancellationTokenSource source = new CancellationTokenSource();

        private string _getProductsButtonText = "Get Products";
        private HashSet<string> _processedProduct;
        private ConcurrentBag<string> _productsAdded;
        private ConcurrentBag<string> _productsUpdated;
        private ConcurrentBag<string> _productsDeleted;
        private ConcurrentBag<string> _productsError;

        public string GetProductsButtonText
        {
            get => _getProductsButtonText;
            set => this.RaiseAndSetIfChanged(ref _getProductsButtonText, value);
        }

        public Action<string> RecordErrorAction { get; set; }
        public ICommand GetProductsCommand { get; private set; }
        private async Task GetProductsAsync()
        {
            if(IsProcessing)
            {
                var result = await _dialogService.ShowYesNoAsync("Are you sure you want to cancel?");
                if (result == true)
                {
                    source.Cancel();

                    ProgressText = "Canceling ...";
                }
                return;
            }

            IsProcessing = true;
            GetProductsButtonText = "Cancel";

            //_log.Information($"Creating directory: {OutputRootFolder}");
            try
            {
                var allProducts = Path.Combine(OutputRootFolder, ProductsDir);
                Directory.CreateDirectory(allProducts);
                var log = Path.Combine(OutputRootFolder, "LogFile.txt");

                ProgressText = "Starting ...";
                MaxProgress = 1;
                CurrentProgress = 0;

                WriteLog(log, "Starting ...");

                _processedProduct = new HashSet<string>();
                _productsAdded = new ConcurrentBag<string>();
                _productsUpdated = new ConcurrentBag<string>();
                _productsDeleted = new ConcurrentBag<string>();
                _productsError = new ConcurrentBag<string>();

                RecordErrorAction = (x) =>
                {
                    WriteLog(log, x);
                    _productsError.Add(x);
                };

                source = new CancellationTokenSource();
                var token = source.Token;

                var productList = await GetProductListAsync(token);
                var uniqueDirs = Directory.GetDirectories(allProducts);

                var uniqueProducts = productList
                    .Where(x => !string.IsNullOrWhiteSpace(x.Title))
                    .Select(x => SafeFilename(x.Title))
                    .Distinct()
                    .ToDictionary(x => x);

                foreach (var dir in uniqueDirs)
                {
                    var dirName = new DirectoryInfo(dir).Name;

                    if (!uniqueProducts.ContainsKey(dirName))
                    {
                        _log.Information($"Deleted {dirName}");
                        _productsDeleted.Add(dir);
                        Directory.Delete(dir, true);
                    }
                }

                MaxProgress = productList.Count;
                CurrentProgress = 0;

                foreach (var p in productList)
                {
                    CurrentProgress++;

                    ProgressText = $"({CurrentProgress.ToString("D3")}/{(MaxProgress).ToString("D3")}) Processing '{p.Id} - {p.Title}' ...";

                    var errorDir = await ProcessProductAsync(p, token);

                    if (!string.IsNullOrWhiteSpace(errorDir) && Directory.Exists(errorDir))
                        Directory.Delete(errorDir, true);

                    if (token.IsCancellationRequested)
                        break;
                }

                if (!token.IsCancellationRequested)
                {
                    CurrentProgress = MaxProgress;
                    ProgressText = $"({(MaxProgress).ToString("D3")}/{(MaxProgress).ToString("D3")}) Finished.";
                    WriteLog(log, $"finished with {_productsAdded.Count} products added, {_productsUpdated.Count} updated, {_productsDeleted.Count} deleted, {_productsError.Count} errors.");
                }
                else
                {
                    ProgressText = $"({CurrentProgress.ToString("D3")}/{(MaxProgress).ToString("D3")}) Canceled by user.";
                    WriteLog(log, "Canceled by user.");
                }
            }
            catch(Exception ex)
            {
                _log.Error(ex.ToString());
                await _dialogService.ShowErrorAsync(ex.ToString());
            }

            GetProductsButtonText = "Get Products";
            IsProcessing = false;
        }

        const string ProductsDir = "Products";

        /// <summary>
        /// Process the product.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>If process has issue, it'll return errorDir.</returns>
        private async Task<string> ProcessProductAsync(Product p, CancellationToken token)
        {
            var safeProductTitle = SafeFilename(p.Title);

            if (_processedProduct.Contains(safeProductTitle))
            {
                RecordErrorAction($"Copy failed for \"{p.Id} - {p.Title}\" a product with same title already exists.");
                return string.Empty;
            }

            _processedProduct.Add(safeProductTitle);

            var isModified = false;

            if (string.IsNullOrWhiteSpace(p.Title))
            {
                RecordErrorAction($"Copy failed for \"{p.Id}\" no title was found.");
                return string.Empty;
            }

            var productDir = Path.Combine(OutputRootFolder, ProductsDir, safeProductTitle);

            var priceFloat = default(float);

            if (p.Price == null || !float.TryParse(p.Price, out priceFloat))
            {
                RecordErrorAction($"Copy failed for \"{p.Id} - {p.Title}\" no price was found.");
                return productDir;
            }

            if (string.IsNullOrWhiteSpace(p.Description))
            {
                RecordErrorAction($"Copy failed for \"{p.Id} - {p.Title}\" no description was found.");
                return productDir;
            }

            if (string.IsNullOrWhiteSpace(p.ImageLink))
            {
                RecordErrorAction($"Copy failed for \"{p.Id} - {p.Title}\" no main image was found.");
                return productDir;
            }

            if (Directory.Exists(productDir))
            {
                isModified = true;
                
                var lastmodText = Directory.GetCreationTime(productDir).ToString("yyyy-MM-dd HH:mm:ss");
                
                if (string.IsNullOrWhiteSpace(p.LastModified))
                {
                    Directory.Delete(productDir, true);
                }
                else if(!string.IsNullOrWhiteSpace(lastmodText) && string.Compare(p.LastModified, lastmodText, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    Directory.Delete(productDir, true);
                }
                else
                {
                    _log.Information($"Skipping {p.Id} - {p.Title} ...");
                    return string.Empty;
                }
            }

            Directory.CreateDirectory(productDir);

            DateTime lastModDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(p.LastModified))
            {
                try
                {
                    lastModDate
                        = DateTime.ParseExact(p.LastModified, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    
                }
                catch(Exception ex)
                {
                    _log.Error(ex.ToString());
                }
            }

            int price = IsPriceRounding ? (int)Math.Round(priceFloat) : (int)Math.Floor(priceFloat);
            var productDescriptionFile = Path.Combine(productDir, $"{price}.txt");

            var prodict = p.ToDictionary();
            var description = string.Join("", DescriptionStart, p.Description, DescriptionEnd);

            foreach(var kv in prodict)
            {
                description = description.Replace(kv.Key, kv.Value);
            }

            File.WriteAllText(productDescriptionFile, description);

            File.SetCreationTime(productDescriptionFile, lastModDate);
            File.SetLastAccessTime(productDescriptionFile, lastModDate);
            File.SetLastWriteTime(productDescriptionFile, lastModDate);

            var imageDownloadList = new Dictionary<char, string>();
            var key = 'A';

            imageDownloadList.Add(key++, p.ImageLink);
            foreach (var l in p.AdditionalImageLinks)
            {
                imageDownloadList.Add(key++, l);
            }

            var imageDownloadError = new ConcurrentBag<string>();

            //Parallel.ForEach(imageDownloadList, async (img) =>
            //foreach(var img in imageDownloadList)
            await Task.WhenAll(imageDownloadList.Select(async img =>
            {
                //_log.Information(img.Value);
                var targetFilename =
                        Path.Combine(
                            productDir,
                            $"{img.Key}{Path.GetExtension(GetFileNameFromUrl(img.Value))}");
                //_log.Information(targetFilename);

                var tmp = Path.GetTempFileName();

                try
                {
                    using (var client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(
                            new Uri(img.Value),
                            tmp);
                    }

                    //_log.Information($"Moving '{tmp}' to '{targetFilename}'");
                    if (File.Exists(targetFilename))
                        File.Delete(targetFilename);

                    File.Move(tmp, targetFilename);

                    File.SetCreationTime(targetFilename, lastModDate);
                    File.SetLastAccessTime(targetFilename, lastModDate);
                    File.SetLastWriteTime(targetFilename, lastModDate);
                }
                catch (Exception ex)
                {
                    imageDownloadError.Add(img.Value);
                    _log.Error(ex.ToString());
                }
            }));

            if(imageDownloadError.Count > 0)
            {
                foreach (var imgError in imageDownloadError)
                {
                    RecordErrorAction($"Copy failed for \"{p.Id} - {p.Title}\" unable to download {imgError}.");
                    break;
                }
                return productDir;
            }

            await Task.CompletedTask;

            if (isModified)
                _productsUpdated.Add($"{p.Id} - {p.Title}");
            else
                _productsAdded.Add($"{p.Id} - {p.Title}");

            Directory.SetCreationTime(productDir, lastModDate);
            Directory.SetLastAccessTime(productDir, lastModDate);
            Directory.SetLastWriteTime(productDir, lastModDate);

            return string.Empty;
        }

        private async Task<List<Product>> GetProductListAsync(CancellationToken token)
        {
            var outputPath = Path.GetTempFileName();
            var url = FeedUrl;

            var result = new List<Product>();
            try
            {
                using (var client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(
                        new Uri(url),
                        outputPath);
                }

                var productsXml = XElement.Load(outputPath);
                var productNodeList = productsXml.Descendants("product");

                foreach (var p in productNodeList)
                {
                    var productObject = new Product()
                    {
                        Id = HtmlDecode((string)p?.Element("id")),
                        Title = HtmlDecode((string)p?.Element("title")),
                        Description = HtmlDecode((string)p?.Element("description")),
                        Link = HtmlDecode((string)p?.Element("link")),
                        ImageLink = HtmlDecode((string)p?.Element("image_link")),
                        Price = HtmlDecode((string)p?.Element("price")),
                        Availability = HtmlDecode((string)p?.Element("availability")),
                        AddToCartLink = HtmlDecode((string)p?.Element("add_to_cart_link")),
                        StockLevel = HtmlDecode((string)p?.Element("stock_level")),
                        Hidden = HtmlDecode((string)p?.Element("hidden")),
                        Visibility = HtmlDecode((string)p?.Element("visibility")),
                        Virtual = HtmlDecode((string)p?.Element("virtual")),
                        LastModified = HtmlDecode((string)p?.Element("last_modified")),
                    };

                    
                    var categories = p.Element("categories")
                        ?.Descendants("category");

                    if (categories != null)
                    {
                        foreach (var category in categories)
                        {
                            var cat = HtmlDecode((string)category).Trim();
                            if (!string.IsNullOrWhiteSpace(cat))
                                productObject.Categories.Add(cat);
                        }
                    }

                    var categoriesLink = p.Element("category_links")
                        ?.Descendants("category_link");

                    if (categoriesLink != null)
                    {
                        foreach (var category in categoriesLink)
                        {
                            var cat = HtmlDecode((string)category).Trim();
                            if (!string.IsNullOrWhiteSpace(cat))
                                productObject.CategoryLinks.Add(cat);
                        }
                    }

                    var additionalImages = p.Elements()
                        ?.Where(x => x.Name.ToString().StartsWith("additional_image_link"));

                    if (additionalImages != null)
                    {
                        foreach (var item in additionalImages)
                        {
                            var link = HtmlDecode((string)item).Trim();
                            if (!string.IsNullOrWhiteSpace(link))
                                productObject.AdditionalImageLinks.Add(link);
                        }
                    }

                    result.Add(productObject);
                }
            }
            catch (TaskCanceledException ex) 
            {
                _log.Warning(ex.ToString());
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                await _dialogService.ShowErrorAsync(ex.ToString());
                result = new List<Product>();
            }

            try
            {
                File.Delete(outputPath);
            }
            catch { }

            return result;
        }

        private string SafeFilename(string filename)
        {
            return string.Join("", filename.Split(Path.GetInvalidFileNameChars()));
        }

        private string GetFileNameFromUrl(string url)
        {
            //_log.Information(url);
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                uri = new Uri(new Uri("https://www.juniansoft.com/"), url);

            return (Path.GetFileName(uri.LocalPath));
        }

        private int _currentProgress = 0;
        public int CurrentProgress
        {
            get => _currentProgress;
            set => this.RaiseAndSetIfChanged(ref _currentProgress, value);
        }

        private int _maxProgress = 1;
        public int MaxProgress
        {
            get => _maxProgress;
            set => this.RaiseAndSetIfChanged(ref _maxProgress, value);
        }

        private void WriteLog(string path, string message)
        {
            var logMessage
                = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - {message}{Environment.NewLine}";
            File.AppendAllText(path, logMessage);
            AppendLogAction(logMessage);
        }

        public Action<string> AppendLogAction { get; set; }

        private string _progressText = "...";
        public string ProgressText
        {
            get => _progressText;
            set => this.RaiseAndSetIfChanged(ref _progressText, value);
        }

        private string HtmlDecode(string html) 
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            return WebUtility.HtmlDecode(html);
        }

    }
}
