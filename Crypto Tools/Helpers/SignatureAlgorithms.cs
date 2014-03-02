using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTools.Helpers
{
    /// <summary>
    /// Helper class to get all Signature Algorithms for a given KeyPairType
    /// </summary>
    public class SignatureAlgorithms
    {
        private static readonly IDictionary<KeyPairType, IEnumerable<String>> KeypairMap;
        static SignatureAlgorithms()
        {
            
            KeypairMap = new Dictionary<KeyPairType, IEnumerable<String>>
                             {
                                 {
                                     KeyPairType.Rsa, new List<String>(
                                     new String[]
                                         {
                                             "MD5withRSA", "SHA1withRSA", "SHA224withRSA", "SHA256withRSA", "SHA512withRSA"
                                             , "RIPEMD128withRSA", "RIPEMD160withRSA", "RIPEMD256withRSA"
                                         })
                                 },
                                 {
                                     KeyPairType.Dsa, new List<String>(
                                     new String[]
                                         {
                                             "SHA1withDSA", "SHA224withDSA", "SHA256withDSA", "SHA384withDSA",
                                             "SHA512withDSA"
                                         })
                                 },
                                 {
                                     KeyPairType.Ec, new List<String>(
                                     new String[]
                                         {
                                             "SHA1withECDSA", "SHA224withECDSA", "SHA256withECDSA", "SHA384withECDSA",
                                             "SHA512withECDSA"
                                         })
                                 },
                                 {
                                     KeyPairType.Dh, new List<String>(
                                     new String[] {})
                                 },
                                 {
                                     KeyPairType.Elgamal, new List<String>(
                                     new String[] {})
                                 },
                                 {
                                     KeyPairType.Gost3410, new List<String>(
                                     new String[]
                                         {
                                             "GOST3411WITHGOST3410-2001", "GOST3411WITHECGOST3410-2001",
                                             "GOST3411WITHECGOST3410", "GOST3411WITHGOST3410-94", "GOST3411WITHGOST3410"
                                         })
                                 }
                             };
        }

        /// <summary>
        /// Gets Signature Algorithms for give KeyPairType
        /// </summary>
        /// <param name="keyPairType"></param>
        /// <returns></returns>
        public static IEnumerable<String> ValuesFor(KeyPairType keyPairType)
        {
            IEnumerable<String> values = KeypairMap[keyPairType];
            return values ?? Enumerable.Empty<String>();
        }
    }
}
