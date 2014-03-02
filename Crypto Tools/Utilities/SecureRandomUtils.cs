using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Crypto;
using CryptoTools.Helpers;

namespace CryptoTools.Utilities
{
    public class SecureRandomUtils
    {
        /// <summary>
        /// Gets an instance of SecureRandom given the types.
        /// </summary>
        /// <param name="type1">this parameter provides the type of <see cref="IRandomGenerator"/></param>
        /// <param name="type2">Required only IFF type1 is ReversedWindowGenerator or DigestRandomGenerator. type2 shud be typeof IRandomGenerator if type1 is ReversedWindowGenerator or DigestRandomGenerator otherwsie</param>
        /// <returns></returns>
        public static SecureRandom GetInstance(Type type1, Type type2)
        {            
            IRandomGenerator rng = null;
            if (type1 == typeof(ReversedWindowGenerator))
            {
                //get the supporting stuff
                //we require another IRandomGenerator
                //here IRandomGenerator cannot be  ReversedWindowGenerator or DigestRandomGenerator
                //just to avoid complexity
                
                var supporterRng = (IRandomGenerator)Activator.CreateInstance(type2);
                rng = (IRandomGenerator)Activator.CreateInstance(type1, supporterRng);
            }
            else if (type1 == typeof(DigestRandomGenerator))
            {
                //get the supporting stuff
                //we require IDigest
                IDigest digest = (IDigest)Activator.CreateInstance(type2);
                rng = (IRandomGenerator)Activator.CreateInstance(type1, digest);
            }
            else
            {
                rng = (IRandomGenerator)Activator.CreateInstance(type1);
            }
            return new SecureRandom(rng);
        }

        public static SecureRandom GetInstance(TypeWrapper type1, TypeWrapper type2)
        {
            return GetInstance(type1.Type, type2 == null ? null : type2.Type);
        }
    }
}
