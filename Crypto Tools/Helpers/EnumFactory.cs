using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoTools.Helpers
{
    public enum KeyStoreEntryType
    {
        /// <summary>
        /// Value to place in the type column for a key pair entry
        /// </summary>
        KeyPairEntry,

        /// <summary>
        /// Value to place in the type column for a trusted certificate entry
        /// </summary>
        TrustCertEntry = 1,

        /// <summary>
        /// Value to place in the type column for a key entry
        /// </summary>
        KeyEntry = 2
    }
    public enum KeyPairType
    {
        /** RSA key pair type. */
        Rsa,
        /** DSA key pair type. */
        Dsa,
        /** ECDSA key pair type. */
        Ec,
        Dh,
        Elgamal,
        Gost3410
    }

    /// <summary>
    /// Since the BouncyCastle APIs uses constant values <see cref="Org.BouncyCastle.Asn1.X509.CrlReason"/>
    /// </summary>
    public enum CrlReasonEx
    {
        Unspecified = 0,
        KeyCompromise = 1,
        CACompromise = 2,
        AffiliationChanged = 3,
        Superseded = 4,
        CessationOfOperation = 5,
        CertificateHold = 6,
        RemoveFromCrl = 8,
        PrivilegeWithdrawn = 9,
        AACompromise = 10
    }
}
