using System;
using CryptoTools.Helpers;
using CryptoTools.Utilities;
using Org.BouncyCastle.Asn1.X509;

namespace CryptoTools.Models
{
    public class X509CertModel : ModelBase
    {
        public const string SignatureAlgorithmPropertyName = "SignatureAlgorithm";
        public const string KeyPairGeneratorPropertyName = "KeyPairGenerator";
        private String _caRepoPath;
        private bool _isCa;
        private TypeWrapper _keyPairGenerator;
        private KeyPairType _keyPairType;
        private String _signatureAlgorithm;
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

        /// <summary>
        /// 
        /// </summary>
        public String SignatureAlgorithm
        {
            get { return _signatureAlgorithm; }
            set
            {
                if (_signatureAlgorithm != value)
                {
                    _signatureAlgorithm = value;
                    OnPropertyChanged(SignatureAlgorithmPropertyName);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public TypeWrapper KeyPairGenerator
        {
            get { return _keyPairGenerator; }
            set
            {
                if (_keyPairGenerator != value)
                {
                    _keyPairGenerator = value;
                    OnPropertyChanged(KeyPairGeneratorPropertyName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int KeyStrength { get; set; }

        /// <summary>
        /// Selected KeyUsages seperated by commas(,)
        /// </summary>
        public String KeyUsages { get; set; }

        /// <summary>
        /// Extended KeyUsages seperated by commas(,)
        /// </summary>
        public String ExtendedKeyUsages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Validity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int PathLenContraint { get; set; }

        /// <summary>
        /// fires PropertyChanged
        /// </summary>
        public bool IsCertificateAuthority
        {
            get { return _isCa; }
            set
            {
                _isCa = value;
                OnPropertyChanged("IsCertificateAuthority");
            }
        }

        /// <summary>
        /// fires PropertyChanged
        /// </summary>
        public KeyPairType KeyPairType
        {
            get { return _keyPairType; }
            set
            {
                _keyPairType = value;
                OnPropertyChanged("KeyPairType");
            }
        }

        /// <summary>
        /// fires PropertyChanged
        /// </summary>
        public String CARepositoryPath
        {
            get { return _caRepoPath; }
            set
            {
                _caRepoPath = value;
                OnPropertyChanged("CARepositoryPath");
            }
        }

        #region IDataErrorInfo Implementation

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
                        //case "AdditionalOIDs":
                        //    if (!String.IsNullOrEmpty(AdditionalOIDs))
                        //    {

                        //    }
                        //    break;

                        //case "RandomGenerator":

                        //    if (RandomGenerator != null)
                        //    {
                        //        RNGArgumentRequired = RandomGenerator.Type == typeof(ReversedWindowGenerator) ||
                        //                              RandomGenerator.Type == typeof(DigestRandomGenerator);
                        //        RNGArgumentName = RandomGenerator.Type == typeof(ReversedWindowGenerator)
                        //                              ? "Random Generator"
                        //                              : "Digest";
                        //    }
                        //    else
                        //    {
                        //        result = "Please select RNG";
                        //    }
                        //    break;
                        //case "RandomGeneratorArgument":
                        //    if (RNGArgumentRequired && RandomGenerator.Type == typeof(ReversedWindowGenerator))
                        //    {
                        //        if (RandomGeneratorArgument.Type == typeof(DigestRandomGenerator) || RandomGeneratorArgument.Type == typeof(ReversedWindowGenerator))
                        //        {
                        //            result = "????";
                        //        }
                        //    }
                        //    break;
                    case "KeyPairType":
                        break;
                    case "SignatureAlgorithm":
                        if (string.IsNullOrEmpty(SignatureAlgorithm))
                            result = "Please enter a SignatureAlgorithm";
                        break;
                    case "KeyPairGenerator":
                        if (KeyPairGenerator == null)
                            result = "Please enter a SignatureAlgorithm";
                        break;
                    case "KeyUsages":
                        //Console.WriteLine(KeyUsages);
                        //KeyUsageUtils.GetKeyUsage(KeyUsages);
                        break;
                    case "ExtendedKeyUsages":
                        //Console.WriteLine(ExtendedKeyUsages);
                        //KeyUsageUtils.GetExtendedKeyUsages(ExtendedKeyUsages);
                        break;
                    case "KeyStrength":
                        if (KeyStrength == 0)
                            result = "Please enter Key Strength";
                        break;
                    case "Validity":
                        if (Validity == 0)
                            result = "Please enter Validity";
                        break;
                    case "PathLenContraint:":
                        break;
                    case "CARepositoryPath":
                        result = CARepositoryPath.IsNullOrEmpty() ? "Please select CA Repository" : null;
                        break;
                }

                return result;
            }
        }

        #endregion
    }
}