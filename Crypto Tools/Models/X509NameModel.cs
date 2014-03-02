using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoTools.Helpers;
using CryptoTools.Utilities;
using Org.BouncyCastle.Asn1.X509;

namespace CryptoTools.Models
{
    public class X509NameModel : ModelBase
    {
        private String _additionalOIDs;
        private String _selectedOIDValue;
        /// <summary>
        /// Common Name(CN)
        /// </summary>
        public String CommonName { get; set; }

        /// <summary>
        /// Organisation(O)
        /// </summary>
        public String Organisation { get; set; }

        /// <summary>
        /// Organisation Unit(OU)
        /// </summary>
        public String OrganisationUnit { get; set; }

        /// <summary>
        /// Email(E)
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Two letter ISO Country Code
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String SelectedOIDName { get; set; }

        /// <summary>
        /// AdditionalOIDs specified sepearted by newline chars
        /// fires PropertyChanged
        /// </summary>
        public String AdditionalOIDs
        {
            get { return _additionalOIDs; }
            set
            {
                _additionalOIDs = value;
                OnPropertyChanged("AdditionalOIDs");
            }
        }

        /// <summary>
        /// 
        ///fires PropertyChanged
        /// </summary>
        public String SelectedOIDValue
        {
            get { return _selectedOIDValue; }
            set
            {
                _selectedOIDValue = value;
                OnPropertyChanged("SelectedOIDValue");
            }
        }

        /// <summary>
        /// Helper method to get <see cref="X509Name"/>
        /// </summary>
        public X509Name X509Name
        {
            get
            {
                return X509Utils.GetX509Name(CommonName, OrganisationUnit, Organisation, null, null,
                                             Country == null ? null : Country.ISO2Code, Email, AdditionalOIDs);
            }
        }

        public override string Error
        {
            get { throw new NotImplementedException(); }
        }

        public override string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case "CommonName":
                        if (string.IsNullOrEmpty(CommonName))
                            result = "Please enter a CommonName";
                        break;
                    case "Country":
                        if (Country == null)
                            result = "Please select a country";
                        break;
                    case "Email":
                        result = String.IsNullOrEmpty(Email)
                                     ? null
                                     : Email.IsValidEmail() ? null : "Please enter valid email";
                        break;
                }
                return result;
            }
        }
    }
}
