using Juniansoft.MvvmReady;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            _log.Information($"Opening `{path}`");
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
        public string GetProductsButtonText
        {
            get => _getProductsButtonText;
            set => this.RaiseAndSetIfChanged(ref _getProductsButtonText, value);
        }

        public ICommand GetProductsCommand { get; private set; }
        private async Task GetProductsAsync()
        {
            if(IsProcessing)
            {
                var result = await _dialogService.ShowYesNoAsync("Are you sure you want to cancel?");
                if(result == true)
                    source.Cancel();
                return;
            }

            IsProcessing = true;
            GetProductsButtonText = "Cancel";

            source = new CancellationTokenSource();
            var token = source.Token;

            var productList = await GetProductListAsync(token);

            await Task.Delay(10000, token);

            GetProductsButtonText = "Get Products";
            IsProcessing = false;
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
                        Id = (int) p.Element("id"),
                        Title = WebUtility.HtmlDecode((string)p.Element("title")),
                        Description = WebUtility.HtmlDecode((string)p.Element("description")),
                        Link = WebUtility.HtmlDecode((string)p.Element("link")),
                        ImageLink = WebUtility.HtmlDecode((string)p.Element("image")),
                        Price = (float)p.Element("price"),
                        Availability = WebUtility.HtmlDecode((string)p.Element("availability")),
                        AddToCartLink = WebUtility.HtmlDecode((string)p.Element("add_to_cart_link")),
                        StockLevel = (int)p.Element("stock_level"),
                        Hidden = WebUtility.HtmlDecode((string)p.Element("hidden")),
                        Visibility = WebUtility.HtmlDecode((string)p.Element("visibility")),
                        Virtual = WebUtility.HtmlDecode((string)p.Element("virtual")),
                    };

                    var categories = p.Element("categories")
                        .Descendants("category");

                    foreach (var category in categories)
                    {
                        var cat = WebUtility.HtmlDecode((string)category).Trim();
                        if (!string.IsNullOrWhiteSpace(cat))
                            productObject.Categories.Add(cat);
                    }

                    var categoriesLink = p.Element("category_links")
                        .Descendants("category_link");

                    foreach (var category in categoriesLink)
                    {
                        var cat = WebUtility.HtmlDecode((string)category).Trim();
                        if (!string.IsNullOrWhiteSpace(cat))
                            productObject.CategoryLinks.Add(cat);
                    }

                    var additionalImages = p.Elements()
                        .Where(x => x.Name.ToString().StartsWith("additional_image_link"));

                    foreach (var item in additionalImages)
                    {
                        productObject.AdditionalImageLinks.Add(WebUtility.HtmlDecode(item.ToString().Trim()));
                    }

                    _log.Information(productObject.AdditionalImageLinks.Count + "");

                    result.Add(productObject);
                }
            }
            catch (TaskCanceledException ex) { }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorAsync(ex.ToString());
                return new List<Product>();
            }

            try
            {
                File.Delete(outputPath);
            }
            catch { }

            return result;
        }

    }
}
