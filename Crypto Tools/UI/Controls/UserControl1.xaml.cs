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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CryptoTools.ViewModels;

namespace CryptoTools.UI.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Is it possible to do this XAML
            //tried binding the passwordBox itself as 
            //suggested in http://stackoverflow.com/questions/5863826/wpf-commandparameter-binding-to-passwordbox-password
            //but no luck :(
            var v = chkcboxKeyusage.Text;
            var viewmodel = (X509CertViewModel)DataContext;
            viewmodel.GenerateCommand.Execute(new object[] {passwordBox1,passwordBox2});
        }
    }
}
