using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Helpers;
using CryptoTools.UI.MessageBox;
using CryptoTools.Utilities;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using SystemX509 = System.Security.Cryptography.X509Certificates;

namespace CryptoTools.ViewModels
{
    public class KeyStoreViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ListItemEntry> _listItems = new ObservableCollection<ListItemEntry>();
        private readonly char[] _password;
        private readonly Pkcs12Store _store;
        private readonly X509CertViewModel _x509CertViewModel;
        private bool _changed;
        private ICommand _exportCertCmd;
        private ICommand _exportKeyCmd;
        private ICommand _exportPkcs12Cmd;
        private ICommand _genCertCmd;
        private string _name = "Open";
        private ICommand _openCrlDirCmd;
        private ICommand _publishCrlCmd;
        private bool _readOnly = true;
        private ICommand _revokeCmd;
        private bool _showCertGenerator;


        public KeyStoreViewModel(SystemX509.StoreName storeName, SystemX509.StoreLocation storeLocation)
        {
            var storeBuilder = new Pkcs12StoreBuilder();
            _store = storeBuilder.Build();
            Name = String.Format("System KeyStore: [{0} : {1}]", storeLocation.ToString(), storeName.ToString());
            Load(storeName, storeLocation);
        }

        /// <summary>
        /// Loads the keystore from the <code>Repository.Instance.CertificatesKeyStore</code>
        /// You shud call <code>Repository.Instance.OpenExistingCertificateAuthority</code>
        /// before calling this.
        /// </summary>
        /// <param name="password">password for the keystore</param>
        public KeyStoreViewModel(char[] password, String path)
        {
            if (!Repository.Instance.OpenExistingCertificateAuthority(path, password))
            {
                throw new Exception("Unable to open KeyStore. Invalid keystore directory");
            }
            Name = Path.GetFileName(path);
            ReadOnlyKeyStore = false;
            _password = password;
            _listItems.Clear();
            var storeBuilder = new Pkcs12StoreBuilder();
            _store = storeBuilder.Build();
            _x509CertViewModel = new X509CertViewModel((alias, cert, keypair) =>
                                                           {
                                                               AddEntry(alias, cert, keypair);
                                                               ShowCertificateGenerator = false;
                                                           });
            Load();
        }

        public X509CertViewModel X509CertViewModel
        {
            get { return _x509CertViewModel; }
        }

        public ObservableCollection<ListItemEntry> KeyStoreItems
        {
            get { return _listItems; }
        }

        public bool ShowCertificateGenerator
        {
            get { return _showCertGenerator; }
            set
            {
                _showCertGenerator = value;
                OnPropertyChanged("ShowCertificateGenerator");
            }
        }


        /// <summary>
        /// Indicator as to whether or not the keystore has been altered since its last save
        /// </summary>
        public bool Changed
        {
            get { return _changed; }
            set
            {
                _changed = value;
                OnPropertyChanged("Changed");
            }
        }

