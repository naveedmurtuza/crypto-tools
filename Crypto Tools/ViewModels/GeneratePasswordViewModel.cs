using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Models;
using CryptoTools.Passwords;

namespace CryptoTools.ViewModels
{
    public class GeneratePasswordViewModel : IPageViewModel
    {
        private ICommand _generatePasswordCommand;
        public GeneratePasswordModel Model { get; set; }

        public GeneratePasswordViewModel()
        {
            Model = new GeneratePasswordModel();
        }

        public ICommand GeneratePasswordCommand
        {
            get
            {
                if(_generatePasswordCommand == null)
                {
                    _generatePasswordCommand = new RelayCommand(param=>GeneratePassword(), param=>CanExecute());
                }
                return _generatePasswordCommand;
            }
        }

        private void GeneratePassword()
        {
            PasswordGenerator passwdGen = GetPasswordGenerator();
            Model.Passwords = String.Empty;
            List<String> passwords = passwdGen.Generate(Model.PasswordCount);
            foreach (var password in passwords)
            {
                Model.Passwords += password + Environment.NewLine;
            }
        }

        private bool CanExecute()
        {
            return Model.PasswordLength > 0 && Model.PasswordCount > 0;
        }

        private PasswordGenerator GetPasswordGenerator()
        {
            if (Model.GeneratePronounceablePassword)
            {
                return new PronounceablePasswordGenerator(Model.PasswordLength);
            }
            return new SimplePasswordGenerator
                       {
                           ConsecutiveCharacters = Model.ConsectiveChars,
                           RepeatCharacters = Model.RepeatChars,
                           ExcludeSymbols = !Model.IncludeSymbols,
                           PasswordLength = Model.PasswordLength
                           
                       };
        }

        public string Name { get { return "Password Generator"; } }
        public string ImageSource { get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/password.png"; } }
        public bool CanClose
        {
            get { return true; }
        }
        public bool HasBackStack { get { return false; } }
        public void Back()
        {
            throw new NotImplementedException();
        }
    }
}
