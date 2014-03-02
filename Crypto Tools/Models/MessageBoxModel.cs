using System.Windows;
using CryptoTools.Utilities;

namespace CryptoTools.Models
{
    public class MessageBoxModel : Observable
    {
        public static MessageBoxModel Information(string text,string title)
        {
            return new MessageBoxModel
                       {
                           MessageBoxButton = MessageBoxButton.OK,
                           MessageBoxImage = MessageBoxImage.Information,
                           Text = text,
                           Title = title
                       };
        }
        public static MessageBoxModel Error(string text)
        {
            return new MessageBoxModel
            {
                MessageBoxButton = MessageBoxButton.OK,
                MessageBoxImage = MessageBoxImage.Error,
                Text = text
            };
        }
        public static MessageBoxModel Error(string text,string expanderHeader,string expanderDetails)
        {
            return new MessageBoxModel
            {
                MessageBoxButton = MessageBoxButton.OK,
                MessageBoxImage = MessageBoxImage.Error,
                Text = text,
                ExpanderDetails = expanderDetails,
                ExpanderHeader = expanderHeader
            };
        }
        private string _dontAskAgainText;
        private bool _dontShowAgainVisible;
        private string _expanderDetails;
        private string _expanderHeader;
        private MessageBoxButton _messageBoxButton;
        private MessageBoxImage _messageBoxImage;
        private string _text;

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }


        public MessageBoxButton MessageBoxButton
        {
            get { return _messageBoxButton; }
            set
            {
                if (_messageBoxButton != value)
                {
                    _messageBoxButton = value;
                    OnPropertyChanged("MessageBoxButton");
                }
            }
        }

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

    }
}