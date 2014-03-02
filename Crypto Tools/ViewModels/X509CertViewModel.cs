using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using CryptoTools.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CryptoTools.Helpers;
using Org.BouncyCastle.Asn1.X509;
using CryptoTools.Utilities;
using Org.BouncyCastle.Crypto;
using System.Windows.Input;
using CryptoTools.Command;
using Org.BouncyCastle.X509;

namespace CryptoTools.ViewModels
{
    public class X509CertViewModel : ViewModelBase 
    {
        private X509CertModel _model;

        public X509CertModel Model
        {
            get { return _model; }
            set
            {
                if (_model != null)
                    _model.PropertyChanged -= ModelPropertyChanged;
                _model = value;
                _model.PropertyChanged += ModelPropertyChanged;
            }
        }

        private readonly ObservableCollection<TypeWrapper> _keypairGen = new ObservableCollection<TypeWrapper>();
        private readonly ObservableCollection<String> _sigAlgorithm = new ObservableCollection<String>();
        private ICommand _newKeyStoreCommand, _newCertificateCommand, _caRepoDir;
        private readonly Action<IPageViewModel> _newCaCompletedAction;
        private readonly Action<String, X509Certificate, AsymmetricCipherKeyPair> _newCertCompletedAction;

        /// <summary>
        ///
        /// </summary>
        /// <param name="newCaCompletedAction"></param>
        /// <param name="isCa"></param>
        private X509CertViewModel(Action<IPageViewModel> newCaCompletedAction,Action<String, X509Certificate, AsymmetricCipherKeyPair> newCertCompletedAction, bool isCa)
        {
            Model = new X509CertModel { IsCertificateAuthority = isCa };
            Model.PropertyChanged += ModelPropertyChanged;
            FillSigAlgoAndKeyGens();
            _newCaCompletedAction = newCaCompletedAction;
            _newCertCompletedAction = newCertCompletedAction;
            GenerateCommand = isCa ? NewKeyStoreCommand : NewCertificateCommand;
        }

        public X509CertViewModel(Action<IPageViewModel> action) : this(action,null, true)
        {
        }

        public X509CertViewModel(Action<String, X509Certificate, AsymmetricCipherKeyPair> action)
            : this(null, action, false)
        {
        }

