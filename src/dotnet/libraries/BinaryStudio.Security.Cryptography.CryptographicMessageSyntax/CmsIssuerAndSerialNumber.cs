using System;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [DefaultProperty(nameof(CertificateSerialNumber))]
    public class CmsIssuerAndSerialNumber : CmsSignerIdentifier, ICmsIssuerAndSerialNumber
        {
        public String CertificateSerialNumber { get; }
        public X509RelativeDistinguishedNameSequence CertificateIssuer { get; }
        IX509GeneralName ICmsIssuerAndSerialNumber.CertificateIssuer { get { return CertificateIssuer; }}

        public CmsIssuerAndSerialNumber(Asn1Sequence o)
            : base(o)
            {
            if (o == null) { throw new ArgumentNullException(nameof(o)); }
            if (o.Count != 2) { throw new ArgumentOutOfRangeException(nameof(o)); }
            CertificateSerialNumber = ((BigInteger)(Asn1Integer)o[1]).ToByteArray().Reverse().ToArray().ToString("x");
            CertificateIssuer = X509RelativeDistinguishedNameSequence.Build(o[0]);
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return CertificateSerialNumber;
            }
        }
    }