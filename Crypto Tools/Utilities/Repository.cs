using System;
using System.IO;
using CryptoTools.Helpers;
using CryptoTools.Models;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;

namespace CryptoTools.Utilities
{
    public sealed class Repository
    {
        public const string CaAlias = "ca";
        public const string CaDirName = "ca";
        public const string CaPfxFilename = "ca.pfx";
        public const string CaCertFilename = "ca.crt";
        public const string CertsDirName = "certs";
        public const string CrlsDirName = "crl";
        public const string CrlFilename = "crl.crl";
        public const string CertsPfxFilename = "certs.pfx";
        public const string SerialFilename = "serial";
        public const string RevokeFilename = "revoke_serials";
        public const int SerialNumberRadix = 10;
        public const int CrlFrequency = 24*30; // 1 month
        public static readonly IRandomGenerator DefaultRandomGenerator = new DigestRandomGenerator(new Sha512Digest());
        public static readonly SecureRandom Srand = new SecureRandom(DefaultRandomGenerator);
        //private static IList<String> _revokedSerials;
        private RevokedSerials _revoked;
        /// <summary>
        /// Password for the Loaded KeyStore
        /// </summary>
        public char[] KeyStorePassword { get; private set; }

        /// <summary>
        /// KeyPair of the loaded CA Certificate in the KeyStore
        /// </summary>
        public KeyPairType KeyPairType { get; set; }

        public String CertificateAuthorityRootDir { get; private set; }

        /// <summary>
        /// Absolute path to ca keystore 
        /// </summary>
        public String CAKeyStore
        {
            get { return Path.Combine(CertificateAuthorityRootDir, CaDirName, CaPfxFilename); }
        }

        /// <summary>
        /// Absolute path to Crl
        /// </summary>
        public String CrlFilePath
        {
            get { return Path.Combine(CertificateAuthorityRootDir, CrlsDirName, CrlFilename); }
        }

        /// <summary>
        /// Absolute path to certifiactes keystore 
        /// </summary>
        public String CertificatesKeyStore
        {
            get { return Path.Combine(CertificateAuthorityRootDir, CertsDirName, CertsPfxFilename); }
        }

        /// <summary>
        /// Absolute path to serials file
        /// </summary>
        public String SerialFile
        {
            get { return Path.Combine(CertificateAuthorityRootDir, CertsDirName, SerialFilename); }
        }

        /// <summary>
        /// Absolute path to serials file
        /// </summary>
        public String CrlSerialFile
        {
            get { return Path.Combine(CertificateAuthorityRootDir, CrlsDirName, SerialFilename); }
        }

        /// <summary>
        /// Absolute path to Revoked Serials file
        /// </summary>
        public String RevokedSerialsFile
        {
            get { return Path.Combine(CertificateAuthorityRootDir, CertsDirName, RevokeFilename); }
        }

        /// <summary>
        /// Ca Certificate 
        /// </summary>
        public X509Certificate CaCertificate { get; set; }

        #region Singleton

        public static Repository Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit

            internal static readonly Repository instance = new Repository();

            static Nested()
            {
            }
        }

        #endregion

        /// <summary>
        /// Creates a default directory structure in the given path.
        /// <pre>
        ///      /--<code>path</code>
        ///          |---/ca
        ///          |      |--ca.pfx
        ///          |---/certs
        ///                 |---certs.pfx
        ///                 |---serial
        ///                 |---revoke_serials
        /// </pre>
        /// </summary>
        /// <param name="path">parent directory to create a ca</param>
        /// <param name="password">Password to the KeyStore</param>
        public void NewCertificateAuthority(String path, char[] password)
        {
            KeyStorePassword = password;
            CertificateAuthorityRootDir = path;
            CreateDirectoryStructure();
        }

