using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CryptoTools.Models;
using Org.BouncyCastle.X509;

namespace CryptoTools.UI.Windows
{
    /// <summary>
    /// Interaction logic for RevokeWindow.xaml
    /// </summary>
    public partial class RevokeWindow : Window
    {
        public RevokeWindow(X509Certificate cert)
        {
            InitializeComponent();
            Certificate = cert;
            RevokeModel.Serial = cert.SerialNumber.ToString();
        }

        

        public X509Certificate Certificate { get; private set; }
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            
        }

        private void btnRevoke_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
//X509CertificateUI.ViewCertificate(Certificate);
        }


    }
}
