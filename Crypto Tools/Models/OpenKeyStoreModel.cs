﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemX509 = System.Security.Cryptography.X509Certificates;
namespace CryptoTools.Models
{
    public class OpenKeyStoreModel : ModelBase
    {
        
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
        public bool IsSystemKeyStore
        {
            get { return _isSystemKeyStore; }
            set
            {
                _isSystemKeyStore = value;
                OnPropertyChanged("IsSystemKeyStore");
            }
        }

        private bool _isSystemKeyStore;
        public String KeyStorePath
        {
            get { return _keyStorePath; }
            set
            {
                _keyStorePath = value;
                OnPropertyChanged("KeyStorePath");
            }
        }

        private String _keyStorePath;
        private SystemX509.StoreLocation _systemStorelocation;
        private SystemX509.StoreName _systemStoreName;

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
