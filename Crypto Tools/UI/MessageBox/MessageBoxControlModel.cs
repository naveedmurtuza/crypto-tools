using System.Windows;
using CryptoTools.Utilities;

namespace CryptoTools.UI.MessageBox
{
    public class MessageBoxControlModel : Observable
    {
        private MessageBoxImage _messageBoxImage;
        private string _text;
        private bool _dontShowAgainVisible;
        private string _expanderHeader;
        private string _expanderDetails;
        private string _dontAskAgainText;


        public bool DontAskAgainVisible
        {
            get { return _dontShowAgainVisible; }
            set
            {
                if (_dontShowAgainVisible != value)
                {
                    _dontShowAgainVisible = value;
                    OnPropertyChanged("DontShowAgainVisible");
                }
            }
        }

        public string DontAskAgainText
        {
            get { return _dontAskAgainText; }
            set
            {
                if (_dontAskAgainText != value)
                {
                    _dontAskAgainText = value;
                    OnPropertyChanged("DontAskAgainText");
                }
            }
        }
        public MessageBoxImage MessageBoxImage
        {
            get { return _messageBoxImage; }
            set
            {
                if (_messageBoxImage != value)
                {
                    _messageBoxImage = value;
                    OnPropertyChanged("MessageBoxImage");
                }
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        public string ExpanderHeader
        {
            get { return _expanderHeader; }
            set
            {
                if (_expanderHeader != value)
                {
                    _expanderHeader = value;
                    OnPropertyChanged("ExpanderHeader");
                }
            }
        }

        public string ExpanderDetails
        {
            get { return _expanderDetails; }
            set
            {
                if (_expanderDetails != value)
                {
                    _expanderDetails = value;
                    OnPropertyChanged("ExpanderDetails");
                }
            }
        }

        //public string Title
        //{
        //    get { return _title; }
        //    set
        //    {
        //        if (_title != value)
        //        {
        //            _title = value;
        //            NotifyPropertyChange("Title");
        //        }
        //    }
        //}
        
    }
}
