using System;
using System.IO;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Utilities;

namespace CryptoTools.ViewModels
{
    public class SelectCertCrlViewModel : ViewModelBase
    {
        private ICommand _browseCertCmd;
        private ICommand _cmd;
        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }

        public ICommand ExamineCommand
        {
            get { return _cmd ?? (_cmd = new RelayCommand(p => ExamineCryptObject(), p => CanExecute())); }
        }

        public ICommand BrowseCertificateCommand
        {
            get { return _browseCertCmd ?? (_browseCertCmd = new RelayCommand(param => BrowseCertificate())); }
        }

        public override string Name
        {
            get { return "Certificate/CRL"; }
            protected set { }
        }

        public override string ImageSource
        {
            get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/certificate.png"; }
        }

        private bool CanExecute()
        {
            return !String.IsNullOrEmpty(Path) && File.Exists(Path);
        }

        private void ExamineCryptObject()
        {
            CryptUI.CryptUIViewContext(Path, "Crypto Tools", IntPtr.Zero);
        }

        private void BrowseCertificate()
        {
            String filename;
            FileUtils.OpenFileUI("Select Certificate/Crl", out filename,
                                 "Certificate/Crl Files|*.cer;*.crt;*.pem;*.crl;*.der|All Files|*.*");
            if (filename != null)
            {
                Path = filename;
            }
        }
    }
}