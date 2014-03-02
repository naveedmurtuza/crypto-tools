using System;
using System.Windows;
using CryptoTools.Models;

namespace CryptoTools.UI.MessageBox
{
    public class FxMessageBox
    {
        //
        // Summary:
        //     Displays a message box in front of the specified window. The message box
        //     displays a message and returns a result.
        //
        // Parameters:
        //   owner:
        //     A System.Windows.Window that represents the owner window of the message box.
        //
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.
        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            return Show(null, messageBoxText, "Message Box");
        }

        //
        // Summary:
        //     Displays a message box in front of the specified window. The message box
        //     displays a message and title bar caption; and it returns a result.
        //
        // Parameters:
        //   owner:
        //     A System.Windows.Window that represents the owner window of the message box.
        //
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        //   caption:
        //     A System.String that specifies the title bar caption to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
        {
            return Show(owner, messageBoxText, "Message Box", MessageBoxButton.OK);
        }
        //
        // Summary:
        //     Displays a message box in front of the specified window. The message box
        //     displays a message, title bar caption, button, and icon; and it also returns
        //     a result.
        //
        // Parameters:
        //   owner:
        //     A System.Windows.Window that represents the owner window of the message box.
        //
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        //   caption:
        //     A System.String that specifies the title bar caption to display.
        //
        //   button:
        //     A System.Windows.MessageBoxButton value that specifies which button or buttons
        //     to display.
        //
        //   icon:
        //     A System.Windows.MessageBoxImage value that specifies the icon to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(owner, messageBoxText, caption, null, null, false, null, button, icon);
        }
        //
        // Summary:
        //     Displays a message box in front of the specified window. The message box
        //     displays a message, title bar caption, and button; and it also returns a
        //     result.
        //
        // Parameters:
        //   owner:
        //     A System.Windows.Window that represents the owner window of the message box.
        //
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        //   caption:
        //     A System.String that specifies the title bar caption to display.
        //
        //   button:
        //     A System.Windows.MessageBoxButton value that specifies which button or buttons
        //     to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.

        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
        {
            return Show(owner, messageBoxText, caption, button, MessageBoxImage.None);
        }
       

        // Summary:
        //     Displays a message box that has a message and that returns a result.
        //
        // Parameters:
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.
        
        public static MessageBoxResult Show(string messageBoxText)
        {
            return Show(messageBoxText,"Message Box");
        }
        //
        // Summary:
        //     Displays a message box that has a message and title bar caption; and that
        //     returns a result.
        //
        // Parameters:
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        //   caption:
        //     A System.String that specifies the title bar caption to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.
        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            return Show(messageBoxText, caption,MessageBoxButton.OK);
        }
        
        //
        // Summary:
        //     Displays a message box that has a message, title bar caption, and button;
        //     and that returns a result.
        //
        // Parameters:
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        //   caption:
        //     A System.String that specifies the title bar caption to display.
        //
        //   button:
        //     A System.Windows.MessageBoxButton value that specifies which button or buttons
        //     to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            return Show(messageBoxText, caption,button,MessageBoxImage.None);
        }
        
        //
        // Summary:
        //     Displays a message box that has a message, title bar caption, button, and
        //     icon; and that returns a result.
        //
        // Parameters:
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        //   caption:
        //     A System.String that specifies the title bar caption to display.
        //
        //   button:
        //     A System.Windows.MessageBoxButton value that specifies which button or buttons
        //     to display.
        //
        //   icon:
        //     A System.Windows.MessageBoxImage value that specifies the icon to display.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.
        
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(null, messageBoxText, caption, button, icon);
        }
        
        //
        // Summary:
        //     Displays a message box that has a message, title bar caption, button, and
        //     icon; and that accepts a default message box result and returns a result.
        //
        // Parameters:
        //   messageBoxText:
        //     A System.String that specifies the text to display.
        //
        //   caption:
        //     A System.String that specifies the title bar caption to display.
        //
        //   button:
        //     A System.Windows.MessageBoxButton value that specifies which button or buttons
        //     to display.
        //
        //   icon:
        //     A System.Windows.MessageBoxImage value that specifies the icon to display.
        //
        //   defaultResult:
        //     A System.Windows.MessageBoxResult value that specifies the default result
        //     of the message box.
        //
        // Returns:
        //     A System.Windows.MessageBoxResult value that specifies which message box
        //     button is clicked by the user.
        
        
        public static MessageBoxResult Show(Window owner,string text, string caption,String detailsHeader,String details,bool dontAskAgainVisible,string dontAskAgainText, MessageBoxButton button, MessageBoxImage icon)
        {
            var mbcModel = new MessageBoxControlModel
                               {
                                   DontAskAgainVisible = dontAskAgainVisible,
                                   MessageBoxImage = icon,
                                   Text = text,
                                   ExpanderHeader = detailsHeader,
                                   ExpanderDetails = details,
                                   DontAskAgainText = dontAskAgainText
                               };
            var mbcontrol = new MessageBoxControl(mbcModel);
            var m = new Model {MessageBoxButton = button, Title = caption, Content = mbcontrol};

            var fx = new FxMessageWindow(m);
            fx.Owner = owner;
            fx.ShowInTaskbar = false;
            fx.ShowDialog();
            return fx.Result;
        }
    }
    
}
