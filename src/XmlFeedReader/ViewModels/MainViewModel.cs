﻿using Juniansoft.MvvmReady;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private string _feedUrl;
        public string FeedUrl
        {
            get => Settings.Default.FeedUrl;
            set
            {
                Settings.Default.FeedUrl = value;
                this.RaisePropertyChanged();
            }
        }

        private string _outputRootFolder;
        public string OutputRootFolder
        {
            get => _outputRootFolder;
            set => this.RaiseAndSetIfChanged(ref _outputRootFolder, value);
        }

        private bool _isPriceRounding;
        public bool IsPriceRounding
        {
            get => _isPriceRounding;
            set => this.RaiseAndSetIfChanged(ref _isPriceRounding, value);
        }

        private string _startDescriptions;
        public string StartDescriptions
        {
            get => _startDescriptions;
            set => this.RaiseAndSetIfChanged(ref _startDescriptions, value);
        }

        private string _endDescriptions;
        public string EndDescriptions
        {
            get => _endDescriptions; 
            set => this.RaiseAndSetIfChanged(ref _endDescriptions, value);
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
            var selectedPath = await _dialogService.ShowFolderBrowserDialogAsync();
            if(!string.IsNullOrWhiteSpace(selectedPath))
            {
                OutputRootFolder = selectedPath;
            }
        }
        

    }
}
