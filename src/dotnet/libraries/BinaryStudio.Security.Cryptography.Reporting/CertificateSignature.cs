using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Reporting;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Reporting
    {
    using CertificateSKI=CertificateSubjectKeyIdentifier;
    using CertificateAKI=CertificateAuthorityKeyIdentifier;
    public class GOSTCertificateSignature : ReportSource
        {
        #region M:Build(Object,XmlWriter)
        public override void Build(Object source, XmlWriter target) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            Build(source as X509CertificateStorage, target);
            }
        #endregion
        #region M:Build(X509CertificateStorage,XmlWriter)
        private void Build(X509CertificateStorage source, XmlWriter target) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            using (target.ElementScope("Report")) {
                var cersS = source.Certificates.Where(i => IsGOST(i.SignatureAlgorithm)).ToArray();
                var crlsS = source.CertificateRevocationLists.Where(i => IsGOST(i.SignatureAlgorithm)).ToArray();
                var cersP = new Dictionary<X509Certificate,X509Certificate>();
                foreach (var subject in cersS) {
                    foreach (var issuer in cersS) {
                        using (var context = CryptographicContext.AcquireContext(issuer.SignatureAlgorithm,CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                            if (context.VerifySignature(out var e, subject,issuer)) {
                                cersP[subject] = issuer;
                                break;
                                }
                            }
                        }
                    }
                using (target.ElementScope("Page")) {
                    foreach (var i in cersS) {
                        using (target.ElementScope("Chain")) {
                            var j = i;
                            while (true) {
                                WriteCertificate(target,j);
                                if (cersP.TryGetValue(j, out var issuer)) {
                                    if (issuer.Thumbprint == j.Thumbprint) {
                                        break;
                                        }
                                    j = issuer;
                                    }
                                else
                                    {
                                    break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        #endregion
        #region M:IsGOST(Oid):Boolean
        private static Boolean IsGOST(Oid oid) {
            if (oid != null) {
                switch (oid.Value) {
                    case ObjectIdentifiers.szOID_CP_GOST_R3410:
                    case ObjectIdentifiers.szOID_CP_GOST_R3410EL:
                    case ObjectIdentifiers.szOID_CP_GOST_R3410_12_256:
                    case ObjectIdentifiers.szOID_CP_GOST_R3410_12_512:
                    case ObjectIdentifiers.szOID_CP_GOST_R3410_94_ESDH:
                    case ObjectIdentifiers.szOID_CP_GOST_R3410_01_ESDH:
                    case ObjectIdentifiers.szOID_CP_GOST_R3411_R3410:
                    case ObjectIdentifiers.szOID_CP_GOST_R3411_R3410EL:
                    case ObjectIdentifiers.szOID_CP_GOST_R3411_12_256_R3410:
                    case ObjectIdentifiers.szOID_CP_GOST_R3411_12_512_R3410:
                        {
                        return true;
                        }
                    }
                }
            return false;
            }
        #endregion

        private static void WriteCertificate(XmlWriter writer, X509Certificate value) {
            using (writer.ElementScope("Certificate")) {
                writer.WriteAttributeString("NotBefore",value.NotBefore.ToString("yyyyMMddTHHmmss"));
                writer.WriteAttributeString("NotAfter",value.NotAfter.ToString("yyyyMMddTHHmmss"));
                writer.WriteAttributeString("SerialNumber",value.SerialNumber);
                writer.WriteAttributeString("Subject",value.Subject);
                writer.WriteAttributeString("Issuer",value.Issuer);
                var SKI = value.Source.Extensions.OfType<CertificateSKI>().FirstOrDefault();
                var AKI = value.Source.Extensions.OfType<CertificateAKI>().FirstOrDefault();
                if (SKI != null) {
                    using (writer.ElementScope("SKI")) {
                        writer.WriteAttributeString("KeyIdentifier",SKI.KeyIdentifier.ToString("x"));
                        }
                    }
                if (AKI != null) {
                    using (writer.ElementScope("AKI")) {
                        if (AKI.KeyIdentifier != null)      { writer.WriteAttributeString("KeyIdentifier",AKI.KeyIdentifier.ToString("x")); }
                        if (AKI.SerialNumber != null)       { writer.WriteAttributeString("SerialNumber",AKI.SerialNumber); }
                        if (AKI.CertificateIssuer != null)  { writer.WriteAttributeString("CertificateIssuer",AKI.CertificateIssuer.ToString()); }
                        }
                    }
                }
            }
        }
    }