        /// <summary>
        /// Indicator as to whether the keystore is readonly
        /// </summary>
        public bool ReadOnlyKeyStore
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                OnPropertyChanged("ReadOnlyKeyStore");
            }
        }

        /// <summary>
        /// Indicator as to whether the keystore is readonly
        /// </summary>
        public bool NotReadOnlyKeyStore
        {
            get { return !ReadOnlyKeyStore; }
        }

        public ICommand OpenCrlDirectoryCommand
        {
            get
            {
                if (_openCrlDirCmd == null)
                {
                    _openCrlDirCmd =
                        new RelayCommand(
                            param =>
                            FileUtils.OpenDirectoryInExplorer(Path.GetDirectoryName(Repository.Instance.CrlFilePath),
                                                              false));
                }
                return _openCrlDirCmd;
            }
        }

        public ICommand PublishCrlCommand
        {
            get
            {
                if (_publishCrlCmd == null)
                {
                    _publishCrlCmd = new RelayCommand(param => Repository.Instance.PublishCrl());
                }
                return _publishCrlCmd;
            }
        }

        public ICommand RevokeEntryCommand
        {
            get
            {
                if (_revokeCmd == null)
                {
                    _revokeCmd = new RelayCommand(RevokeEntry, param => !ReadOnlyKeyStore);
                }
                return _revokeCmd;
            }
        }

        public ICommand ExportKeyCommand
        {
            get
            {
                if (_exportKeyCmd == null)
                {
                    _exportKeyCmd = new RelayCommand(ExportKey, param => !ReadOnlyKeyStore);
                }
                return _exportKeyCmd;
            }
        }

        public ICommand ExportPkcs12Command
        {
            get
            {
                if (_exportPkcs12Cmd == null)
                {
                    _exportPkcs12Cmd = new RelayCommand(ExportPkcs12, param => !ReadOnlyKeyStore);
                }
                return _exportPkcs12Cmd;
            }
        }

        public ICommand ExportCertificateCommand
        {
            get
            {
                if (_exportCertCmd == null)
                {
                    _exportCertCmd = new RelayCommand(ExportCertificate);
                }
                return _exportCertCmd;
            }
        }

        public ICommand GenerateCertCommand
        {
            get
            {
                if (_genCertCmd == null)
                {
                    _genCertCmd = new RelayCommand(param => ShowCertificateGenerator = true);
                }
                return _genCertCmd;
            }
        }

        public override string Name
        {
            get { return _name; }
            protected set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public override string ImageSource
        {
            get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/folder.png"; }
        }

        public override bool CanClose
        {
            get
            {
                if (Changed)
                {
                    MessageBoxResult dr =
                        FxMessageBox.Show(String.Format("Do you want to save the changes to {0}", Name), "Crypto Tools",
                                          MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (dr == MessageBoxResult.Yes)
                    {
                        Save();
                    }
                }
                else
                {
                    MessageBoxResult dr = FxMessageBox.Show(String.Format("Close KeyStore - {0}", Name), "Crypto Tools",
                                                            MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    return dr == MessageBoxResult.Yes;
                }
                return true;
            }
        }

        public override bool HasBackStack
        {
            get { return ShowCertificateGenerator; }
        }

        public override void Back()
        {
            ShowCertificateGenerator = false;
        }

        /// <summary>
        /// Loads the specified system keystore and puts
        /// the keys store in read-only mode
        /// </summary>
        /// <param name="storeName">one of the enum values </param>
        /// <param name="storelocation">one of the enum values</param>
        public void Load(SystemX509.StoreName storeName, SystemX509.StoreLocation storelocation)
        {
            ReadOnlyKeyStore = true;
            _listItems.Clear();
            var store = new SystemX509.X509Store(storeName, storelocation);
            store.Open(SystemX509.OpenFlags.ReadOnly);
            foreach (SystemX509.X509Certificate2 cert in store.Certificates)
            {
                AddEntry(cert.FriendlyName, DotNetUtilities.FromX509Certificate(cert), null);
            }
        }

        /// <summary>
        /// Loads the keystore from the <code>Repository.Instance.CertificatesKeyStore</code>
        /// You shud call <code>Repository.Instance.OpenExistingCertificateAuthority</code>
        /// before calling this.
        /// </summary>
        private void Load()
        {
            //load the keystore
            //check if it exists first
            if (File.Exists(Repository.Instance.CertificatesKeyStore))
            {
                using (Stream istream = File.Open(Repository.Instance.CertificatesKeyStore, FileMode.Open))
                {
                    _store.Load(istream, _password);
                }
                RefreshStore();
            }
        }

        /// <summary>
        /// Adds entry to the keystore. This method updates the UI
        /// so it shud be called from UI Thread.
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="cert"></param>
        /// <param name="keypair"></param>
        public void AddEntry(String alias, X509Certificate cert, AsymmetricCipherKeyPair keypair)
        {
            if (keypair == null)
            {
                _store.SetCertificateEntry(alias, X509Utils.WrapX509Certificates(cert)[0]);
                _listItems.Add(new ListItemEntry(KeyStoreEntryType.TrustCertEntry, alias, cert));
            }
            else
            {
                _store.SetKeyEntry(alias, new AsymmetricKeyEntry(keypair.Private), X509Utils.WrapX509Certificates(cert));
                _listItems.Add(new ListItemEntry(KeyStoreEntryType.KeyPairEntry, alias, cert));
            }
            if (!ReadOnlyKeyStore)
            {
                //signal that it has been changed
                Changed = true;
                Save();
            }
        }

        /// <summary>
        /// Saves the existing keystore to <code>Repository.Instance.CertificatesKeyStore</code>
        /// </summary>
        public void Save()
        {
            String tmpBroFile = FileUtils.GetTempBrotherFileName(Repository.Instance.CertificatesKeyStore);
            Save(tmpBroFile);
            if (File.Exists(Repository.Instance.CertificatesKeyStore))
                File.Delete(Repository.Instance.CertificatesKeyStore);
            File.Move(tmpBroFile, Repository.Instance.CertificatesKeyStore);
            Changed = false;
        }

        /// <summary>
        /// Saves the existing keystore to the given path
        /// </summary>
        /// <param name="filename">Absolute path to save keystore</param>
        public void Save(String filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                _store.Save(stream, _password, Repository.Srand);
            }
        }

        private void RefreshStore()
        {
            _listItems.Clear();
            foreach (String alias in _store.Aliases)
            {
                KeyStoreEntryType entryType;
                if (_store.IsCertificateEntry(alias))
                {
                    entryType = KeyStoreEntryType.TrustCertEntry;
                }
                else if (_store.IsKeyEntry(alias) && _store.GetCertificateChain(alias) != null &&
                         _store.GetCertificateChain(alias).Length != 0)
                {
                    entryType = KeyStoreEntryType.KeyPairEntry;
                }
                else
                    entryType = KeyStoreEntryType.KeyEntry;
                X509Certificate cert = _store.GetCertificate(alias).Certificate;
                _listItems.Add(new ListItemEntry(entryType, alias, cert,
                                                 Repository.Instance.IsRevokedCertificate(cert.SerialNumber.ToString())));
            }
        }

        private void ExportCertificate(object param)
        {
            var entry = (ListItemEntry) param;
            String filename;
            FileUtils.SaveFileUI("Select the path to store certificate", out filename);
            if (filename != null)
            {
                File.WriteAllText(filename, PemUtilities.Encode(entry.Certificate));
            }
        }

        private void ExportKey(object param)
        {
            var entry = (ListItemEntry) param;
            if (_store.IsEntryOfType(entry.Alias, typeof (AsymmetricKeyEntry)))
            {
                MessageBoxContent = new ExportKeyViewModel(entry, ExportKey, CloseMessageBox);
                IsMessageBoxVisible = true;
            }
        }


        private void ExportKey(ListItemEntry entry, String algorithm, char[] password, String path)
        {
            AsymmetricKeyEntry keyentry = _store.GetKey(entry.Alias);
            string pem = PemUtilities.Encode(keyentry.Key, algorithm, password,
                                             Repository.Srand);
            File.WriteAllText(path, pem);
        }

        private void ExportPkcs12(object param)
        {
            var entry = (ListItemEntry) param;
            if (_store.IsEntryOfType(entry.Alias, typeof (AsymmetricKeyEntry)))
            {
                MessageBoxContent = new ExportKeyViewModel(entry, ExportPkcs12, CloseMessageBox);
                IsMessageBoxVisible = true;
            }
        }

        private void ExportPkcs12(ListItemEntry entry, String path, char[] password)
        {
            AsymmetricKeyEntry keyentry = _store.GetKey(entry.Alias);
            X509CertificateEntry certEntry = _store.GetCertificate(entry.Alias);
            X509Utils.ExportPKCS12(path, entry.Alias, keyentry, password, certEntry);
        }

        private void RevokeEntry(object param)
        {
            var entry = (ListItemEntry) param;

            string serial = entry.Certificate.SerialNumber.ToString();
            //check if the serial is already revoked.
            //if yes unrevoke it,revoke otherwise
            if (Repository.Instance.IsRevokedCertificate(entry.Certificate.SerialNumber.ToString()))
            {
                //already revoked
                //unrevoke it
                Repository.Instance.RemoveRevokedSerial(serial);
                entry.Revoked = false;
            }
            else
            {
                MessageBoxContent = new RevokeCertViewModel(entry, RevokeEntry, CloseMessageBox);
                IsMessageBoxVisible = true;
            }
        }

        private void RevokeEntry(ListItemEntry entry, String serial, int reason, DateTime now)
        {
            Repository.Instance.AddRevokedSerial(serial, reason, now);
            entry.Revoked = true;
        }
    }
}