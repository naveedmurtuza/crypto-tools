using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Helpers;
using CryptoTools.Models;
using CryptoTools.Utilities;
using Org.BouncyCastle.Crypto;

namespace CryptoTools.ViewModels
{
    public class DigestViewModel : IPageViewModel


    {
        private readonly ObservableCollection<TypeWrapper> _digestsSource = new ObservableCollection<TypeWrapper>();
        private ICommand _browseFile;
        private ICommand _digestCmd;

        public DigestViewModel()
        {
            Model = new DigestModel();
            FillItemsSources();
            Model.PropertyChanged += ModelPropertyChanged;
        }

        public DigestModel Model { get; set; }


        public ObservableCollection<TypeWrapper> DigestSource
        {
            get { return _digestsSource; }
        }

        public ICommand BrowseFile
        {
            get { return _browseFile ?? (_browseFile = new RelayCommand(param => BrowseCARepository())); }
        }

        public ICommand ComputeDigest
        {
            get
            {
                if (_digestCmd == null)
                {
                    _digestCmd = new RelayCommand(param => ComputeHash(), param => CanExecute());
                }
                return _digestCmd;
            }
        }

        #region IPageViewModel Members

        public string Name
        {
            get { return "Hash"; }
        }

        public string ImageSource
        {
            get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/hash.png"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool HasBackStack
        {
            get { return false; }
        }

        public void Back()
        {
            throw new NotImplementedException();
        }

        #endregion

        private void BrowseCARepository()
        {
            String filename;
            FileUtils.OpenFileUI("Select File", out filename);
            if (filename != null)
                Model.FilePath = filename;
        }

        private void FillItemsSources()
        {
            _digestsSource.Clear();

            if (Model.BouncyImplementation)
            {
                _digestsSource.AddRange(Reflection.FindImplementations<IDigest>(Type.EmptyTypes));
            }
            else
            {
                _digestsSource.AddRange(
                    Reflection.FindImplementations<HashAlgorithm>(Type.EmptyTypes));
            }
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BouncyImplementation")
            {
                FillItemsSources();
            }
        }

        private void ComputeHash()
        {
            if (Model.BouncyImplementation)
            {
                var hash = Model.HashAlgorithm.Instance<IDigest>();
                byte[] buffer = File.ReadAllBytes(Model.FilePath);
                hash.BlockUpdate(buffer, 0, buffer.Length);
                var final = new byte[hash.GetDigestSize()];
                hash.DoFinal(final, 0);
                Model.Hash = final;
            }
            else
            {
                var hash = Model.HashAlgorithm.Instance<HashAlgorithm>();
                Model.Hash = hash.ComputeHash(File.ReadAllBytes(Model.FilePath));
            }
        }

        private bool CanExecute()
        {
            return (!Model.FilePath.IsNullOrEmpty() || !Model.Text.IsNullOrEmpty()) && File.Exists(Model.FilePath);
        }
    }
}