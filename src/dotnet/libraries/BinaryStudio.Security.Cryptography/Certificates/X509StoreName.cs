namespace BinaryStudio.Security.Cryptography.Certificates
    {
    /// <summary>
    /// Specifies the name of the X.509 certificate store to open.
    /// </summary>
    public enum X509StoreName
        {
        AddressBook = 1,
        AuthRoot = 2,
        CertificateAuthority = 3,
        Disallowed = 4,
        My = 5,
        Root = 6,
        TrustedPeople = 7,
        /** <summary>1111</summary> */
        TrustedPublisher = 8,
        /* <summary>1111</summary> */
        TrustedDevices = 10,
        /* 1111 */
        Device = 9,
        /// <summary>
        /// YYYY
        /// </summary>
        NTAuth = 11,
        /** XXX */
        Memory = 12
        }
    }