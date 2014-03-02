using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Helpers;
using CryptoTools.Models;
using CryptoTools.Utilities;
using Microsoft.Win32;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using SysSecurity = System.Security.Cryptography;

namespace CryptoTools.ViewModels
{
    public class BlockCipherViewModel : ViewModelBase
    {
        private const int Blocksize = 16;
        private readonly ObservableCollection<Object> _bcSource = new ObservableCollection<Object>();
        private readonly ObservableCollection<Object> _bcmSource = new ObservableCollection<Object>();
        private readonly ObservableCollection<Object> _bcpSource = new ObservableCollection<Object>();
        private ICommand _decryptCmd;
        private ICommand _encryptCmd;
        private ICommand _openFileCmd;
        private readonly PbkdfViewModel _pbkdfViewModel;
        public BlockCipherViewModel()
        {
            BlockCipherModel = new BlockCipherModel();
            _pbkdfViewModel = new PbkdfViewModel();
            FillItemsSources();
            BlockCipherModel.PropertyChanged += ModelPropertyChanged;
        }

        public PbkdfViewModel PbkdfViewModel { get { return _pbkdfViewModel; } }

        public PbkdfModel PbkdfModel { get { return _pbkdfViewModel.Model; } }
        public BlockCipherModel BlockCipherModel { get; set; }

        public IEnumerable<Object> PaddingSource
        {
            get { return _bcpSource; }
        }

        public IEnumerable<Object> CipherSource
        {
            get { return _bcSource; }
        }

        public IEnumerable<Object> CipherModeSource
        {
            get { return _bcmSource; }
        }

        public ICommand OpenFileCommand
        {
            get { return _openFileCmd ?? (_openFileCmd = new RelayCommand(param => OpenFile(), param => CanExecute())); }
        }

        public ICommand EncryptCommand
        {
            get { return _encryptCmd ?? (_encryptCmd = new RelayCommand(param => Process(true), param => CanExecute())); }
        }

        public ICommand DecryptCommand
        {
            get { return _decryptCmd ?? (_decryptCmd = new RelayCommand(param => Process(false), param => CanExecute())); }
        }

        private void FillItemsSources()
        {
            _bcpSource.Clear();
            _bcSource.Clear();
            _bcmSource.Clear();
            if (BlockCipherModel.BouncyImplementation)
            {
                _bcpSource.AddRange(Reflection.FindImplementations<IBlockCipherPadding>(Type.EmptyTypes));
                _bcSource.AddRange(Reflection.FindImplementations<IBlockCipher>(Type.EmptyTypes));
                _bcmSource.AddRange(Reflection.FindImplementations<IBlockCipher>(typeof (IBlockCipher)).Union(
                    Reflection.FindImplementations<IBlockCipher>(typeof (IBlockCipher), typeof (Int32))));
            }
            else
            {
                _bcSource.AddRange(Reflection.FindImplementations<SysSecurity.SymmetricAlgorithm>(Type.EmptyTypes));
                _bcmSource.AddRange(Enum.GetNames(typeof (SysSecurity.CipherMode)));
                _bcpSource.AddRange(Enum.GetNames(typeof (SysSecurity.PaddingMode)));
            }
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //refill the combo boxes the BouncyImplementation property changes
            if (e.PropertyName == "BouncyImplementation")
            {
                FillItemsSources();
            }
        }

        private bool CanExecute()
        {
            // TODO:
            return true;
        }

