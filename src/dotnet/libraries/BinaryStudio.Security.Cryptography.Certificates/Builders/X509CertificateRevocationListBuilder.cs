using System;
using System.Collections.Generic;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;

namespace BinaryStudio.Security.Cryptography.Certificates.Builders
    {
    public class X509CertificateRevocationListBuilder
        {
        public DateTime EffectiveDate { get; }
        public DateTime NextUpdate { get; }
        public X509CertificateRevocationListBuilder(DateTime EffectiveDate, DateTime NextUpdate)
            {
            this.EffectiveDate = EffectiveDate;
            this.NextUpdate = NextUpdate;
            }

        public void Build(X509Certificate IssuerCertificate, RequestSecureString RequestSecureString, out X509CertificateRevocationList CRL) {
            if (IssuerCertificate == null) { throw new ArgumentNullException(nameof(IssuerCertificate)); }
            CRL = null;
            var r = new Asn1Sequence{
                new Asn1Sequence {
                    new Asn1Integer(1),
                    new Asn1Sequence {
                        new Asn1ObjectIdentifier(IssuerCertificate.SignatureAlgorithm)
                        },
                    IssuerCertificate.Source.Issuer.BuildSequence(),
                    new Asn1UtcTime(EffectiveDate),
                    new Asn1UtcTime(NextUpdate),
                    new Asn1Sequence()
                    },
                new Asn1Sequence{
                    new Asn1ObjectIdentifier(IssuerCertificate.SignatureAlgorithm)
                    },
                new Asn1BitString()
                };
            }
        }
    }