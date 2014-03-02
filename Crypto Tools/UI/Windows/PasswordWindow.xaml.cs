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

namespace CryptoTools.UI.Windows
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        public char[] Password
        {
            get
            {
                return passwordBox1.Password.ToCharArray();
            }
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
            //if (!String.IsNullOrEmpty(passwordBox1.Password))
            //{
               
            //}
        }
    }
}
