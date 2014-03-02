using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoTools.Helpers;

namespace CryptoTools.Models
{
    public class DigestModel : ModelBase
    {
        private String _filepath;
        public String FilePath
        {
            get { return _filepath; }
            set
            {
                _filepath = value;
                OnPropertyChanged("FilePath");
            }
        }
        public String Text { get; set; }
        private byte[] _hash;
        public byte[] Hash
        {
            get { return _hash; }
            set
            {
                _hash = value;
                OnPropertyChanged("Hash");
            }
        }
        private bool _bouncyImpl;
        public bool BouncyImplementation
        {
            get { return _bouncyImpl; }
            set
            {
                _bouncyImpl = value;
                OnPropertyChanged("BouncyImplementation");
            }
        }
        public TypeWrapper HashAlgorithm { get; set; }
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
