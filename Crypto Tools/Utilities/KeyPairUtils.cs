using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto;
using CryptoTools.Helpers;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;

namespace CryptoTools.Utilities
{
    public class KeyPairUtils
    {
        private const int UnknownKeySize = -1;

        /// <summary>
        /// Generate a key pair.
        /// </summary>
        /// <param name="keygen">an instance of IAsymmetricCipherKeyPairGenerator</param>
        /// <param name="keySize">iKeySize Key size of key pair</param>
        /// <param name="random">an instance of IRandomGenerator</param>
        /// <returns>A key pair</returns>
        public static AsymmetricCipherKeyPair GenerateKeyPair(IAsymmetricCipherKeyPairGenerator keygen, int keySize, IRandomGenerator random)
        {
            // Create a SecureRandom
            var rand = new SecureRandom(random);

            // Initialize key pair generator with key strength and a randomness
            keygen.Init(new KeyGenerationParameters(rand, keySize));

            // Generate and return the key pair
            return keygen.GenerateKeyPair();
        }



        /// <summary>
        /// Generate a key pair.
        /// </summary>
        /// <param name="keyPairType">keyPairType Key pair type to generate</param>
        /// <param name="iKeySize">iKeySize Key size of key pair</param>
        /// <returns>A key pair</returns>
        public static AsymmetricCipherKeyPair GenerateKeyPair(KeyPairType keyPairType, int iKeySize)
        {
            IAsymmetricCipherKeyPairGenerator keyPairGen = null;
            try
            {
                switch (keyPairType)
                {
                    case KeyPairType.Ec:
                        keyPairGen = new ECKeyPairGenerator();
                        break;
                    case KeyPairType.Dsa:
                        keyPairGen = new DsaKeyPairGenerator();
                        break;
                    //case KeyPairType.Rsa:
                    default:
                        keyPairGen = new RsaKeyPairGenerator();
                        break;
                }


                // Initialize key pair generator with key strength and a randomness
                keyPairGen.Init(new KeyGenerationParameters(Repository.Srand, iKeySize));

                // Generate and return the key pair
            }
            catch (InvalidParameterException ex)
            {

            }
            return keyPairGen.GenerateKeyPair();
        }

        /// <summary>
        /// Get the key size of a public key.
        /// </summary>
        /// <param name="pubKey">The public key</param>
        /// <returns>The key size, <see cref="UnknownKeySize">if not known</see></returns>
        public static int GetKeyLength(AsymmetricKeyParameter pubKey)
        {
            if (pubKey is RsaKeyParameters)
            {
                return ((RsaKeyParameters)pubKey).Modulus.BitLength;
            }
            if (pubKey is DsaKeyParameters)
            {
                return ((DsaKeyParameters)pubKey).Parameters.P.BitLength;
            }
            if (pubKey is DHKeyParameters)
            {
                return ((DHKeyParameters)pubKey).Parameters.P.BitLength;
            }
            if (pubKey is ECKeyParameters)
            {
                // TODO: how to get key size from these?
                return UnknownKeySize;
            }
            return UnknownKeySize;
        }
        public static KeyPairType QueryKeyType(AsymmetricKeyParameter pubKey)
        {
            if (pubKey is RsaKeyParameters)
            {
                return KeyPairType.Rsa;
            }
            else if (pubKey is DsaKeyParameters)
            {
                return KeyPairType.Dsa;
            }
            else if (pubKey is DHKeyParameters)
            {
                return KeyPairType.Dh;
            }
            else if (pubKey is ECKeyParameters)
            {
                // TODO: how to get key size from these?
                return KeyPairType.Ec;
            }
            // TODO: shud never come here
            return KeyPairType.Elgamal;
        }

        
        public static IAsymmetricCipherKeyPairGenerator CreateGenerator(SecureRandom random, TypeWrapper kpGen,
                                                                        int keystrength)
        {
            var keypairGen = kpGen.Instance<IAsymmetricCipherKeyPairGenerator>();
            //var random = SecureRandomUtils.GetInstance(Model.RandomGenerator, Model.RandomGeneratorArgument);
            if (keypairGen is DsaKeyPairGenerator)
            {
                DsaParametersGenerator pGen = new DsaParametersGenerator();
                pGen.Init(keystrength, 80, random); //TODO:
                DsaParameters parameters = pGen.GenerateParameters();
                DsaKeyGenerationParameters genParam = new DsaKeyGenerationParameters(random, parameters);
                keypairGen.Init(genParam);
                return keypairGen;
            }
            if (keypairGen is ECKeyPairGenerator)
            {
                keypairGen.Init(new KeyGenerationParameters(random, keystrength));
                return keypairGen;
            }
            keypairGen.Init(new KeyGenerationParameters(random, keystrength));
            return keypairGen;
        }
    }
}
