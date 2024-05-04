using Juniansoft.MvvmReady;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlFeedReader.Properties;
using XmlFeedReader.Services;

namespace XmlFeedReader.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        private readonly AssemblyService _assembly;
        private readonly ILogger _log;

        public Action<Action> SafeAction { get; set; }

        public MainViewModel()
        {
            _assembly = ServiceLocator.Current.Get<AssemblyService>();

            _log = ServiceLocator.Current.Get<ILogger>();

            AppTitle = $"{_assembly.AssemblyProduct} - v{_assembly.AssemblyVersion}";
            AppIcon = Resources.Favicon;
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
    }
}
