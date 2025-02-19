﻿using Juniansoft.MvvmReady;
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
                buttons: MessageBoxButtons.YesNoCancel,
                icon: MessageBoxIcon.Question);

            if( result == DialogResult.Yes)
                return true;
            else if( result == DialogResult.No)
                return false;

            return null;
        }

        public Task ShowSuccessAsync(string caption)
        {
            ShowSuccess(caption);
            return Task.CompletedTask;
        }

        public void ShowSuccess(string text)
        {
            MessageBox.Show(
                owner: Program.MainForm,
                text: text,
                caption: _assemblyService.AssemblyProduct,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Hand);
        }

        public Task ShowErrorAsync(string caption)
        {
            ShowError(caption);
            return Task.CompletedTask;
        }

        public void ShowError(string text)
        {
            MessageBox.Show(
                owner: Program.MainForm,
                text: text,
                caption: _assemblyService.AssemblyProduct,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Error);
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
                caption: _assemblyService.AssemblyProduct,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.None);
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
