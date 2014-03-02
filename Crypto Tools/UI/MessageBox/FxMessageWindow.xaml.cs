using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CryptoTools.Command;

namespace CryptoTools.UI.MessageBox
{
    /// <summary>
    /// Interaction logic for FxMessageWindow.xaml
    /// </summary>
    public partial class FxMessageWindow
    {
        public FxMessageWindow(Model model)
        {
            InitializeComponent();
            DataContext = new ViewModel(this,model);
        }

        //internal static MessageBoxResult Show(string p)
        //{
        //    //var fxMessage = new FxMessageWindow();
        //    //fxMessage.ShowDialog();
        //    //MessageBoxResult result = ((ViewModel) fxMessage.DataContext).Result;
        //    //MessageBox.Show("" + result);
        //    //return ((ViewModel) fxMessage.DataContext).Result;
        //}

        public MessageBoxResult Result
        {
            get
            {
                return ((ViewModel)DataContext).Result;
            }
        }
    }



    public class ViewModel : INotifyPropertyChanged
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

        private readonly Window _view;
        private ICommand _button1Command, _button2Command, _button3Command;

        private InnerMessageBoxResult _innerMessageBoxResult;
        private Model _model;

        public ViewModel(Window view,Model model)
        {
            _view = view;
            _model = model;
            //Model = new Model();
            //Model.MessageBoxButton = MessageBoxButton.YesNoCancel;
            //Model.Title = "Title";
        }

        public Model Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    NotifyPropertyChange("Model");
                }
            }
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
                                                                                      _view.Close();
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
                                                               _view.Close();
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
                                                               _view.Close();
                                                           });
                return _button3Command;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void NotifyPropertyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

    public class Model : INotifyPropertyChanged
    {
        private ContentControl _content;
        private MessageBoxButton _messageBoxButton;
        private string _title;
        
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChange("Title");
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
                    NotifyPropertyChange("MessageBoxButton");
                }
            }
        }
        public ContentControl Content
        {
            get { return _content; }
            set
            {
                if (!Equals(_content, value))
                {
                    _content = value;
                    NotifyPropertyChange("Content");
                }
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void NotifyPropertyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}