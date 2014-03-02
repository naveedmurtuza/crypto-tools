using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Helpers;
using CryptoTools.Models;
using CryptoTools.Utilities;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace CryptoTools.ViewModels
{
    public class PbkdfViewModel : ViewModelBase
    {
        public PbkdfModel Model { get; set; }
        private IEnumerable<TypeWrapper> _dfSource;
        private IEnumerable<TypeWrapper> _digestSource;
        private ICommand _genPasswdCmd;
        public PbkdfViewModel()
        {
            Model = new PbkdfModel();
        }
        public IEnumerable<TypeWrapper> DerivationFunctionsSource
        {
            get { return _dfSource ?? (_dfSource = Reflection.FindImplementations<BaseKdfBytesGenerator>(typeof(IDigest))); }
        }

        public IEnumerable<TypeWrapper> DigestSource
        {
            get { return _digestSource ?? (_digestSource = Reflection.FindImplementations<IDigest>()); }
        }

        public ICommand GeneratePasswordCommand
        {
            get { return _genPasswdCmd ?? (_genPasswdCmd = new RelayCommand(param => GenerateKey(), param => CanExecute())); }
        }

        private void GenerateKey()
        {
            var sb = new StringBuilder();
            sb.Append("Error generating Key").Append(Environment.NewLine);
            if(Model.Password.IsNullOrEmpty())
            {
                sb.Append("Enter Password!").Append(Environment.NewLine);
            }
            if(Model.Salt.IsNullOrEmpty())
            {
                sb.Append("Enter Salt!").Append(Environment.NewLine);
            }
            if (Model.KeySize <= 0)
            {
                sb.Append("Enter KeySize!").Append(Environment.NewLine);
            }
            if (!Model.BouncyImplementation && Model.Iterations <= 0)
            {
                sb.Append("Enter Iterations value!").Append(Environment.NewLine);
            }
            if (Model.BouncyImplementation && Model.Digest == null)
            {
                sb.Append("Select Digest!").Append(Environment.NewLine);
            }
            if (Model.BouncyImplementation && Model.DerivationFunction == null)
            {
                sb.Append("Select derivation function!").Append(Environment.NewLine);
            }
            var errors = sb.ToString();
            if (!errors.IsNullOrEmpty())
            {
                MessageBoxContent = new MessageBoxViewModel(CloseMessageBox,
                                                            MessageBoxModel.Error(errors));
                IsMessageBoxVisible = true;
                return;
            }
            byte[] password = Encoding.UTF32.GetBytes(Model.Password);
            byte[] salt = Encoding.UTF32.GetBytes(Model.Salt);

            if (Model.BouncyImplementation)
            {
                Model.Key = new byte[Model.KeySize];
                var digest = Model.Digest.Instance<IDigest>();
                var kdf = Model.DerivationFunction.Instance<BaseKdfBytesGenerator>(digest);

                var kdfParams = new KdfParameters(password, salt);
                kdf.Init(kdfParams);
                kdf.GenerateBytes(Model.Key, 0, Model.Key.Length);
            }
            else
            {
                var k1 = new Rfc2898DeriveBytes(password, salt, Model.Iterations);
                Model.Key = k1.GetBytes(Model.KeySize);
            }
        }

        public bool CanExecute()
        {
            // TODO: 
            return true;
        }

        public override string Name { get { return "Password Generator"; } protected set { }
        }
        public override string ImageSource { get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/password.png"; } }

    }
}
