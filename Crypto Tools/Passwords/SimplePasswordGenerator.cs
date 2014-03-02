using System.Text;

namespace CryptoTools.Passwords
{
    public class SimplePasswordGenerator : PasswordGenerator
    {
        private const int UBoundDigit = 61;

        private readonly char[] _pwdCharArray =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".
                ToCharArray();


        public SimplePasswordGenerator()
        {
            ConsecutiveCharacters = false;
            RepeatCharacters = true;
            ExcludeSymbols = false;
            Exclusions = null;
        }

        public string Exclusions { get; set; }

        public bool ExcludeSymbols { get; set; }

        public bool RepeatCharacters { get; set; }

        public bool ConsecutiveCharacters { get; set; }


        protected char GetRandomCharacter()
        {
            int upperBound = _pwdCharArray.GetUpperBound(0);

            if (ExcludeSymbols)
            {
                upperBound = UBoundDigit;
            }

            int randomCharPosition = GetCryptographicRandomNumber(
                _pwdCharArray.GetLowerBound(0), upperBound);

            char randomChar = _pwdCharArray[randomCharPosition];

            return randomChar;
        }

        public override string Generate()
        {
            var pwdBuffer = new StringBuilder();

            // Generate random characters

            // Initial dummy character flag
            char lastCharacter = '\n';

            for (int i = 0; i < PasswordLength; i++)
            {
                char nextCharacter = GetRandomCharacter();

                if (false == ConsecutiveCharacters)
                {
                    while (lastCharacter == nextCharacter)
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                if (false == RepeatCharacters)
                {
                    string temp = pwdBuffer.ToString();
                    int duplicateIndex = temp.IndexOf(nextCharacter);
                    while (-1 != duplicateIndex)
                    {
                        nextCharacter = GetRandomCharacter();
                        duplicateIndex = temp.IndexOf(nextCharacter);
                    }
                }

                if ((null != Exclusions))
                {
                    while (-1 != Exclusions.IndexOf(nextCharacter))
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }

            return pwdBuffer.ToString();
        }
    }
}