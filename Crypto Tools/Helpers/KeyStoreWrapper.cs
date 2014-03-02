using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Pkcs;
using CryptoTools.UI.Windows;
using System.IO;
using CryptoTools.Utilities;
using System.Collections.ObjectModel;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto;

namespace CryptoTools.Helpers
{
    public class KeyStoreWrapper
    {
        private readonly ObservableCollection<ListItemEntry> _listItems = new ObservableCollection<ListItemEntry>();
        
        /// <summary>
        /// Keystore entry passwords
        /// </summary>
        private readonly IDictionary<String, char[]> m_mPasswords = new Dictionary<String, char[]>();

        


        ///// <summary>
        ///// Construct a new KeyStoreWrapper for the supplied keystore.
        ///// </summary>
        ///// <param name="keyStore">The keystore</param>
        //public KeyStoreWrapper(Pkcs12Store keyStore)
        //{
        //    KeyStore = keyStore;
        //}


        /// <summary>
        /// Construct a new KeyStoreWrapper for the supplied keystore, keystore file and keystore password.
        /// </summary>
        /// <param name="keyStore">The keystore</param>
        /// <param name="fKeyStore">The keystore file</param>
        /// <param name="cPassword">The keystore password</param>
        public KeyStoreWrapper(String fKeyStore, char[] cPassword)
        {
            if (File.Exists(fKeyStore))
                KeyStore = X509Utils.LoadCertificatesKeyStore(cPassword);
            else
                KeyStore = new Pkcs12Store();
            KeyStoreFile = fKeyStore;
            Password = cPassword;
        }

        


        /// <summary>
        /// Set the password for a particular keystore entry in the wrapper.
        /// </summary>
        /// <param name="sAlias">The keystore entry's alias</param>
        /// <param name="cPassword">The keystore entry's password</param>
        public void setEntryPassword(String sAlias, char[] cPassword)
        {
            m_mPasswords[sAlias] = cPassword;
        }

        
        /// <summary>
        /// Remove a particular keystore entry from the wrapper.
        /// </summary>
        /// <param name="sAlias">The keystore entry's alias</param>
        public void removeEntryPassword(String sAlias)
        {
            m_mPasswords.Remove(sAlias);
        }

        
        /// <summary>
        /// Get the password for a particular keystore entry.
        /// </summary>
        /// <param name="sAlias">The keystore entry's alias</param>
        /// <returns>The keystore entry's password or null if none is set</returns>
        public char[] getEntryPassword(String sAlias)
        {
            return m_mPasswords[sAlias];
        }

        
        /// <summary>
        /// Gets/Sets the keystore's physical file.
        /// </summary>
        public String KeyStoreFile {get;set;}
        
        

        

        /// <summary>
        /// Gets/Sets the keystore.
        /// </summary>
        /// <returns>The keystore</returns>
        public Pkcs12Store KeyStore
        {
            get; private set;
        }


        

        /// <summary>
        /// Gets/Sets the keystore password
        /// </summary>
        /// <returns>The keystore password</returns>
        public char[] Password{get;private set;}
        

        
        /// <summary>
        /// Register with the wrapper whether the keystore has been changed since its last save.
        /// </summary>
        /// <param name="bChanged">Has the keystore been changed?</param>
        public bool Changed{get;private set;}

        public ObservableCollection<ListItemEntry> Items
        {
            get
            {
                return _listItems;
            }
        }
        public void RefreshItems()
        {
            _listItems.Clear();
            foreach (String alias in KeyStore.Aliases)
            {
                KeyStoreEntryType entryType;
                if (KeyStore.IsCertificateEntry(alias))
                {
                    entryType = KeyStoreEntryType.TrustCertEntry;
                }
                else if (KeyStore.IsKeyEntry(alias) && KeyStore.GetCertificateChain(alias) != null && KeyStore.GetCertificateChain(alias).Length != 0)
                {
                    entryType = KeyStoreEntryType.KeyPairEntry;
                }
                else
                    entryType = KeyStoreEntryType.KeyEntry;

                _listItems.Add(new ListItemEntry(entryType, alias, KeyStore.GetCertificate(alias).Certificate));
            }
        }
        //public bool Load()
        //{
        //    if (_store == null)
        //    {
        //        _store = new Pkcs12Store();
        //    }
        //    now get the password
        //    PasswordWindow window = new PasswordWindow();
        //    if (window.ShowDialog() == true)
        //    {
        //        using (FileStream fs = new FileStream(AppResources.CertificatesKeyStore, FileMode.Open))
        //        {
        //            _store.Load(fs, window.Password);
        //        }
        //        foreach (var item in _store.)
        //        {
                    
        //        }
        //        return true;
        //    }
        //    return false;
        //}

        //public bool Save()
        //{

        //}


        public void AddEntry(String alias,X509Certificate cert,AsymmetricCipherKeyPair keypair)
        {
            KeyStore.SetKeyEntry(alias, new AsymmetricKeyEntry(keypair.Private), X509Utils.WrapX509Certificates(cert));
            _listItems.Add(new ListItemEntry(KeyStoreEntryType.KeyPairEntry, alias, cert));
            //signal that it has been changed
            Changed = true;
        }

        
    }
}
