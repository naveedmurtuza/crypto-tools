using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Utilities;
using CryptoTools.ViewModels;

namespace CryptoTools.UI.Controls
{
    /// <summary>
    /// Interaction logic for SelectCertCrlControl.xaml
    /// </summary>
    public partial class SelectCertCrlControl : UserControl
    {
        public SelectCertCrlControl()
        {
            InitializeComponent();
            //DataContext = new SelectCertCrlViewModel();
        }

        public void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;

            e.Handled = true;
        }

        private void textBox1_PreviewDrop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            //var formats = data.GetFormats();
            var converted = (string[]) data.GetData("FileNameW", true);
            textBox1.Text = converted[0];
        }
    }

    
}