using System.ComponentModel;
using System.Windows;
using CryptoTools.Models;

namespace CryptoTools.UI.MessageBox
{
    /// <summary>
    /// Interaction logic for MessageBoxControl.xaml
    /// </summary>
    public partial class MessageBoxControl 
    {
        public MessageBoxControl(MessageBoxControlModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }

    
}
