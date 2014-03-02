using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using CryptoTools.Utilities;
using CryptoTools.ViewModels;

namespace CryptoTools.Models
{
    public class MainMenuModel : Observable
    {
        private ObservableCollection<IPageViewModel> _items;
        private bool _isEnabled;
        public string Header { get; set; }
        public string ImageSource { get; set; }
        public string Description { get; set; }
        
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public ObservableCollection<IPageViewModel> Items
        {
            get
            {
                return (_items = _items ??
                                 new ObservableCollection<IPageViewModel>());
            }
        }

        public bool HasChildren
        {
            get { return Items.Count > 0; }
        }

    }
}
