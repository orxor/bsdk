using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;

namespace BinaryStudio.Security.Cryptography.Certificates.Builders
    {
    public class X509CertificateRevocationListBuilder
        {
        public DateTime EffectiveDate { get; }
        public DateTime NextUpdate { get; }
        public BigInteger Number { get; }
        public List<CertificateRevocationListEntry> Entries { get; }

        public X509CertificateRevocationListBuilder(DateTime EffectiveDate, DateTime NextUpdate, BigInteger Number)
            {
            Entries = new List<CertificateRevocationListEntry>();
            this.EffectiveDate = EffectiveDate;
            this.NextUpdate = NextUpdate;
            this.Number = Number;
            }

        public void Build(X509Certificate IssuerCertificate, RequestSecureString RequestSecureString, out X509CertificateRevocationList CRL) {
            if (IssuerCertificate == null) { throw new ArgumentNullException(nameof(IssuerCertificate)); }
            CRL = null;
            var extensions = new Asn1Sequence{
                new CRLNumber(Number)
                };
            var SKI = IssuerCertificate.Source.Extensions.
                OfType<CertificateSubjectKeyIdentifier>().
                FirstOrDefault();
            if (SKI != null) {
                extensions.Add(new CertificateAuthorityKeyIdentifier(IssuerCertificate.Source));
                }
            var r = new Asn1Sequence{
                new Asn1Sequence {
                    new Asn1Integer(1),
                    new Asn1Sequence {
                        new Asn1ObjectIdentifier(IssuerCertificate.SignatureAlgorithm)
                        },
                    IssuerCertificate.Source.Subject.BuildSequence(),
                    new Asn1UtcTime(EffectiveDate),
                    new Asn1UtcTime(NextUpdate),
                    new Asn1Sequence(Entries.OfType<Asn1Object>().ToArray()),
                    new Asn1ContextSpecificObject(0,extensions)
                    },
                new Asn1Sequence{
                    new Asn1ObjectIdentifier(IssuerCertificate.SignatureAlgorithm)
                    },
                new Asn1BitString()
                };
            var ProviderType = CryptographicContext.ProviderTypeFromAlgId(IssuerCertificate.SignatureAlgorithm);
            using (var o = new MemoryStream()) {
                r[0].WriteTo(o, true);
                o.Seek(0,SeekOrigin.Begin);
                var Flags = (RequestSecureString != null)
                    ? CryptographicContextFlags.CRYPT_SILENT
                    : CryptographicContextFlags.CRYPT_NONE;
                using (var context = CryptographicContext.AcquireContext(ProviderType,IssuerCertificate.Container,Flags)) {
                    if (RequestSecureString != null) {
                        var e = new RequestSecureStringEventArgs();
                        if (RequestSecureString.GetSecureString(context,e) == HRESULT.S_OK) {
                            context.SecureCode = e.SecureString;
                            }
                        }
                    using (var engine = new CryptHashAlgorithm(context, IssuerCertificate.HashAlgorithm)) {
                        engine.Compute(o);
                        engine.SignHash(IssuerCertificate.KeySpec, out var Digest, out var Signature);
                        r[2] = new Asn1BitString(0,Signature.Reverse().ToArray());
                        }
                    }
                }
            using (var o = new MemoryStream()) {
                r.WriteTo(o,true);
                CRL = new X509CertificateRevocationList(o.ToArray());
                }
            }
        }
    }