        /// <summary>
        /// Opens existing Certificate Repository.
        /// </summary>
        /// <param name="path">directory containg the repository</param>
        /// <param name="password">password to the ca certificate</param>
        /// <returns></returns>
        public bool OpenExistingCertificateAuthority(String path, char[] password)
        {
            if (ValidateDirectoryStructure(path))
            {
                KeyStorePassword = password;
                CertificateAuthorityRootDir = path;
                UpdateKeyPairType(password);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates the given Directory Structure
        /// </summary>
        /// <param name="path">absolute path to ca directory</param>
        /// <returns>true if valid,false otherwise</returns>
        public bool ValidateDirectoryStructure(String path)
        {
            //check if all the necessary folders are available
            bool valid =
                Directory.Exists(Path.Combine(path, CaDirName)) |
                File.Exists(Path.Combine(path, CaDirName, CaPfxFilename)); //check if ca keystore is present 
            if (valid)
            {
                //checks certs dir
                if (!Directory.Exists(Path.Combine(path, CertsDirName)))
                    Directory.CreateDirectory(Path.Combine(path, CertsDirName));
            }
            return valid;
        }

        /// <summary>
        /// Reads the last used serial number
        /// </summary>
        /// <returns></returns>
        public String ReadSerialNumber()
        {
            return File.ReadAllText(SerialFile);
        }

        /// <summary>
        /// Reads the last user CrlSerial NUmber
        /// </summary>
        /// <returns></returns>
        public String ReadCrlSerialNumber()
        {
            return File.ReadAllText(CrlSerialFile);
        }

        /// <summary>
        /// Increments the serial number in the serial file
        /// </summary>
        public void IncrementSerial()
        {
            String txt = File.ReadAllText(SerialFile);
            var bi = new BigInteger(txt, SerialNumberRadix);
            bi = bi.Add(BigInteger.One);
            File.WriteAllText(SerialFile, bi.ToString(SerialNumberRadix));
        }
        /// <summary>
        /// Increments the serial number in the Crlserial file
        /// </summary>
        public void IncrementCrlSerial()
        {
            String txt = File.ReadAllText(CrlSerialFile);
            var bi = new BigInteger(txt, SerialNumberRadix);
            bi = bi.Add(BigInteger.One);
            File.WriteAllText(CrlSerialFile, bi.ToString(SerialNumberRadix));
        }

        /// <summary>
        /// Adds the specified serial to revoked certs file
        /// </summary>
        /// <param name="serial">serial number of the certiifcate to revoke</param>
        /// <param name="reason"> reason to revoke</param>
        /// <param name="dateTime">date time of revoke</param>
        public void AddRevokedSerial(String serial, int reason, DateTime dateTime)
        {
            if (_revoked == null)
            {
                if (File.Exists(RevokedSerialsFile))
                    _revoked = RevokedSerials.Deserialize(RevokedSerialsFile);
                else
                {
                    _revoked = new RevokedSerials();
                }
            }
            _revoked[serial] = new RevokedSerial {Reason = reason, RevocationDate = dateTime, Serial = serial};
            RevokedSerials.Serialize(_revoked, RevokedSerialsFile);
            //File.AppendAllText(RevokedSerialsFile, serial + Environment.NewLine);
            //RefreshRevokedSerialsList(serial);
        }

        /// <summary>
        /// Publishes the crl 
        /// </summary>
        public void PublishCrl()
        {
            if(_revoked == null)
            {
                return;
                //TODO: may be show a messagebox or something?
            }
            Pkcs12Store store = LoadCAPfx(KeyStorePassword);
            if (!store.ContainsAlias(CaAlias) || !store.IsEntryOfType(CaAlias, typeof(AsymmetricKeyEntry))) return;
            AsymmetricKeyParameter key = store.GetKey(CaAlias).Key;
            X509Certificate caCert = store.GetCertificate(CaAlias).Certificate;


            var crlNumber = new BigInteger(ReadCrlSerialNumber(), SerialNumberRadix);
            var crlGen = new X509V2CrlGenerator();
            crlGen.SetIssuerDN(caCert.SubjectDN);
            //crlGen.SetNextUpdate();    
            crlGen.SetSignatureAlgorithm(caCert.SigAlgName.Replace("-", ""));
            crlGen.SetThisUpdate(DateTime.UtcNow);
            crlGen.SetNextUpdate(DateTime.UtcNow.AddHours(CrlFrequency));
            crlGen.AddExtension(X509Extensions.CrlNumber, false, new CrlNumber(crlNumber));
            crlGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, false,
                                new AuthorityKeyIdentifierStructure(caCert));
            //crlGen.AddExtension(X509Extensions.KeyUsage, false, new KeyUsage(KeyUsage.KeyAgreement | KeyUsage.CrlSign | KeyUsage.DataEncipherment | KeyUsage.DecipherOnly | KeyUsage.EncipherOnly | KeyUsage.KeyEncipherment | KeyUsage.NonRepudiation));
            foreach (RevokedSerial rs in _revoked.RevokedSerialCollection)
            {
                crlGen.AddCrlEntry(new BigInteger(rs.Serial), rs.RevocationDate, rs.Reason);
            }
            X509Crl crl = crlGen.Generate(key);
            string crlEncoded = PemUtilities.Encode(crl);
            File.WriteAllText(CrlFilePath, crlEncoded);
            IncrementCrlSerial();
        }

        public void RemoveRevokedSerial(String serial)
        {
            if (File.Exists(RevokedSerialsFile))
                _revoked = RevokedSerials.Deserialize(RevokedSerialsFile);
            else
            {
                _revoked = new RevokedSerials();
            }
            _revoked[serial] = null;
            RevokedSerials.Serialize(_revoked, RevokedSerialsFile);
        }


        public Pkcs12Store LoadCAPfx(char[] password)
        {
            var keyStore = new Pkcs12Store();
            using (var fs = new FileStream(CAKeyStore, FileMode.Open))
            {
                keyStore.Load(fs, password);
            }
            return keyStore;
        }

        /// <summary>
        /// Checks wether the given serial is in revoked serials list
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public bool IsRevokedCertificate(String serial)
        {
            if (_revoked == null)
            {
                _revoked = RevokedSerials.Deserialize(RevokedSerialsFile);
                //_revokedSerials = new List<String>(Regex.Split(File.ReadAllText(RevokedSerialsFile), Environment.NewLine));
            }
            return _revoked[serial] != null; //_revokedSerials.Contains(serial);
        }

        /// <summary>
        /// Creates a default directory structure in <see cref="CertificateAuthorityRootDir"/>
        /// </summary>
        private void CreateDirectoryStructure()
        {
            //create ca dir
            Directory.CreateDirectory(Path.Combine(CertificateAuthorityRootDir, CaDirName));

            String certsDir = Path.Combine(CertificateAuthorityRootDir, CertsDirName);
            //create a certs dir
            Directory.CreateDirectory(certsDir);
            //create crl dir
            String crlDir = Path.Combine(CertificateAuthorityRootDir, CrlsDirName);
            Directory.CreateDirectory(crlDir);
            File.WriteAllText(Path.Combine(crlDir, SerialFilename), "1");
            //create serial/revoke files
            //the serial file contains the serial number for the
            //last issued certificate.. like openssl
            //the revoke file contains the serial numbers for all the
            //revoked certificates
            File.WriteAllText(Path.Combine(certsDir, SerialFilename), "1");
            //File.WriteAllText(Path.Combine(certsDir, RevokeFilename), "");
        }

        private void UpdateKeyPairType(char[] password)
        {
            Pkcs12Store store = LoadCAPfx(password);
            if (store.ContainsAlias(CaAlias) && store.IsEntryOfType(CaAlias, typeof(AsymmetricKeyEntry)))
            {
                AsymmetricKeyEntry keyEntry = store.GetKey(CaAlias);
                CaCertificate = store.GetCertificate(CaAlias).Certificate;
                KeyPairType = KeyPairUtils.QueryKeyType(keyEntry.Key);
            }
        }
    }
}