using System;
using System.Windows;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.Models;

namespace CryptoTools.ViewModels
{
    public class MessageBoxViewModel
    {
        #region InnerMessageBoxResult enum

        public enum InnerMessageBoxResult
        {
            None,
            Button1,
            Button2,
            Button3
        }

        #endregion
        private ICommand _button1Command, _button2Command, _button3Command;
        private InnerMessageBoxResult _innerMessageBoxResult;
        private readonly Action<object> _closeAction;
        public MessageBoxModel Model { get; private set; }
        public MessageBoxViewModel(Action<object> closeAction,MessageBoxModel model)
        {
            Model = model;
            this._closeAction = closeAction;
        }

        public MessageBoxResult Result
        {
            get
            {
                switch (_innerMessageBoxResult)
                {
                    case InnerMessageBoxResult.Button3:
                        return MessageBoxResult.Cancel;
                    case InnerMessageBoxResult.Button2:
                        return Model.MessageBoxButton == MessageBoxButton.OKCancel
                                   ? MessageBoxResult.Cancel
                                   : MessageBoxResult.No;
                    case InnerMessageBoxResult.Button1:
                        if (Model.MessageBoxButton == MessageBoxButton.OK ||
                            Model.MessageBoxButton == MessageBoxButton.OKCancel)
                            return MessageBoxResult.OK;
                        return MessageBoxResult.Yes;
                    default:
                        return MessageBoxResult.None;
                }
            }
        }

        public ICommand Button1Command
        {
            get
            {
                return _button1Command ?? (_button1Command = new RelayCommand(delegate
                {
                    _innerMessageBoxResult =
                        InnerMessageBoxResult.Button1;
                    _closeAction.Invoke(null);
                }));
            }
        }

        public ICommand Button2Command
        {
            get
            {
                if (_button2Command == null)
                    _button2Command = new RelayCommand(delegate
                    {
                        _innerMessageBoxResult = InnerMessageBoxResult.Button2;
                        _closeAction.Invoke(null);
                    });
                return _button2Command;
            }
        }

        public ICommand Button3Command
        {
            get
            {
                if (_button3Command == null)
                    _button3Command = new RelayCommand(delegate
                    {
                        _innerMessageBoxResult = InnerMessageBoxResult.Button3;
                        _closeAction.Invoke(null);
                    });
                return _button3Command;
            }
        }
    }
}