using Juniansoft.MvvmReady;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XmlFeedReader.Services
{
    public class DialogService
    {
        private readonly AssemblyService _assemblyService;
        public DialogService() 
        { 
            _assemblyService = ServiceLocator.Current.Get<AssemblyService>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>True if Yes, False if No, Null if Cancel</returns>
        public Task<bool?> ShowYesNoAsync(string text)
        {
            return Task.FromResult(ShowYesNo(text));
        }

        public bool? ShowYesNo(string text)
        {
            var result = MessageBox.Show(
                owner: Program.MainForm,
                text: text,
                caption: _assemblyService.AssemblyProduct,
                buttons: MessageBoxButtons.YesNoCancel);

            if( result == DialogResult.Yes)
                return true;
            else if( result == DialogResult.No)
                return false;

            return null;
        }

        public Task ShowMessageAsync(string caption)
        {
            ShowMessage(caption);
            return Task.CompletedTask;
        }

        public void ShowMessage(string text)
        {
            MessageBox.Show(
                owner: Program.MainForm, 
                text: text, 
                caption: _assemblyService.AssemblyProduct);
        }
        public Task<string> ShowFolderBrowserAsync()
        {
            return Task.FromResult(ShowFolderBrowser());
        }

        public string ShowFolderBrowser()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return fbd.SelectedPath;
                }
            }

            return null;
        }
    }
}
