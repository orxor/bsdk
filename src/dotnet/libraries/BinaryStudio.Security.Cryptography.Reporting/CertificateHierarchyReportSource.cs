using System;
using System.Linq;
using System.Xml;
using BinaryStudio.Reporting;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Reporting
    {
    using CertificateSKI=CertificateSubjectKeyIdentifier;
    using CertificateAKI=CertificateAuthorityKeyIdentifier;

    internal class CertificateHierarchyReportSource : ReportSource
        {
        #region M:Build(XmlWriter,{params}Object[])
        public override void Build(XmlWriter target, params Object[] args)
            {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            Build(target,args[0] as CertificateHierarchy);
            }
        #endregion
        #region M:Build(XmlWriter,CertificateHierarchy)
        private void Build(XmlWriter target, CertificateHierarchy source)
            {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            using (target.ElementScope("CertificateHierarchy")) {
                WriteCer(target, source);
                }
            }
        #endregion

        private static void WriteCer(XmlWriter target, HierarchyNodeCER cer) {
            using (target.ElementScope("Certificate")) {
                target.WriteAttributeString(nameof(cer.CER.Thumbprint),cer.CER.Thumbprint);
                target.WriteAttributeString(nameof(cer.IsSelfSigned),cer.IsSelfSigned ? "True" : "False");
                target.WriteElementString(nameof(cer.CER.NotBefore),cer.CER.NotBefore.ToString("yyyy-MM-ddTHH:mm:ss"));
                target.WriteElementString(nameof(cer.CER.NotAfter),cer.CER.NotAfter.ToString("yyyy-MM-ddTHH:mm:ss"));
                target.WriteElementString(nameof(cer.CER.SerialNumber),cer.CER.SerialNumber);
                target.WriteElementString(nameof(cer.CER.Subject),cer.CER.Subject);
                target.WriteElementString(nameof(cer.CER.Issuer),cer.CER.Issuer);
                var SKI = cer.CER.Source.Extensions.OfType<CertificateSKI>().FirstOrDefault();
                var AKI = cer.CER.Source.Extensions.OfType<CertificateAKI>().FirstOrDefault();
                if (SKI != null) {
                    using (target.ElementScope("SKI")) {
                        target.WriteAttributeString("Key",SKI.KeyIdentifier.ToString("x"));
                        }
                    }
                if (AKI != null) {
                    using (target.ElementScope("AKI")) {
                        if (AKI.KeyIdentifier != null)      { target.WriteAttributeString("Key",AKI.KeyIdentifier.ToString("x")); }
                        if (AKI.SerialNumber != null)       { target.WriteElementString(nameof(AKI.SerialNumber),AKI.SerialNumber); }
                        if (AKI.CertificateIssuer != null)  { target.WriteElementString("Issuer",AKI.CertificateIssuer.ToString()); }
                        }
                    }
                if (cer.DescendantsCER.Any()) {
                    using (target.ElementScope("Descendants")) {
                        foreach (var cerI in cer.DescendantsCER) {
                            WriteCer(target,cerI);
                            }
                        }
                    }
                }
            }
        }
    }