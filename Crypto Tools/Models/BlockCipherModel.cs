using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoTools.Helpers;

namespace CryptoTools.Models
{
    public class BlockCipherModel : ModelBase
    {
        private byte[] _key;
        private String _path;
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
        public Object Padding { get; set; }
        public Object Mode { get; set; }
        public TypeWrapper Engine { get; set; }
        public int KeySize { get; set; }

        public byte[] Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }
        public String Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged("Path");
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
