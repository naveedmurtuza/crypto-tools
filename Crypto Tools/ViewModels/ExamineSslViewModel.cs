using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CryptoTools.Command;
using CryptoTools.UI.MessageBox;
using CryptoTools.Utilities;

namespace CryptoTools.ViewModels
{
    public class ExamineSslViewModel : Observable, IPageViewModel
    {
        public ExamineSslViewModel()
        {
            int debug = 9;
        }
        private const int port = 443;
        public String Url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged("Url");
            }
        }

        public String Response
        {
            get { return _response; }
            set
            {
                _response = value;
                OnPropertyChanged("Response");
            }
        }

        public ICommand ExamineSslConnectionCommand
        {
            get { return _cmd ?? (_cmd = new RelayCommand(p => ExamineSslConnection(), p => CanExecute())); }
        }

        private void ExamineSslConnection()
        {
            TcpClient client = null;
            try
            {
                Response = "Connecting to " + Url + " ...  ";
                // Create a TCP/IP client socket. 
                // machineName is the host running the server application.
                client = new TcpClient(Url, port);
                Response += "Done" + Environment.NewLine;
                var sslStream = new SslStream(
                    client.GetStream(),
                    false,
                    ValidateServerCertificate,
                    null // SelectLocalCertificate
                    );
                // The server name must match the name on the server certificate.
                sslStream.AuthenticateAsClient(Url);
                Response += "Cipher Algorithm: " + sslStream.CipherAlgorithm;
                Response += Environment.NewLine;
                Response += "Cipher Strength: " + sslStream.CipherStrength;
                Response += Environment.NewLine;
                Response += "Hash Algorithm: " + sslStream.HashAlgorithm;
                Response += Environment.NewLine;
                Response += "Hash Strength: " + sslStream.HashStrength;
                Response += Environment.NewLine;
                Response += "Key Exchange Algorithm: " + sslStream.KeyExchangeAlgorithm;
                Response += Environment.NewLine;
                Response += "Key Exchange Strength: " + sslStream.KeyExchangeStrength;
                Response += Environment.NewLine;
                Response += "Ssl Protocol: " + sslStream.SslProtocol;
            }
            catch (AuthenticationException e)
            {
                Response += String.Format("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Response += String.Format("Inner exception: {0}", e.InnerException.Message);
                }
                Response += String.Format("Authentication failed - closing the connection.");
                if (client != null)
                    client.Close();
            }
            catch(Exception ex)
            {
                Response += String.Format("Exception: {0}", ex.Message);
            }
        }
        public bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            Response += "----- BEGIN SSL POLICY ERRORS -----";
            Response += Environment.NewLine;
            var msg = new StringBuilder();
            msg.AppendLine("Ssl Policy Errors!");
            if (FlagsHelper.IsSet(sslPolicyErrors, SslPolicyErrors.RemoteCertificateChainErrors))
            {
                msg.AppendLine("Remote certificate chain cannot be verified.");
            }
            if (FlagsHelper.IsSet(sslPolicyErrors, SslPolicyErrors.RemoteCertificateNameMismatch))
            {
                msg.AppendLine("Remote certificate name mismatch.");
            }
            if (FlagsHelper.IsSet(sslPolicyErrors, SslPolicyErrors.RemoteCertificateNotAvailable))
            {
                msg.AppendLine("Remote certificate not available.");
            }
            Response += msg.ToString();
            Response += "----- END SSL POLICY ERRORS -----" + Environment.NewLine;
            //just showing off my messagebox,
            MessageBoxResult result = FxMessageBox.Show(msg.ToString(), "Warning", MessageBoxButton.OK,
                                                      MessageBoxImage.Warning);
            //Since we are just examining the connection, return true
            //returning false will abort the connection.
            return true;
        }
        private bool CanExecute()
        {
            return true; //TODO:
        }

       


        public string Name { get { return "Ssl"; } }
        public string ImageSource { get { return "pack://application:,,,/Crypto Tools;component/Images/tiles/ssl.png"; } }
        public bool CanClose
        {
            get { return true; }
        }
        public bool HasBackStack { get { return false; } }
        public void Back()
        {
            throw new NotImplementedException();
        }

        private String _response;
        private String _url;
        private ICommand _cmd;
    }
}
