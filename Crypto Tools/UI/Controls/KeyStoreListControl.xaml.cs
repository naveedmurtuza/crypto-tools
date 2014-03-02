using System.Windows.Controls;
using System.Windows.Input;
using CryptoTools.ViewModels;

namespace CryptoTools.UI.Controls
{
    /// <summary>
    /// Interaction logic for KeyStoreListControl.xaml
    /// </summary>
    public partial class KeyStoreListControl : UserControl
    {
        public KeyStoreListControl()
        {
            InitializeComponent();
            //this.DataContext = viewModel;
        }

        private void listView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Finally someone said  this!
            //"Please, code behind is not a bad thing at all. Unfortunately, quite a lot people in the WPF community got this wrong."  - jbe
            //http://stackoverflow.com/questions/1035023/firing-a-double-click-event-from-a-wpf-listview-item-using-mvvm

            //if (listView1.SelectedIndex != -1)
            //{
            //    MainViewModelEx.OpenListViewItemCommand.Execute(listView1.SelectedItem);
            //}
        }
    }
}
