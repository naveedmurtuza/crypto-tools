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
    /// Interaction logic for ExportKeyControl.xaml
    /// </summary>
    public partial class ExportKeyControl : UserControl
    {
        public ExportKeyControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (ExportKeyViewModel) DataContext;
            if(dataContext != null)
            {
                dataContext.Command.Execute(new object[] { passwordBox1 ,passwordBox2});
            }
        }
    }
}
