using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CryptoTools.Utilities;

namespace CryptoTools.UI.Windows
{
    /// <summary>
    /// Interaction logic for ExportKeyWindow.xaml
    /// </summary>
    public partial class ExportKeyWindow : Window
    {
        public ExportKeyWindow()
        {
            InitializeComponent();
            cboxAlgo.ItemsSource = PemUtilities.Algorithims;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            String file;
            FileUtils.SaveFileUI("Select path to save key", out file);
            if (file != null)
                txtPath.Text = file;
        }

        public char[] Password
        {
            get { return passwordBox1.Password.ToCharArray(); }
        }

        public String Algorithm
        {
            get { return cboxAlgo.SelectedValue.ToString(); }
        }

        public String Path
        {
            get { return txtPath.Text; }
        }
    }
}
