using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoTools.Helpers;

namespace CryptoTools.Models
{
    public class PbkdfModel : ModelBase
    {
        public bool BouncyImplementation { get; set; }
        public String Password { get; set; }
        public int KeySize { get; set; }
        public int Iterations { get; set; }
        public String Salt { get; set; }
        public TypeWrapper DerivationFunction { get; set; }
        public TypeWrapper Digest { get; set; }

        private byte[] _key;
        public byte[] Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
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
