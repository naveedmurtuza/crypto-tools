using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemX509 = System.Security.Cryptography.X509Certificates;

namespace CryptoTools.Models
{
    public class KeyStoreModel : ModelBase
    {
        private string _keyStorePath;
        private bool _isSystemKeyStore;
        private SystemX509.StoreName _systemStoreName;
        private SystemX509.StoreLocation _systemStorelocation;

        public char[] Password { get; set; }

        public SystemX509.StoreName SystemStoreName
        {
            get { return _systemStoreName; }
            set
            {
                _systemStoreName = value;
                OnPropertyChanged("SystemStoreName");
            }
        }

        public SystemX509.StoreLocation SystemStoreLocation
        {
            get { return _systemStorelocation; }
            set
            {
                _systemStorelocation = value;
                OnPropertyChanged("SystemStoreLocation");
            }
        }


        public String KeyStorePath
        {
            get { return _keyStorePath; }
            set
            {
                _keyStorePath = value;
                OnPropertyChanged("KeyStorePath");
            }
        }

        public bool IsSystemKeyStore
        {
            get { return _isSystemKeyStore; }
            set
            {
                _isSystemKeyStore = value;
                OnPropertyChanged("IsSystemKeyStore");
            }
        }

        
        public override string Error
        {
            get { throw new NotImplementedException(); }
        }

        public override string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }
    }
}