        public 
        void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "KeyPairType":
                    FillSigAlgoAndKeyGens();
                    break;
            }
        }

        private void FillSigAlgoAndKeyGens()
        {
            _sigAlgorithm.Clear();
            foreach (var item in SignatureAlgorithms.ValuesFor(Model.KeyPairType))
            {
                _sigAlgorithm.Add(item);
            }
            IEnumerable<TypeWrapper> gens =
                Reflection.FindImplementations<IAsymmetricCipherKeyPairGenerator>().Where(
                    key => key.ToString().ToLower().Contains(Enum.GetName(typeof(KeyPairType), Model.KeyPairType).ToLower()));
            _keypairGen.Clear();
            foreach (var item in gens)
                _keypairGen.Add(item);
        }

        #region ItemsSources

        public IEnumerable<KeyPairType> KeyPairTypesSource
        {
            get
            {
                return Model.IsCertificateAuthority ? Enum.GetValues(typeof(KeyPairType)).Cast<KeyPairType>() : new[] { Repository.Instance.KeyPairType };
            }
        }

        public IEnumerable<KeyUsageWrapper<KeyPurposeID>> ExtendedKeyUsagesSource
        {
            get
            {
                return KeyUsageUtils.GetExtendedKeyUsages();
            }
        }

        public IEnumerable<KeyUsageWrapper<Int32>> KeyUsagesSource
        {
            get
            {
                return KeyUsageUtils.GetKeyUsages();
            }
        }
        public ObservableCollection<TypeWrapper> KeyPairGeneratorSource
        {
            get
            {
                return _keypairGen;
            }
        }

        
        public ObservableCollection<String> SignatureAlgorthmsSource
        {
            get
            {
                return _sigAlgorithm;
            }
        }
        #endregion

        #region ICommands

        public ICommand GenerateCommand { private set; get; }
        

        public ICommand BrowseFolderCARepo
        {
            get { return _caRepoDir ?? (_caRepoDir = new RelayCommand(param => BrowseCARepository())); }
        }

        public ICommand NewKeyStoreCommand
        {
            get
            {
                return _newKeyStoreCommand ??
                   (_newKeyStoreCommand = new RelayCommand(NewKeyStore, param => CanExecuteCommand()));
            }
        }

        public ICommand NewCertificateCommand
        {
            get
            {
                return _newCertificateCommand ??
                       (_newCertificateCommand =
                        new RelayCommand(NewCertificate, param => CanExecuteCommand()));
            }
        }

        #endregion

        private StringBuilder ValidateInputs(object[] passwordBoxes)
        {
            StringBuilder sb = new StringBuilder(ValidateInputs().ToString());
            char[] password1 = ((PasswordBox) passwordBoxes[0]).Password.ToArray();
            char[] password2 = ((PasswordBox)passwordBoxes[1]).Password.ToArray();
            if(password1.Length == 0)
            {
                sb.Append("Please enter password").Append(Environment.NewLine);
            }
            if (!password1.SequenceEqual(password2))
            {
                sb.Append("password and confirm do not match").Append(Environment.NewLine);
            }
            if (Model.CARepositoryPath.IsNullOrEmpty())
                sb.Append("Please select a CARepositoryPath").Append(Environment.NewLine);
            Array.Clear(password1,0,password1.Length);
            Array.Clear(password2, 0, password2.Length);
            return sb;

        }
        private StringBuilder ValidateInputs()
        {
            StringBuilder sb = new StringBuilder();
            if (Model.CommonName.IsNullOrEmpty())
                sb.Append("Please enter a Common Name").Append(Environment.NewLine);
            if(Model.KeyPairGenerator == null)
                sb.Append("Please select a Key Pair Generator").Append(Environment.NewLine);
            if (Model.KeyStrength <= 0)
                sb.Append("Please enter Key Strength").Append(Environment.NewLine);
            if (Model.Validity <= 0)
                sb.Append("Please enter Validity of Certificate").Append(Environment.NewLine);
            if (Model.SignatureAlgorithm.IsNullOrEmpty())
                sb.Append("Please select a Signature Algorithm").Append(Environment.NewLine);
            return sb;
        }
        void NewKeyStore(object o)
        {
            var passwordBoxes = (object[]) o;
            
            var errors = ValidateInputs(passwordBoxes);
            if(!errors.ToString().IsNullOrEmpty())
            {
                MessageBoxContent = new MessageBoxViewModel(CloseMessageBox,
                                                            MessageBoxModel.Error("Errors generating Certificate - " +
                                                                                  Environment.NewLine +
                                                                                  errors.ToString()));
                IsMessageBoxVisible = true;
                return;
            }
            var password = ((PasswordBox)passwordBoxes[0]).Password.ToCharArray();
            var keygen = KeyPairUtils.CreateGenerator(Repository.Srand, Model.KeyPairGenerator, Model.KeyStrength);
            var keypair = keygen.GenerateKeyPair();
            var repo = Repository.Instance;
            repo.NewCertificateAuthority(Model.CARepositoryPath,password);
            repo.KeyPairType = Model.KeyPairType;
            var cert = X509Utils.GenerateCACertificate(Model.X509Name,
                                             Model.Validity,
                                             keypair.Public,
                                             keypair.Private,
                                             Model.SignatureAlgorithm,
                                             KeyUsageUtils.GetKeyUsage(Model.KeyUsages),
                                             Model.ExtendedKeyUsages == null ? null : new ExtendedKeyUsage(KeyUsageUtils.GetExtendedKeyUsages(Model.ExtendedKeyUsages)),
                                             Model.PathLenContraint);
            X509Utils.ExportPKCS12(Repository.Instance.CAKeyStore, /*Model.CommonName*/ "ca", keypair.Private, password, cert);
            File.WriteAllText(Repository.CaPfxFilename, PemUtilities.Encode(cert));
            _newCaCompletedAction.Invoke(new KeyStoreViewModel(password, Model.CARepositoryPath));
            //KeyStoreViewModelEx.Instance.Load(password);

        }

        void NewCertificate(object o)
        {
            var errors = ValidateInputs();
            if (!errors.ToString().IsNullOrEmpty())
            {
                MessageBoxContent = new MessageBoxViewModel(CloseMessageBox,
                                                            MessageBoxModel.Error("Errors generating Certificate - " +
                                                                                  Environment.NewLine +
                                                                                  errors.ToString()));
                IsMessageBoxVisible = true;
                return;
            }
            var keygen = KeyPairUtils.CreateGenerator(Repository.Srand, Model.KeyPairGenerator, Model.KeyStrength);
            var keypair = keygen.GenerateKeyPair();
            //load the ca pfx file
            var caStore = X509Utils.LoadCAPfx(Repository.Instance.KeyStorePassword);
            var caCert = caStore.GetCertificate("ca").Certificate;
            var caKey = caStore.GetKey("ca").Key;
            var cert = X509Utils.GenerateUserCertificate(Model.X509Name,
                                                         caCert.SubjectDN,
                                                         Model.Validity,
                                                         keypair.Public,
                                                         caKey,
                                                         Model.SignatureAlgorithm,
                                                         KeyUsageUtils.GetKeyUsage(Model.KeyUsages),
                                                         Model.ExtendedKeyUsages == null
                                                             ? null
                                                             : new ExtendedKeyUsage(
                                                                   KeyUsageUtils.GetExtendedKeyUsages(
                                                                       Model.ExtendedKeyUsages)));
            _newCertCompletedAction.Invoke(Model.CommonName, cert, keypair);
        }

        void BrowseCARepository()
        {
            String filename;
            FileUtils.FolderBrowserUI("Select CA Repository", out filename);
            if (filename != null)
                Model.CARepositoryPath = filename;
        }

        bool CanExecuteCommand()
        {
            return true; //and inform user of errors on generating
        }
        public override string Name { get { return "New"; } protected set { } }
        public override string ImageSource { get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/sun.png"; } }
        
    }
}
