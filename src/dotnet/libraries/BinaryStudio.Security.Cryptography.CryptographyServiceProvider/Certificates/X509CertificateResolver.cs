using System;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public abstract class X509CertificateResolver
        {
        public abstract X509Certificate FindBySerialNumber(String serialnumber, String issuer);
        }
    }