using System;
using System.Runtime.InteropServices;

namespace CryptoTools.Utilities
{
    public class CryptUI
    {
        #region CertStoreContext enum

        public enum CertStoreContent //dwContentType for CryptQueryObject
        {
            Certificate = 1,
            Ctl = 2,
            Crl = 3,
            SerializedStore = 4,
            SerializedCert = 5,
            SerializedCtl = 6,
            SerializedCrl = 7,
            Pkcs7Signed = 8,
            Pkcs7Unsigned = 9,
            Pkcs7SignedEmbed = 10,
            Pkcs10 = 11,
            Pfx = 12,
            CertPair = 13,
            PfxAndLoad = 14,
        }

        #endregion

        public enum CertQueryFormat
        {
            /// <summary>
            /// the content is in binary format
            /// </summary>
            Binary = 1,

            /// <summary>
            /// //the content is base64 encoded
            /// </summary>
            Base64Encoded = 2,

            /// <summary>
            /// the content can be of any format
            /// </summary>
            AsnASciiHexEncoded = 3
        }

        public enum CertQueryFormatFlag
        {
            /// <summary>
            /// the content is in binary format
            /// </summary>
            Binary = 1 << CertQueryFormat.Binary,

            /// <summary>
            /// //the content is base64 encoded
            /// </summary>
            Base64Encoded = 1 << CertQueryFormat.Base64Encoded,

            /// <summary>
            /// //the content is ascii hex encoded with "{ASN}" prefix
            /// </summary>
            AsnAsciiHexEncoded = 1 << CertQueryFormat.AsnASciiHexEncoded,

            /// <summary>
            /// the content can be of any format
            /// </summary>
            All = CertQueryFormat.Binary | CertQueryFormat.Base64Encoded | CertQueryFormat.AsnASciiHexEncoded
        }

        public enum CertStoreContentFlag
        {
            /// <summary>
            /// encoded single certificate
            /// </summary>
            Certificate = 1 << CertStoreContent.Certificate,

            /// <summary>
            /// encoded single CRL
            /// </summary>
            Crl = 1 << CertStoreContent.Crl,

            /// <summary>
            /// encoded single CTL
            /// </summary>
            Ctl = 1 << CertStoreContent.Ctl,
            SerializedStore = 1 << CertStoreContent.SerializedStore,
            SerializedCert = 1 << CertStoreContent.SerializedCert,
            SerializedCtl = 1 << CertStoreContent.SerializedCtl,
            SerializedCrl = 1 << CertStoreContent.SerializedCrl,
            Pkcs7Signed = 1 << CertStoreContent.Pkcs7Signed,
            Pkcs7Unsigned = 1 << CertStoreContent.Pkcs7Unsigned,
            Pkcs7SignedEmbed = 1 << CertStoreContent.Pkcs7SignedEmbed,
            Pkcs10 = 1 << CertStoreContent.Pkcs10,
            Pfx = 1 << CertStoreContent.Pfx,
            CertPair = 1 << CertStoreContent.CertPair,
            PfxAndLoad = 1 << CertStoreContent.PfxAndLoad,

            FlagAll =
                Certificate |
                Crl |
                Ctl |
                SerializedStore |
                SerializedCert |
                SerializedCtl |
                SerializedCrl |
                Pkcs7Signed |
                Pkcs7Unsigned |
                Pkcs7SignedEmbed |
                Pkcs10 |
                Pfx |
                CertPair,
        }

        public enum ObjectType
        {
            /// <summary>
            /// The object is stored in a file.
            /// </summary>
            File = 0x00000001,

            /// <summary>
            /// The object is stored in a structure in memory.
            /// </summary>
            Blob = 0x00000002
        }

