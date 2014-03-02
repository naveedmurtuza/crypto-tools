using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoTools.Helpers
{
    /// <summary>
    /// Simple class to wrap around, basically <code>Org.BouncyCastle.Asn1.X509.KeyUsage</code> and <code>Org.BouncyCastle.Asn1.X509.KeyPurposeId</code>,
    ///  so that .ToString() returns a meaningful suggested Name
    /// </summary>
    public class KeyUsageWrapper<T>
    {
        public T Value { private set;get;}
        public String Name { private set; get; }
        public bool IsSelected { get; set; }
        public KeyUsageWrapper(T value, String name)
        {
            this.Value = value;
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
