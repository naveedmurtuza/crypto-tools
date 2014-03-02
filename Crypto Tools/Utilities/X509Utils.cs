using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace CryptoTools.Utilities
{
    public class X509Utils
    {
        private static X509Certificate GenerateCertificate(X509Name subject, X509Name issuer,
                                                           int iValidity, AsymmetricKeyParameter publicKey,
                                                           AsymmetricKeyParameter privateKey, String signatureType,
                                                           int keyusages, ExtendedKeyUsage extendedKeyUsages, bool isCA,
                                                           int pathLenConstraint)
        {
            // Get an X509 Version 1 Certificate generator
            var certGen = new X509V3CertificateGenerator();

            // Load the generator with generation parameters

            // Set the issuer distinguished name
            certGen.SetIssuerDN(issuer);

            // Valid before and after dates now to iValidity days in the future
            certGen.SetNotBefore(DateTime.Now);
            certGen.SetNotAfter(DateTime.Now.AddDays(iValidity));

            // Set the subject distinguished name (same as issuer for our purposes)
            certGen.SetSubjectDN(subject);

            // Set the public key
            certGen.SetPublicKey(publicKey);

            // Set the algorithm
            certGen.SetSignatureAlgorithm(signatureType);

            // Set the serial number
            //read the serial number
            String serial = Repository.Instance.ReadSerialNumber();
            var biSerial = new BigInteger(serial, Repository.SerialNumberRadix);
            certGen.SetSerialNumber(biSerial);

            certGen.AddExtension(X509Extensions.KeyUsage, false, new KeyUsage(keyusages));
            if (extendedKeyUsages != null)
                certGen.AddExtension(X509Extensions.ExtendedKeyUsage, false, extendedKeyUsages);
            if (isCA)
                certGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(pathLenConstraint));

            try
            {
                // Generate an X.509 certificate, based on the current issuer and subject
                return certGen.Generate(privateKey);
            }
                // Something went wrong
                //catch (GeneralSecurityException ex)
                //{
                //    throw new CryptoException(ex.Message,ex);
                //}
            finally
            {
                Repository.Instance.IncrementSerial();
            }
        }

        /// <summary>
        /// Generates a CA certificate.
        /// </summary>
        /// <param name="subject">X509Name subject name </param>
        /// <param name="iValidity">validity in days</param>
        /// <param name="publicKey">publickey</param>
        /// <param name="privateKey">private key of the issuer</param>
        /// <param name="signatureType">signature type</param>
        /// <param name="keyusages">keyusages, <see>Org.BouncyCastle.Asn1.X509.KeyUsage</see></param>
        /// <param name="extendedKeyUsages">extendedKeyUsages <see>Org.BouncyCastle.Asn1.X509.KeyPurposeID</see></param>
        /// <param name="pathLenConstraint"> </param>
        /// <returns>brand new generated X509Certificate</returns>
        public static X509Certificate GenerateCACertificate(X509Name subject,
                                                            int iValidity, AsymmetricKeyParameter publicKey,
                                                            AsymmetricKeyParameter privateKey, String signatureType,
                                                            int keyusages, ExtendedKeyUsage extendedKeyUsages,
                                                            int pathLenConstraint)
        {
            return GenerateCertificate(subject, subject, iValidity, publicKey, privateKey, signatureType, keyusages,
                                       extendedKeyUsages, true, pathLenConstraint);
        }

        /// <summary>
        /// Generates a user certificate
        /// </summary>
        /// <param name="subject">X509Name subject name </param>
        /// <param name="issuer">X509Name issuer name</param>
        /// <param name="iValidity">validity in days</param>
        /// <param name="publicKey">publickey</param>
        /// <param name="privateKey">private key of the issuer</param>
        /// <param name="signatureType">signature type</param>
        /// <param name="keyusages">keyusages, <see>Org.BouncyCastle.Asn1.X509.KeyUsage</see></param>
        /// <param name="extendedKeyUsages">extendedKeyUsages <see>Org.BouncyCastle.Asn1.X509.KeyPurposeID</see></param>
        /// <returns>brand new generated X509Certificate</returns>
        public static X509Certificate GenerateUserCertificate(X509Name subject, X509Name issuer,
                                                              int iValidity, AsymmetricKeyParameter publicKey,
                                                              AsymmetricKeyParameter privateKey, String signatureType,
                                                              int keyusages, ExtendedKeyUsage extendedKeyUsages)
        {
            return GenerateCertificate(subject, issuer, iValidity, publicKey, privateKey, signatureType, keyusages,
                                       extendedKeyUsages, false, 0);
        }

        /// <summary>
        /// Generates a X509Name from the given attr
        /// </summary>
        /// <param name="sCommonName"></param>
        /// <param name="sOrganisationUnit"></param>
        /// <param name="sOrganisation"></param>
        /// <param name="sLocality"></param>
        /// <param name="sState"></param>
        /// <param name="sCountryCode"></param>
        /// <param name="sEmailAddress"></param>
        /// <returns></returns>
        public static X509Name GetX509Name(String sCommonName, String sOrganisationUnit,
                                           String sOrganisation, String sLocality, String sState, String sCountryCode,
                                           String sEmailAddress, IDictionary<DerObjectIdentifier, String> other)
        {
            // Holds certificate attributes
            IDictionary attrs = new Dictionary<DerObjectIdentifier, String>();
            var vOrder = new ArrayList();

            // Load certificate attributes
            if (sCommonName != null)
            {
                attrs[X509Name.CN] = sCommonName;
                vOrder.Insert(0, X509Name.CN);
            }

            if (sOrganisationUnit != null)
            {
                attrs[X509Name.OU] = sOrganisationUnit;
                vOrder.Insert(0, X509Name.OU);
            }

            if (sOrganisation != null)
            {
                attrs[X509Name.O] = sOrganisation;
                vOrder.Insert(0, X509Name.O);
            }

            if (sLocality != null)
            {
                attrs[X509Name.L] = sLocality;
                vOrder.Insert(0, X509Name.L);
            }

            if (sState != null)
            {
                attrs[X509Name.ST] = sState;
                vOrder.Insert(0, X509Name.ST);
            }

            if (sCountryCode != null)
            {
                attrs[X509Name.C] = sCountryCode;
                vOrder.Insert(0, X509Name.C);
            }

            if (sEmailAddress != null)
            {
                attrs[X509Name.E] = sEmailAddress;
                vOrder.Insert(0, X509Name.E);
            }
            if (other != null)
            {
                foreach (var item in other)
                {
                    attrs[item.Key] = item.Value;
                    vOrder.Insert(0, item.Key);
                }
            }
            return new X509Name(vOrder, attrs);
        }

        public static X509Name GetX509Name(String sCommonName, String sOrganisationUnit,
                                           String sOrganisation, String sLocality, String sState, String sCountryCode,
                                           String sEmailAddress, string other)
        {
            IDictionary<DerObjectIdentifier, String> attrs = new Dictionary<DerObjectIdentifier, string>();
            if (other != null)
            {
                foreach (
                    string str in other.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] kvpair = str.Split('=');
                    attrs.Add((DerObjectIdentifier) X509Name.DefaultLookup[kvpair[0].ToLower()], kvpair[1]);
                }
            }
            return GetX509Name(sCommonName, sOrganisationUnit, sOrganisation, sLocality, sState, sCountryCode,
                               sEmailAddress, attrs);
        }

        public static void ExportPKCS12(String path, String alias, AsymmetricKeyEntry privKey, char[] password,
                                        params X509CertificateEntry[] certs)
        {
            var keyStore = new Pkcs12Store();
            keyStore.SetKeyEntry(alias, privKey, certs);
            using (var fs = new FileStream(path, FileMode.Create))
            {
                keyStore.Save(fs, password, Repository.Srand);
            }
        }

        public static void ExportPKCS12(String path, String alias, AsymmetricKeyParameter privKey, char[] password,
                                        params X509Certificate[] certs)
        {
            var keyStore = new Pkcs12Store();
            keyStore.SetKeyEntry(alias, new AsymmetricKeyEntry(privKey), WrapX509Certificates(certs));
            using (var fs = new FileStream(path, FileMode.Create))
            {
                keyStore.Save(fs, password, Repository.Srand);
            }
        }

        public static X509CertificateEntry[] WrapX509Certificates(params X509Certificate[] certs)
        {
            var entries = new X509CertificateEntry[certs.Length];
            for (int i = 0; i < certs.Length; i++)
            {
                entries[i] = new X509CertificateEntry(certs[i]);
            }
            return entries;
        }

        public static Pkcs12Store LoadCAPfx(char[] password)
        {
            var keyStore = new Pkcs12Store();
            //PasswordWindow window = new PasswordWindow();
            //if (window.ShowDialog() == true)
            {
                using (var fs = new FileStream(Repository.Instance.CAKeyStore, FileMode.Open))
                {
                    keyStore.Load(fs, password);
                }
            }
            return keyStore;
        }

        internal static Pkcs12Store LoadCertificatesKeyStore(char[] password)
        {
            var keyStore = new Pkcs12Store();
            using (var fs = new FileStream(Repository.Instance.CertificatesKeyStore, FileMode.Open))
            {
                keyStore.Load(fs, password);
            }
            return keyStore;
        }
    }
}