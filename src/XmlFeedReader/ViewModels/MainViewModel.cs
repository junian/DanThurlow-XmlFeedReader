using Juniansoft.MvvmReady;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
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

            SaveSettingsCommand = ReactiveCommand.CreateFromTask(SaveSettingsAsync);
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

    }
}