        /// <summary>
        /// The CryptUIDlgViewContext function displays a certificate, CTL, or CRL context.
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa380290%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="dwContextType">value indicating whether pvContext is a pointer to a certificate, a CRL, or a CTL context as indicated in the following table. </param>
        /// <param name="pvContext">A pointer to a certificate, CRL, or CTL context to be displayed.</param>
        /// <param name="hwnd">Handle of the window for the display. If NULL, the display defaults to the desktop window.</param>
        /// <param name="szTitle">Display title string. If NULL, the default context type is used as the title.</param>
        /// <param name="dwFlags">Currently not used and should be set to 0.</param>
        /// <param name="pvReserved">Reserved for future use.</param>
        /// <returns>This function returns TRUE on success and FALSE on failure.</returns>
        [DllImport("CryptUI.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean CryptUIDlgViewContext(int dwContextType, IntPtr pvContext, IntPtr hwnd,
                                                            [MarshalAs(UnmanagedType.LPWStr)] String szTitle,
                                                            int dwFlags, IntPtr pvReserved);

/*
        [DllImport("CryptUI.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean CryptUIDlgViewCertificate(
            ref CRYPTUI_VIEWCERTIFICATE_STRUCT pCertViewInfo,
            ref bool pfPropertiesChanged
            );
*/

        /// <summary>
        /// The CertFreeCRLContext function frees a certificate revocation list (CRL) context by decrementing its reference count. When the reference count goes to zero, CertFreeCRLContext frees the memory used by a CRL context.
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa376076%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="pCrlContext">A pointer to the CRL_CONTEXT to be freed.</param>
        /// <returns>The function always returns nonzero.</returns>
        [DllImport("CRYPT32.DLL", EntryPoint = "CertFreeCRLContext", SetLastError = true)]
        private static extern Boolean CertFreeCRLContext(
            IntPtr pCrlContext
            );

        /// <summary>
        /// The CertFreeCertificateContext function frees a certificate context by decrementing its reference count. When the reference count goes to zero, CertFreeCertificateContext frees the memory used by a certificate context.
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa376075%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="pCertContext">A pointer to the CERT_CONTEXT to be freed.</param>
        /// <returns>The function always returns nonzero.</returns>
        [DllImport("crypt32.dll", EntryPoint = "CertFreeCertificateContext", SetLastError = true)]
        private static extern bool CertFreeCertificateContext(IntPtr pCertContext);


        /// <summary>
        /// The CryptQueryObject function retrieves information about the contents of a cryptography API object, 
        /// such as a certificate, a certificate revocation list, or a certificate trust list. 
        /// The object can either reside in a structure in memory or be contained in a file.
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa380264%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="dwObjectType">Indicates the type of the object to be queried. This must be one of the following values.</param>
        /// <param name="pvObject">A pointer to the object to be queried. The type of data pointer depends on the contents of the dwObjectType parameter.</param>
        /// <param name="dwExpectedContentTypeFlags">Indicates the expected content type. This can be one of the following values.</param>
        /// <param name="dwExpectedFormatTypeFlags">Indicates the expected format of the returned type. This can be one of the following values.</param>
        /// <param name="dwFlags">This parameter is reserved for future use and must be set to zero.</param>
        /// <param name="pdwMsgAndCertEncodingType">A pointer to a DWORD value that receives the type of encoding used in the message. If this information is not needed, set this parameter to NULL.This parameter can receives a combination of one or more of the following values.</param>
        /// <param name="pdwContentType">A pointer to a DWORD value that receives the actual type of the content. If this information is not needed, set this parameter to NULL. The returned content type can be one of the following values.</param>
        /// <param name="pdwFormatType">A pointer to a DWORD value that receives the actual format type of the content. If this information is not needed, set this parameter to NULL. The returned format type can be one of the following values.</param>
        /// <param name="phCertStore">A pointer to an HCERTSTORE value that receives a handle to a certificate store that includes all of the certificates, CRLs, and CTLs in the object.</param>
        /// <param name="phMsg">A pointer to an HCRYPTMSG value that receives the handle of an opened message.If this information is not needed, set this parameter to NULL.</param>
        /// <param name="ppvContext">A pointer to a pointer that receives additional information about the object.The format of this data depends on the value received by the dwContentType parameter. 
        /// If this information is not needed, set this parameter to IntPtr.Zero.</param>
        /// <returns>If the function succeeds, the function returns nonzero.If the function fails, it returns zero. </returns>
        [DllImport("CRYPT32.DLL", EntryPoint = "CryptQueryObject", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean CryptQueryObject(
            Int32 dwObjectType,
            [MarshalAs(UnmanagedType.LPWStr)] String pvObject,
            Int32 dwExpectedContentTypeFlags,
            Int32 dwExpectedFormatTypeFlags,
            Int32 dwFlags,
            IntPtr pdwMsgAndCertEncodingType,
            out int pdwContentType,
            IntPtr pdwFormatType,
            IntPtr phCertStore,
            IntPtr phMsg,
            ref IntPtr ppvContext
            );

        public static void  CryptUIViewContext(String file, String title, IntPtr hwdParent)
        {
            CryptUIViewContext(file, ObjectType.File, CertStoreContent.Certificate | CertStoreContent.Crl,
                               CertStoreContentFlag.FlagAll, CertQueryFormatFlag.All, title, hwdParent);
        }
        /// <summary>
        ///  The CryptUIDlgViewContext function displays a certificate, CTL, or CRL context.
        /// </summary>
        /// <param name="file">The absolute path to certificate, crl or ctl</param>
        /// <param name="type">one of the values to <code>ObjectType</code></param>
        /// <param name="certStoreContent">one of the values to <code>CertStoreContent</code></param>
        /// <param name="contentFlag">one of the values to <code>CertStoreContentFlag</code></param>
        /// <param name="formatFlag">one of the values to <code>CertQueryFormatFlag</code></param>
        /// <param name="hwdParent">Handle of the window for the display.Required if you want the window to be modal</param>
        /// <param name="title">title of the window</param>
        public static void CryptUIViewContext(String file, ObjectType type, CertStoreContent certStoreContent,
                                              CertStoreContentFlag contentFlag, CertQueryFormatFlag formatFlag,
                                              String title, IntPtr hwdParent)
        {
            IntPtr pvContext = IntPtr.Zero;
            int contentType;
            bool bResult = CryptQueryObject(
                (int) type,
                file,
                (int) contentFlag,
                (int) formatFlag,
                0, //reserved for future use
                IntPtr.Zero,out contentType,
                
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                ref pvContext
                );
            if (!bResult)
            {
                throw new Exception("CryptQueryObject error #" + Marshal.GetLastWin32Error());
            }
            try
            {
                CryptUIDlgViewContext((contentFlag == CertStoreContentFlag.FlagAll ? contentType : (int)certStoreContent), pvContext, hwdParent, title, 0, IntPtr.Zero);
            }
            finally
            {
                switch (contentFlag)
                {
                    case CertStoreContentFlag.Certificate:
                        CertFreeCertificateContext(pvContext);
                        break;
                    case CertStoreContentFlag.Crl:
                        CertFreeCRLContext(pvContext);
                        break;
                        //TODO: free up ctl
                }
            }
        }

        #region Structures

        #region Nested type: CRYPTOAPI_BLOB

        [StructLayout(LayoutKind.Sequential)]
        public struct CRYPTOAPI_BLOB
        {
            public Int32 cbData;
            public IntPtr pbData;
        }

        #endregion

        #region Nested type: CRYPTUI_VIEWCERTIFICATE_STRUCT

        public struct CRYPTUI_VIEWCERTIFICATE_STRUCT
        {
            public int cPropSheetPages;
            public int cPurposes;
            public int cStores;
            public int dwFlags;
            public int dwSize;
            public Boolean fCounterSigner;
            public Boolean fpCryptProviderDataTrustedUsage;
            public IntPtr hwndParent;
            public int idxCert;
            public int idxCounterSigner;
            public int idxSigner;
            public int nStartPage;
            public IntPtr pCertContext;
            public IntPtr pCryptProviderData; // or hWVTStateData
            public IntPtr rgPropSheetPages;
            public IntPtr rghStores;
            public IntPtr rgszPurposes;
            [MarshalAs(UnmanagedType.LPWStr)] public String szTitle;
        }

        #endregion

        #endregion
    }
}