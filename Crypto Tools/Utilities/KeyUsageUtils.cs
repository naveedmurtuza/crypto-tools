using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoTools.Helpers;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;

namespace CryptoTools.Utilities
{
    public class KeyUsageUtils
    {
        private static IList<KeyUsageWrapper<Int32>> _usages;
        private static IList<KeyUsageWrapper<KeyPurposeID>> _exUsages;

        public static IEnumerable<KeyUsageWrapper<Int32>> GetKeyUsages()
        {
            return _usages ?? (_usages = new List<KeyUsageWrapper<Int32>>
                                             {
                                                 new KeyUsageWrapper<int>(KeyUsage.CrlSign, "CrlSign"),
                                                 new KeyUsageWrapper<int>(KeyUsage.DataEncipherment, "DataEncipherment"),
                                                 new KeyUsageWrapper<int>(KeyUsage.DecipherOnly, "DecipherOnly"),
                                                 new KeyUsageWrapper<int>(KeyUsage.DigitalSignature, "DigitalSignature"),
                                                 new KeyUsageWrapper<int>(KeyUsage.EncipherOnly, "EncipherOnly"),
                                                 new KeyUsageWrapper<int>(KeyUsage.KeyAgreement, "KeyAgreement"),
                                                 new KeyUsageWrapper<int>(KeyUsage.KeyCertSign, "KeyCertSign"),
                                                 new KeyUsageWrapper<int>(KeyUsage.KeyEncipherment, "KeyEncipherment"),
                                                 new KeyUsageWrapper<int>(KeyUsage.NonRepudiation, "NonRepudiation")
                                             });
        }

        public static IEnumerable<KeyUsageWrapper<KeyPurposeID>> GetExtendedKeyUsages()
        {
            return _exUsages ?? (_exUsages = new List<KeyUsageWrapper<KeyPurposeID>>
                                                 {
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.AnyExtendedKeyUsage,
                                                                                       "AnyExtendedKeyUsage"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPClientAuth,
                                                                                       "IdKPClientAuth"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPCodeSigning,
                                                                                       "IdKPCodeSigning"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPEmailProtection,
                                                                                       "IdKPEmailProtection"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPIpsecEndSystem,
                                                                                       "IdKPIpsecEndSystem"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPIpsecTunnel,
                                                                                       "IdKPIpsecTunnel"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPIpsecUser,
                                                                                       "IdKPIpsecUser"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPOcspSigning,
                                                                                       "IdKPOcspSigning"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPServerAuth,
                                                                                       "IdKPServerAuth"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPSmartCardLogon,
                                                                                       "IdKPSmartCardLogon"),
                                                     new KeyUsageWrapper<KeyPurposeID>(KeyPurposeID.IdKPTimeStamping,
                                                                                       "IdKPTimeStamping")
                                                 });
        }

        public static int GetKeyUsage(String values)
        {
            return values == null ? 0 : values.Split(',').Select(Int32.Parse).ToList().Or();
        }

        public static List<DerObjectIdentifier> GetExtendedKeyUsages(String values)
        {
            if (values == null)
                return null;
            String[] exKeyValues = values.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            return exKeyValues.Select(item => DerObjectIdentifier.GetInstance(new DerObjectIdentifier(item))).ToList();
        }
    }
}
