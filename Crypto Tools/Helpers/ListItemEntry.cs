using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CryptoTools.Utilities;
using Org.BouncyCastle.X509;

namespace CryptoTools.Helpers
{
    public class ListItemEntry :
    Observable
    {
       

        public String EntryTypeUri { private set; get; }
        public String Alias { private set; get; }
        public X509Certificate Certificate { private set; get; }

        public const string RevokedPropertyName = "Revoked";
        private bool _revoked;
        public bool Revoked
        {
            get { return _revoked; }
            set
            {
                if (_revoked != value)
                {
                    _revoked = value;
                    OnPropertyChanged(RevokedPropertyName);
                }
            }
        }
        
        public bool Valid { private set; get; }

        public ListItemEntry(KeyStoreEntryType entryType, String alias, X509Certificate cert, bool revoked = false)
        {
            switch (entryType)
            {
                case KeyStoreEntryType.KeyEntry:
                    EntryTypeUri = "/Crypto%20Tools;component/Images/key_entry.gif";
                    break;
                case KeyStoreEntryType.KeyPairEntry:
                    EntryTypeUri = "/Crypto%20Tools;component/Images/keypair_entry.gif";
                    break;
                case KeyStoreEntryType.TrustCertEntry:
                    EntryTypeUri = "/Crypto%20Tools;component/Images/trustcert_entry.gif";
                    break;
            }
            
            Alias = alias;
            Certificate = cert;
            Valid = cert.IsValidNow;
            Revoked = revoked;

        }
    }
}
