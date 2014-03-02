using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Helpers;
using CryptoTools.Models;
using CryptoTools.Utilities;

namespace CryptoTools.ViewModels
{
    public class ExportKeyViewModel : ViewModelBase
    {
        private readonly Action<ListItemEntry, string, char[], string> _action;
        private readonly Action<object> _cancelAction;
        private readonly ListItemEntry _entry;
        private readonly Action<ListItemEntry, string, char[]> _exportPkcs12Action;
        private ICommand _browseCommand;
        private ICommand _cancelCmd;
        private ICommand _command;
        private String _path, _selectedAlgorithm;

        public ExportKeyViewModel(ListItemEntry entry, Action<ListItemEntry, String, char[], String> action,
                                  Action<object> cancelAction)
        {
            _entry = entry;
            _action = action;
            _cancelAction = cancelAction;
            AlgorithmRequired = true;
        }

        public ExportKeyViewModel(ListItemEntry entry, Action<ListItemEntry, String, char[]> exportPkcs12Action,
                                  Action<object> cancelAction)
        {
            _entry = entry;
            _exportPkcs12Action = exportPkcs12Action;
            _cancelAction = cancelAction;
            AlgorithmRequired = false;
        }

        public String Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }

        public bool AlgorithmRequired { get; set; }
        public String SelectedAlgorithm
        {
            get { return _selectedAlgorithm; }
            set
            {
                _selectedAlgorithm = value;
                OnPropertyChanged("SelectedAlgorithm");
            }
        }

        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(param => BrowseFile())); }
        }

        public IEnumerable<String> AlgorithmSource
        {
            get { return PemUtilities.Algorithims; }
        }

        public ICommand Command
        {
            get
            {
                if (_command == null)
                {
                    _command = new RelayCommand(CommandExecuted);
                }
                return _command;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCmd == null)
                {
                    _cancelCmd = new RelayCommand(_cancelAction);
                }
                return _cancelCmd;
            }
        }

        private void BrowseFile()
        {
            String filename;
            FileUtils.SaveFileUI("Select file to export", out filename);
            if (filename != null)
            {
                Path = filename;
            }
        }

        private void CommandExecuted(object o)
        {
            var pboxes = (object[])o;
            var sb = new StringBuilder();
            char[] password1 = ((PasswordBox) pboxes[0]).Password.ToArray();
            char[] password2 = ((PasswordBox)pboxes[1]).Password.ToArray();
            if (password1.Length == 0)
            {
                sb.Append("please enter a password").Append(Environment.NewLine);
            }
            if (!password1.SequenceEqual(password2))
            {
                sb.Append("password and confirm do not match").Append(Environment.NewLine);
            }
            if (Path.IsNullOrEmpty())
            {
                sb.Append("please select a path ").Append(Environment.NewLine);
            }
            if (AlgorithmRequired && SelectedAlgorithm.IsNullOrEmpty())
            {
                sb.Append("please select an algorithm ").Append(Environment.NewLine);
            }
            Array.Clear(password1, 0, password1.Length);
            Array.Clear(password2, 0, password2.Length);
            var errors = sb.ToString();
            if(!errors.IsNullOrEmpty())
            {
                MessageBoxContent = new MessageBoxViewModel(CloseMessageBox,
                                                            MessageBoxModel.Error("Error exporting to file - " +
                                                                                  Environment.NewLine + errors));
                IsMessageBoxVisible = true;
                return;
            }
            char[] password = ((PasswordBox) pboxes[0]).Password.ToArray();
            if (AlgorithmRequired) 
            {
                _action.Invoke(_entry, SelectedAlgorithm, password, Path);
            }
            else
            {
                _exportPkcs12Action.Invoke(_entry, Path, password);
            }
        }

        public override string Name { get; protected set; }

        public override string ImageSource
        {
            get { throw new NotImplementedException(); }
        }
    }
}