        private String Validate()
        {
            var sb = new StringBuilder();
            //check wether key is null or it contains Zeroes
            if(PbkdfModel.Key == null || (new byte[PbkdfModel.KeySize].SequenceEqual(PbkdfModel.Key)))
            {
                sb.Append("Please generate a key first!").Append(Environment.NewLine);
            }
            if (BlockCipherModel.Padding == null)
            {
                sb.Append("Please select padding").Append(Environment.NewLine);
            }
            if (BlockCipherModel.Mode == null)
            {
                sb.Append("Please select Mode").Append(Environment.NewLine);
            }
            if (BlockCipherModel.Engine == null)
            {
                sb.Append("Please select engine").Append(Environment.NewLine);
            }
            if (BlockCipherModel.Path.IsNullOrEmpty())
            {
                sb.Append("Please select a file to encrypt/decrypt").Append(Environment.NewLine);
            }
            return sb.ToString();
        }
        private void Process(bool forEncryption)
        {
            String errors = Validate();
            if (!errors.IsNullOrEmpty())
            {
                MessageBoxContent = new MessageBoxViewModel(CloseMessageBox, MessageBoxModel.Error(errors));
                IsMessageBoxVisible = true;
                return;
            }
            // TODO: 
            var iv = new byte[Blocksize];

            try
            {
                using (Stream istream = File.Open(BlockCipherModel.Path, FileMode.Open))
                {
                    String outputFilePath = FileUtils.ChangeFilename(BlockCipherModel.Path,
                                                                     forEncryption ? "_encrypted" : "_decrypted");

                    using (Stream ostream = File.Create(outputFilePath))
                    {
                        if (BlockCipherModel.BouncyImplementation)
                        {
                            EncryptUsingBC(istream, ostream, iv, forEncryption);
                        }
                        else
                        {
                            EncryptUsingDotNet(istream, ostream, iv, forEncryption);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBoxContent = new MessageBoxViewModel(CloseMessageBox, MessageBoxModel.Error(e.Message,"Details",e.StackTrace));
                IsMessageBoxVisible = true;
            }
        }

        private void EncryptUsingBC(Stream istream, Stream ostream, byte[] iv, bool forEncryption)
        {
            var padding = ((TypeWrapper) BlockCipherModel.Padding).Instance<IBlockCipherPadding>();
            var engine = BlockCipherModel.Engine.Instance<IBlockCipher>();
            var mode = ((TypeWrapper) BlockCipherModel.Mode).Instance<IBlockCipher>(engine);
            var cipher = new PaddedBufferedBlockCipher(mode, padding);
            var buf = new byte[16]; //input buffer
            var obuf = new byte[512]; //output buffer

            int noBytesRead; //number of bytes read from input
            int noBytesProcessed ; //number of bytes processed
            var p = new ParametersWithIV(new KeyParameter(PbkdfModel.Key), iv);
            cipher.Init(forEncryption, p);
            // Buffer used to transport the bytes from one stream to another

            while ((noBytesRead = istream.Read(buf, 0, Blocksize)) > 0)
            {
                //System.out.println(noBytesRead +" bytes read");

                noBytesProcessed =
                    cipher.ProcessBytes(buf, 0, noBytesRead, obuf, 0);
                //System.out.println(noBytesProcessed +" bytes processed");
                ostream.Write(obuf, 0, noBytesProcessed);
            }

            //System.out.println(noBytesRead +" bytes read");
            noBytesProcessed = cipher.DoFinal(obuf, 0);

            //System.out.println(noBytesProcessed +" bytes processed");
            ostream.Write(obuf, 0, noBytesProcessed);

            ostream.Flush();
        }

        private void EncryptUsingDotNet(Stream istream, Stream ostream, byte[] iv, bool forEncryption)
        {
            var engine = BlockCipherModel.Engine.Instance<SysSecurity.SymmetricAlgorithm>();
            var mode =
                (SysSecurity.CipherMode) Enum.Parse(typeof (SysSecurity.CipherMode), BlockCipherModel.Mode.ToString());
            var padding =
                (SysSecurity.PaddingMode)
                Enum.Parse(typeof (SysSecurity.PaddingMode), BlockCipherModel.Padding.ToString());
            engine.Mode = mode;
            engine.Padding = padding;
            SysSecurity.ICryptoTransform cryptoTranform = forEncryption
                                                              ? engine.CreateEncryptor(PbkdfModel.Key, iv)
                                                              : engine.CreateDecryptor(PbkdfModel.Key, iv);
            SysSecurity.CryptoStreamMode csMode = forEncryption
                                                      ? SysSecurity.CryptoStreamMode.Write
                                                      : SysSecurity.CryptoStreamMode.Read;
            using (
                var encStream = new SysSecurity.CryptoStream(forEncryption ? ostream : istream, cryptoTranform, csMode))
            {
                if (forEncryption)
                {
                    CopyStream(istream, encStream);
                }
                else
                {
                    CopyStream(encStream, ostream);
                }
            }
        }

        private void OpenFile()
        {
            //FileUtils.OpenFileUI();
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                BlockCipherModel.Path = ofd.FileName;
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8192];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public override string Name { get { return "Encrypt"; }
            protected set { }
        }
        public override string ImageSource { get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/encrypt.png"; } }
        
    }
}