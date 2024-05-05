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

        public Task ShowMessageAsync(string caption)
        {
            ShowMessageBox(caption);
            return Task.CompletedTask;
        }

        public void ShowMessageBox(string text)
        {
            MessageBox.Show(
                owner: Program.MainForm, 
                text: text, 
                caption: _assemblyService.AssemblyProduct);
        }
        public Task<string> ShowFolderBrowserDialogAsync()
        {
            return Task.FromResult(ShowFolderBrowserDialog());
        }

        public string ShowFolderBrowserDialog()
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
