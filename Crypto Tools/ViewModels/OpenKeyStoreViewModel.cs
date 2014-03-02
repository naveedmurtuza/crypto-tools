using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Models;
using CryptoTools.Utilities;
using SystemX509 = System.Security.Cryptography.X509Certificates;

namespace CryptoTools.ViewModels
{
    public class OpenKeyStoreViewModel : ViewModelBase
    {
        private readonly Action<IPageViewModel> _action;
        private readonly OpenKeyStoreModel model = new OpenKeyStoreModel();
        private ICommand _caRepoDir;
        private String _name = "Open";
        private ICommand _openKeyStore;
        private ICommand _openSystemCmd;

        public OpenKeyStoreViewModel(Action<IPageViewModel> action)
        {
            _action = action;
        }

        public ICommand OpenSystemCommand
        {
            get { return _openSystemCmd ?? (_openSystemCmd = new RelayCommand(Load)); }
        }

        public ICommand OpenKeyStoreCommand
        {
            get
            {
                return _openKeyStore ??
                       (_openKeyStore = new RelayCommand(Load, param => CanOpenKeyStore()));
            }
        }

        public ICommand BrowseFolderCARepo
        {
            get { return _caRepoDir ?? (_caRepoDir = new RelayCommand(param => BrowseCARepository())); }
        }

        public OpenKeyStoreModel Model
        {
            get { return model; }
        }

        public List<String> X509StoreNames
        {
            get { return Enum.GetNames(typeof (SystemX509.StoreName)).ToList(); }
        }

        public List<String> X509StoreLocations
        {
            get { return Enum.GetNames(typeof (SystemX509.StoreLocation)).ToList(); }
        }

        public override string Name
        {
            get { return _name; }
            protected set { }
        }

        public override string ImageSource
        {
            get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/folder.png"; }
        }

        public bool CanOpenKeyStore()
        {
            return !Model.KeyStorePath.IsNullOrEmpty();
        }

        private void Load(object o)
        {
            if (o == null)
                _action.Invoke(new KeyStoreViewModel(Model.SystemStoreName, Model.SystemStoreLocation));
            else if (o is PasswordBox)
            {
                char[] password = ((PasswordBox) o).Password.ToCharArray();
                try
                {
                    _action.Invoke(new KeyStoreViewModel(password, Model.KeyStorePath));
                }
                catch (Exception ex)
                {
                    MessageBoxContent = new MessageBoxViewModel(CloseMessageBox,
                                                                MessageBoxModel.Error(ex.Message, "Details",
                                                                                      ex.StackTrace));
                    IsMessageBoxVisible = true;
                }
            }
        }

        private void BrowseCARepository()
        {
            String filename;
            FileUtils.FolderBrowserUI("Select CA Repository", out filename);
            if (filename != null)
            {
                Model.KeyStorePath = filename;
            }
        }


    }
}