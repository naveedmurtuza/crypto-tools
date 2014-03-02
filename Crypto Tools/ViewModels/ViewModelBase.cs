using System;
using CryptoTools.Utilities;

namespace CryptoTools.ViewModels
{
    public abstract class ViewModelBase : Observable, IPageViewModel, IMessageBoxSupport
    {
        private Action<object> _close;
        private bool _isPopupVisible;
        private object _popupContent;

        public Action<object> CloseMessageBox
        {
            get
            {
                return _close ?? (_close = o =>
                                               {
                                                   IsMessageBoxVisible = false;
                                                   MessageBoxContent = null;
                                               });
            }
        }

        #region IMessageBoxSupport Members

        public bool IsMessageBoxVisible
        {
            get { return _isPopupVisible; }
            set
            {
                if (value != _isPopupVisible)
                {
                    _isPopupVisible = value;
                    OnPropertyChanged("IsMessageBoxVisible");
                }
            }
        }

        public object MessageBoxContent
        {
            get { return _popupContent; }
            set
            {
                if (value != _popupContent)
                {
                    _popupContent = value;
                    OnPropertyChanged("MessageBoxContent");
                }
            }
        }

        #endregion

        #region IPageViewModel Members

        public abstract string Name { get; protected set; }
        public abstract string ImageSource { get; }

        public virtual bool CanClose
        {
            get { return true; }
        }

        public virtual bool HasBackStack
        {
            get { return false; }
        }

        public virtual void Back()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}