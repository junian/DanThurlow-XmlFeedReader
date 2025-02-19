﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XmlFeedReader.Properties;
using XmlFeedReader.ViewModels;

namespace XmlFeedReader.Forms
{
    public partial class MainForm : Form, IViewFor<MainViewModel>
    {
        public MainForm()
        {
            InitializeComponent();
            this.WhenActivated(block =>
            {
                var configPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
                if (!File.Exists(configPath))
                {
                    //Existing user config does not exist, so load settings from previous assembly
                    Settings.Default.Upgrade();
                    Settings.Default.Reload();
                    Settings.Default.Save();
                }

                ViewModel.SafeAction = (action) =>
                {
                    if (this.IsDisposed)
                        return;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(action);
                    }
                    else
                    {
                        action?.Invoke();
                    }
                };

                this.OneWayBind(ViewModel, vm => vm.AppTitle, view => view.Text);
                this.OneWayBind(ViewModel, vm => vm.AppIcon, view => view.Icon);

                this.OneWayBind(ViewModel, vm => vm.IsNotProcessing, view => view.groupBoxSettings.Enabled);

                this.Bind(ViewModel, vm => vm.FeedUrl, view => view.textBoxFeedUrl.Text);
                this.BindCommand(ViewModel, vm => vm.TestFeedCommand, view => view.buttonFeedTest);

                this.Bind(ViewModel, vm => vm.OutputRootFolder, view => view.textBoxOutputFolder.Text);

                this.BindCommand(ViewModel, vm => vm.SelectRootFolderCommand, view => view.buttonSelectOutputFolder);
                this.BindCommand(ViewModel, vm => vm.OpenRootFolderCommand, view => view.buttonOpenOutputFolder);

                this.Bind(ViewModel, vm => vm.IsPriceRounding, view => view.checkBoxPriceRounding.Checked);

                this.Bind(ViewModel, vm => vm.DescriptionStart, view => view.textBoxStartDescriptions.Text);
                this.Bind(ViewModel, vm => vm.DescriptionEnd, view => view.textBoxEndDescriptions.Text);

                this.BindCommand(ViewModel, vm => vm.SaveSettingsCommand, view => view.buttonSaveSettings);

                this.Bind(ViewModel, vm => vm.GetProductsButtonText, view => view.buttonGetProducts.Text);
                this.BindCommand(ViewModel, vm => vm.GetProductsCommand, view => view.buttonGetProducts);

                this.Bind(ViewModel, vm => vm.ProgressText, view => view.labelProgress.Text);
                this.Bind(ViewModel, vm => vm.MaxProgress, view => view.progressBarGetProducts.Maximum);
                this.Bind(ViewModel, vm => vm.CurrentProgress, view => view.progressBarGetProducts.Value);

                this.FormClosing += async (_, e) =>
                {
                    var result = await ViewModel.IsSavingSettingsAsync();
                    if (result == true)
                    {
                        Settings.Default.Save();
                    }
                    else if(result == null)
                    {
                        e.Cancel = true;
                    }
                };

                ViewModel.AppendLogAction = x =>
                {
                    ViewModel.SafeAction(() => textBoxLog.AppendText(x));
                };
            });

            ViewModel = new MainViewModel();
        }
        public MainViewModel ViewModel { get; set; }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)ViewModel;
        }


    }
}
