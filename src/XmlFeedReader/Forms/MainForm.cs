using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

                this.Bind(ViewModel, vm => vm.FeedUrl, view => view.textBoxFeedUrl.Text);
                this.Bind(ViewModel, vm => vm.OutputRootFolder, view => view.textBoxOutputFolder.Text);

                this.BindCommand(ViewModel, vm => vm.SelectRootFolderCommand, view => view.buttonSelectOutputFolder);
                this.BindCommand(ViewModel, vm => vm.OpenRootFolderCommand, view => view.buttonOpenOutputFolder);

                this.Bind(ViewModel, vm => vm.IsPriceRounding, view => view.checkBoxPriceRounding.Checked);

                this.Bind(ViewModel, vm => vm.StartDescriptions, view => view.textBoxStartDescriptions.Text);
                this.Bind(ViewModel, vm => vm.EndDescriptions, view => view.textBoxEndDescriptions.Text);


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
