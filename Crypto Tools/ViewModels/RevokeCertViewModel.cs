using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Helpers;
using CryptoTools.Utilities;
using Org.BouncyCastle.X509;

namespace CryptoTools.ViewModels
{
    public class RevokeCertViewModel
    {
        private readonly RevokedSerial _revokedSerial;
        private readonly Action<ListItemEntry,String, int, DateTime> _revokeAction;
        private readonly Action<object> _closeAction;
        private ICommand _command;
        private ICommand _cancelCmd;
        private readonly ListItemEntry _entry;

        public RevokeCertViewModel(ListItemEntry entry,Action<ListItemEntry,String, int,DateTime> revokeAction,
                                  Action<object> closeAction)
        {
            _revokedSerial = new RevokedSerial();
            Certificate = entry.Certificate;
            RevokedSerial.Serial = Certificate.SerialNumber.ToString();
            this._revokeAction = revokeAction;
            this._closeAction = closeAction;
            this._entry = entry;
        }

        public RevokedSerial RevokedSerial { get {return _revokedSerial; } }
        public IEnumerable<String> CrlReasons
        {
            get { return Enum.GetNames(typeof(CrlReasonEx)); }
        }


        public X509Certificate Certificate { get;private set; }

        public ICommand Command
        {
            get
            {
                if (_command == null)
                {
                    _command = new RelayCommand(param =>
                                                    {
                                                        _revokeAction.Invoke(_entry, RevokedSerial.Serial,
                                                                             RevokedSerial.Reason, DateTime.UtcNow);
                                                        _closeAction.Invoke(null);

                                                    });
                }
                return _command;
            }
        }
        public ICommand HyperLinkCommand
        {
            get
            {
                if (_command == null)
                {
                    _command = new RelayCommand(param => { });
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
                    _cancelCmd = new RelayCommand(_closeAction);
                }
                return _cancelCmd;
            }
        }
    }
}
