using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace CryptoTools.Utilities
{
    public class PemUtilities
    {
        private static IEnumerable<String> _a;
        public static IEnumerable<String> Algorithims
        {
            get
            {
                return _a ?? (_a = new[]
                                       {
                                           "AES-128-CBC", "AES-128-CFB", "AES-128-ECB", "AES-128-OFB",
                                           "AES-192-CBC", "AES-192-CFB", "AES-192-ECB", "AES-192-OFB",
                                           "AES-256-CBC", "AES-256-CFB", "AES-256-ECB", "AES-256-OFB",
                                           "BF-CBC", "BF-CFB", "BF-ECB", "BF-OFB",
                                           "DES-CBC", "DES-CFB", "DES-ECB", "DES-OFB",
                                           "DES-EDE", "DES-EDE-CBC", "DES-EDE-CFB", "DES-EDE-ECB", "DES-EDE-OFB",
                                           "DES-EDE3", "DES-EDE3-CBC", "DES-EDE3-CFB", "DES-EDE3-ECB", "DES-EDE3-OFB",
                                           "RC2-CBC", "RC2-CFB", "RC2-ECB", "RC2-OFB",
                                           "RC2-40-CBC",
                                           "RC2-64-CBC",
                                       });
            }
        }
        /// <summary>
        /// Encodes the given object to String.
        /// </summary>
        /// <param name="obj">Object to encode</param>
        /// <returns>PEM String</returns>
        public static String Encode(Object obj)
        {
            return Encode(obj, null, null, null);
        }

        /// <summary>
        /// Encodes the given object to String.
        /// </summary>
        /// <param name="obj">Object to encode</param>
        /// <param name="algorithm">algorithm to encrypt the data(Generally used for writing PrivateKeys)</param>
        /// <param name="password">password to protect the data with(Generally used for writing PrivateKeys)</param>
        /// <param name="rand"></param>
        /// <returns>PEM String</returns>
        public static String Encode(Object obj, String algorithm, char[] password, SecureRandom rand)
        {
            String data;
            StringWriter sw = new StringWriter();
            Encode(sw, obj, algorithm, password, rand);
            data = sw.ToString();
            sw.Close();
            return data;
        }

        /// <summary>
        /// Writes PEM encoded data to the given Writer
        /// </summary>
        /// <param name="wrt">Stream to write the data in</param>
        /// <param name="obj">data to write in Stream</param>
        public static void Encode(TextWriter wrt, Object obj)
        {
            Encode(wrt, obj, null, null, null);
        }

        /// <summary>
        /// Writes PEM encoded data to the given Writer
        /// </summary>
        /// <param name="wrt">Stream to write the data in</param>
        /// <param name="obj">data to write in Stream</param>
        /// <param name="algorithm">algorithm to encrypt the data(Generally used for writing PrivateKeys)</param>
        /// <param name="password">password to protect the data with(Generally used for writing PrivateKeys)</param>
        /// <param name="rand">source of randomness</param>
        public static void Encode(TextWriter wrt, Object obj, String algorithm, char[] password, SecureRandom rand)
        {
            PemWriter pemWriter = new PemWriter(wrt);
            if ((password != null))
            {
                pemWriter.WriteObject(obj, algorithm, password, rand);
            }
            else
            {
                pemWriter.WriteObject(obj);
            }
        }

        /// <summary>
        /// Reads the PEM encoded string data, converts it to the
        /// appropriate object.X509Certificate object for PEM encoded Certificate,
        /// PKCS10CertificationRequest for PEM encoded Certificate Request ...
        /// </summary>
        /// <param name="pem">PEM encoded string data</param>
        /// <returns>PEM Object</returns>
        public static T Decode<T>(String pem)
        {
            return Decode<T>(pem, null);
        }

        /// <summary>
        /// Reads the PEM encoded string data, converts it to the
        /// appropriate object.X509Certificate object for PEM encoded Certificate,
        /// PKCS10CertificationRequest for PEM encoded Certificate Request ...
        /// </summary>
        /// <param name="pem">PEM encoded string data</param>
        /// <returns>PEM Object</returns>
        public static T Decode<T>(String pem, char[] password)
        {
            PemReader pReader;
            StringReader sr = new StringReader(pem);
            pReader = password == null ? new PemReader(sr) : new PemReader(sr, new Password(password));
            return (T)pReader.ReadObject();
        }

        private class Password
                    : IPasswordFinder
        {
            private readonly char[] password;

            public Password(
                char[] word)
            {
                this.password = (char[])word.Clone();
            }

            public char[] GetPassword()
            {
                return (char[])password.Clone();
            }
        }
    }

}
