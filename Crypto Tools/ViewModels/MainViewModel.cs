using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Models;
using CryptoTools.Utilities;

namespace CryptoTools.ViewModels
{
    public class MainViewModel : Observable
    {
        private ICommand _backCmd;
        private ICommand _changePageCommand;
        private IPageViewModel _currentPageViewModel;
        private ObservableCollection<MainMenuModel> _menuItems;
        private List<IPageViewModel> _pageViewModels;
        private string _selectedMenuItem;

        public MainViewModel()
        {
            FillMenuItems();
        }

        #region Properties / Commands

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel) p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }

        #endregion

        #region Methods

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        #endregion

        public ObservableCollection<MainMenuModel> MenuItems
        {
            get
            {
                return (_menuItems = _menuItems ??
                                     new ObservableCollection<MainMenuModel>());
            }
        }

        public string SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                _selectedMenuItem = value;
                OnPropertyChanged("SelectedMenuItem");
            }
        }


        public ICommand BackCommand
        {
            get { return _backCmd ?? (_backCmd = new RelayCommand(param => BrowseBack())); }
        }

        private void ChangeCurrentPageViewModel(IPageViewModel pvm)
        {
            CurrentPageViewModel = pvm;
        }

        private void FillMenuItems()
        {
            var keystore = new MainMenuModel
                               {
                                   Header = "KeyStore",
                                   Description = "",
                                   ImageSource = "Images/tiles/key.png"
                               };

            keystore.Items.Add(new X509CertViewModel(ChangeCurrentPageViewModel));
            keystore.Items.Add(new OpenKeyStoreViewModel(ChangeCurrentPageViewModel));

            var examine = new MainMenuModel
                              {
                                  Header = "Examine",
                                  Description = "",
                                  ImageSource = "Images/tiles/zoom.png",
                              };
            examine.Items.Add(new SelectCertCrlViewModel());
            examine.Items.Add(new ExamineSslViewModel());

            var tools = new MainMenuModel
                            {
                                Header = "Tools",
                                Description = "",
                                ImageSource = "Images/tiles/gear.png",
                            };
            tools.Items.Add(new GeneratePasswordViewModel());
            tools.Items.Add(new DigestViewModel());
            tools.Items.Add(new BlockCipherViewModel());

            var help = new MainMenuModel
                           {
                               Header = "Help",
                               Description = "",
                               ImageSource = "Images/tiles/help2.png",
                           };
            MenuItems.Add(keystore);
            MenuItems.Add(examine);
            MenuItems.Add(tools);
            MenuItems.Add(help);
        }


        private void BrowseBack()
        {
            if (CurrentPageViewModel.HasBackStack)
                CurrentPageViewModel.Back();
            else if (CurrentPageViewModel.CanClose)
                CurrentPageViewModel = null;
        }
    }
}