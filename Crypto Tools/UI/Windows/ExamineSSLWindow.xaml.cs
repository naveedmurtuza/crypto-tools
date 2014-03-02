using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using CryptoTools.Utilities;

namespace CryptoTools.UI.Windows
{
    /// <summary>
    /// Interaction logic for ExamineSSLWindow.xaml
    /// </summary>
    public partial class ExamineSSLWindow : Window
    {
        private const int port = 443;

        public ExamineSSLWindow()
        {
            InitializeComponent();
        }

        private void GoBtnClicked(object sender, RoutedEventArgs re)
        {
            String url = txtUrl.Text;
            if (!url.IsNullOrEmpty())
                ExamineSslConnection(url, port);
        }

        private void ExamineSslConnection(String server, int port)
        {
            TcpClient client = null;
            try
            {
                // Create a TCP/IP client socket. 
                // machineName is the host running the server application.
                client = new TcpClient(server, port);
                txtTerminal.AppendText("Done" + Environment.NewLine);
                var sslStream = new SslStream(
                    client.GetStream(),
                    false,
                    ValidateServerCertificate,
                    null // SelectLocalCertificate
                    );
                // The server name must match the name on the server certificate.
                sslStream.AuthenticateAsClient(server);
                txtTerminal.AppendText("Cipher Algorithm: " + sslStream.CipherAlgorithm);
                txtTerminal.AppendText(Environment.NewLine);
                txtTerminal.AppendText("Cipher Strength: " + sslStream.CipherStrength);
                txtTerminal.AppendText(Environment.NewLine);
                txtTerminal.AppendText("Hash Algorithm: " + sslStream.HashAlgorithm);
                txtTerminal.AppendText(Environment.NewLine);
                txtTerminal.AppendText("Hash Strength: " + sslStream.HashStrength);
                txtTerminal.AppendText(Environment.NewLine);
                txtTerminal.AppendText("Key Exchange Algorithm: " + sslStream.KeyExchangeAlgorithm);
                txtTerminal.AppendText(Environment.NewLine);
                txtTerminal.AppendText("Key Exchange Strength: " + sslStream.KeyExchangeStrength);
                txtTerminal.AppendText(Environment.NewLine);
                txtTerminal.AppendText("Ssl Protocol: " + sslStream.SslProtocol);
            }
            catch (AuthenticationException e)
            {
                txtTerminal.AppendText(String.Format("Exception: {0}", e.Message));
                if (e.InnerException != null)
                {
                    txtTerminal.AppendText(String.Format("Inner exception: {0}", e.InnerException.Message));
                }
                txtTerminal.AppendText(String.Format("Authentication failed - closing the connection."));
                if(client != null)
                client.Close();
            }
            catch
            {
                
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
            txtTerminal.AppendText(">>>>>>>> SSL ERROR <<<<<<<<<");
            txtTerminal.AppendText(Environment.NewLine);
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

            MessageBoxResult result = System.Windows.MessageBox.Show(msg.ToString(), "Warning", MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);

            return result == MessageBoxResult.Yes;
        }
    }
}