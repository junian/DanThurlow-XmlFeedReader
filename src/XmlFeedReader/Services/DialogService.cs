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
