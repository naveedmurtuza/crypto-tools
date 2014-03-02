using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CryptoTools.Passwords
{
    public abstract class PasswordGenerator
    {
        protected readonly RNGCryptoServiceProvider _rng;

        protected PasswordGenerator() : this(6)
        {
        }

        protected PasswordGenerator(int passwordLength)
        {
            _rng = new RNGCryptoServiceProvider();
            PasswordLength = passwordLength;
        }

        public int PasswordLength { get; set; }

        protected double NextDouble()
        {
            var buffer = new byte[4];
            _rng.GetBytes(buffer);
            UInt32 rand = BitConverter.ToUInt32(buffer, 0);
            return rand/(1.0 + UInt32.MaxValue);
        }

        protected int GetCryptographicRandomNumber(int lBound, int uBound)
        {
            // Assumes lBound >= 0 && lBound < uBound
            // returns an int >= lBound and < uBound
            uint urndnum;
            var rndnum = new Byte[4];
            if (lBound == uBound - 1)
            {
                // test for degenerate case where only lBound can be returned
                return lBound;
            }

            uint xcludeRndBase = (uint.MaxValue -
                                  (uint.MaxValue%(uint) (uBound - lBound)));

            do
            {
                _rng.GetBytes(rndnum);
                urndnum = BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);

            return (int) (urndnum%(uBound - lBound)) + lBound;
        }

        /// <summary>
        /// Generates a set of pronounceable passwords.
        /// </summary>
        /// <param name="passwordCount">The number of passwords to generate.</param>
        /// <returns>An ArrayList of passwords as strings.</returns>
        public List<String> Generate(int passwordCount)
        {
            var result = new List<string>();


            // Pick a random starting point.
            for (int pwnum = 0; pwnum < passwordCount; pwnum++)
            {
                result.Add(Generate());
            } // for pwnum

            return result;
        }

        public abstract string Generate();
    }

    /*internal class CryptoRandom : Random 
        { private RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider(); 
            private byte[] _uint32Buffer = new byte[4]; 
            public CryptoRandom() { } 
            public CryptoRandom(Int32 ignoredSeed) { } 
            public override Int32 Next() { _rng.GetBytes(_uint32Buffer); return BitConverter.ToInt32(_uint32Buffer, 0) & 0x7FFFFFFF; }
            public override Int32 Next(Int32 maxValue) { if (maxValue < 0) throw new ArgumentOutOfRangeException("maxValue"); return Next(0, maxValue); } 
            public override Int32 Next(Int32 minValue, Int32 maxValue) { if (minValue > maxValue) throw new ArgumentOutOfRangeException("minValue"); if (minValue == maxValue) return minValue; Int64 diff = maxValue - minValue; while (true) { _rng.GetBytes(_uint32Buffer); UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0); Int64 max = (1 + (Int64)UInt32.MaxValue); Int64 remainder = max % diff; if (rand < max - remainder) { return (Int32)(minValue + (rand % diff)); } } } 
            public override double NextDouble()
            {
                _rng.GetBytes(_uint32Buffer); 
                UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0); return rand / (1.0 + UInt32.MaxValue);
            } 
            public override void NextBytes(byte[] buffer) { if (buffer == null) throw new ArgumentNullException("buffer"); _rng.GetBytes(buffer); } }*/
}