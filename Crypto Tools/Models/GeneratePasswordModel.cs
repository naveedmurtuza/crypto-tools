using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoTools.Models
{
    public class GeneratePasswordModel : ModelBase
    {
        
        private int _passwordLength;
        public int PasswordLength
        {
            get { return _passwordLength; }
            set { _passwordLength = value;
            OnPropertyChanged("PasswordLength");
            }
        }

        private int _passwordCount;
        public int PasswordCount
        {
            get { return _passwordCount; }
            set
            {
                _passwordCount = value;
                OnPropertyChanged("PasswordCount");
            }
        }

        private bool _generatePronounceablePassword;
        public bool GeneratePronounceablePassword
        {
            get { return _generatePronounceablePassword; }
            set
            {
                _generatePronounceablePassword = value;
                OnPropertyChanged("GeneratePronounceablePassword");
            }
        }

        private bool _includeSymbols;
        public bool IncludeSymbols
        {
            get { return _includeSymbols; }
            set
            {
                _includeSymbols = value;
                OnPropertyChanged("IncludeSymbols");
            }
        }

        private bool _repeatChars;
        public bool RepeatChars
        {
            get { return _repeatChars; }
            set
            {
                _repeatChars = value;
                OnPropertyChanged("RepeatChars");
            }
        }

        private bool _consecutiveChars;
        public bool ConsectiveChars
        {
            get { return _consecutiveChars; }
            set
            {
                _consecutiveChars = value;
                OnPropertyChanged("ConsectiveChars");
            }
        }

        private String _passwords;
        public String Passwords
        {
            get { return _passwords; }
            set
            {
                _passwords = value;
                OnPropertyChanged("Passwords");
            }
        }


        public override string Error
        {
            get { throw new NotImplementedException(); }
        }

        public override string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }
    }
}

