using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Reporting;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.Reporting
    {
    using CertificateSKI=CertificateSubjectKeyIdentifier;
    using CertificateAKI=CertificateAuthorityKeyIdentifier;

    [ReportSourceTransform]
    public class GOSTCertificateSignature : ReportSource
        {
        #region M:Build(XmlWriter,{params}Object[])
        public override void Build(XmlWriter target, params Object[] args) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            Build(target,args[0] as X509CertificateStorage);
            }
        #endregion
        #region M:Build(X509CertificateStorage,XmlWriter)
        private void Build(XmlWriter target,X509CertificateStorage source) {
            var cersS = source.Certificates.Where(i => IsGOST(i.SignatureAlgorithm)).ToArray();
            var crlsS = source.CertificateRevocationLists.Where(i => IsGOST(i.SignatureAlgorithm)).OrderByDescending(i => i.EffectiveDate).ToArray();
            var cersI = new Dictionary<String,Boolean>();
            var cersD = new Dictionary<X509Certificate,IList<X509Certificate>>();
            var crlsD = new Dictionary<X509CertificateRevocationList,IList<X509Certificate>>();

            cersS.AsParallel().ForAll(subject => {
                cersI[subject.Thumbprint] = false;
                cersD[subject] = new List<X509Certificate>();
                foreach (var issuer in cersS) {
                    using (var context = CryptographicContext.AcquireContext(issuer.SignatureAlgorithm,CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                        if (context.VerifySignature(out var e, subject,issuer)) {
                            cersD[subject].Add(issuer);
                            if (subject.Thumbprint == issuer.Thumbprint) {
                                cersI[subject.Thumbprint] = true;
                                }
                            }
                        }
                    }
                });
            crlsS.AsParallel().ForAll(subject => {
                crlsD[subject] = new List<X509Certificate>();
                foreach (var issuer in cersS) {
                    using (var context = CryptographicContext.AcquireContext(issuer.SignatureAlgorithm,CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                        if (context.VerifySignature(out var e, subject,issuer)) {
                            crlsD[subject].Add(issuer);
                            }
                        }
                    }
                });

            using (target.ElementScope("CertificateHierarchies")) {
                //using (target.ElementScope("CertificateStore")) {
                //    #region Certificates
                //    using (target.ElementScope("Certificates")) {
                //        foreach (var cer in cersS) {
                //            #region Certificate
                //            using (target.ElementScope("Certificate")) {
                //                target.WriteAttributeString(nameof(cer.Thumbprint),cer.Thumbprint);
                //                target.WriteAttributeString(nameof(cer.NotBefore),cer.NotBefore.ToString("yyyy-MM-ddTHH:mm:ss"));
                //                target.WriteAttributeString(nameof(cer.NotAfter),cer.NotAfter.ToString("yyyy-MM-ddTHH:mm:ss"));
                //                target.WriteAttributeString(nameof(cer.SerialNumber),cer.SerialNumber);
                //                target.WriteAttributeString(nameof(cer.Subject),cer.Subject);
                //                target.WriteAttributeString(nameof(cer.Issuer),cer.Issuer);
                //                target.WriteAttributeString("IsSelfSigned",cersI[cer.Thumbprint] ? "True" : "False");
                //                var SKI = cer.Source.Extensions.OfType<CertificateSKI>().FirstOrDefault();
                //                var AKI = cer.Source.Extensions.OfType<CertificateAKI>().FirstOrDefault();
                //                if (SKI != null) {
                //                    using (target.ElementScope("SKI")) {
                //                        target.WriteAttributeString(nameof(SKI.KeyIdentifier),SKI.KeyIdentifier.ToString("x"));
                //                        }
                //                    }
                //                if (AKI != null) {
                //                    using (target.ElementScope("AKI")) {
                //                        if (AKI.KeyIdentifier != null)      { target.WriteAttributeString(nameof(AKI.KeyIdentifier),AKI.KeyIdentifier.ToString("x")); }
                //                        if (AKI.SerialNumber != null)       { target.WriteAttributeString(nameof(AKI.SerialNumber),AKI.SerialNumber); }
                //                        if (AKI.CertificateIssuer != null)  { target.WriteAttributeString(nameof(AKI.CertificateIssuer),AKI.CertificateIssuer.ToString()); }
                //                        }
                //                    }
                //                }
                //            #endregion
                //            }
                //        }
                //    #endregion
                //    #region CertificateRevocationLists
                //    using (target.ElementScope("CertificateRevocationLists")) {
                //        foreach (var crl in crlsS) {
                //            #region CertificateRevocationList
                //            using (target.ElementScope("CertificateRevocationList")) {
                //                target.WriteAttributeString(nameof(crl.Thumbprint),crl.Thumbprint);
                //                target.WriteAttributeString(nameof(crl.EffectiveDate),crl.EffectiveDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                //                if (crl.NextUpdate != null)
                //                    {
                //                    target.WriteAttributeString(nameof(crl.NextUpdate),crl.NextUpdate.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
                //                    }
                //                target.WriteAttributeString(nameof(crl.Issuer),crl.Issuer);
                //                var SKI = crl.Source.Extensions.OfType<CertificateSKI>().FirstOrDefault();
                //                var AKI = crl.Source.Extensions.OfType<CertificateAKI>().FirstOrDefault();
                //                if (SKI != null) {
                //                    using (target.ElementScope("SKI")) {
                //                        target.WriteAttributeString(nameof(SKI.KeyIdentifier),SKI.KeyIdentifier.ToString("x"));
                //                        }
                //                    }
                //                if (AKI != null) {
                //                    using (target.ElementScope("AKI")) {
                //                        if (AKI.KeyIdentifier != null)      { target.WriteAttributeString(nameof(AKI.KeyIdentifier),AKI.KeyIdentifier.ToString("x")); }
                //                        if (AKI.SerialNumber != null)       { target.WriteAttributeString(nameof(AKI.SerialNumber),AKI.SerialNumber); }
                //                        if (AKI.CertificateIssuer != null)  { target.WriteAttributeString(nameof(AKI.CertificateIssuer),AKI.CertificateIssuer.ToString()); }
                //                        }
                //                    }
                //                }
                //            #endregion
                //            }
                //        }
                //    #endregion
                //    }
                foreach (var cer in cersS.Where(i => i.SerialNumber == "0111")) {
                    (new CertificateHierarchyReportSource()).Build(
                        target,GetHierarchy(cersD,crlsD,cer));
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

        private static void FillNode(
            IDictionary<X509Certificate,IList<X509Certificate>> cersD,
            IDictionary<X509CertificateRevocationList,IList<X509Certificate>> crlsD,
            HierarchyNodeCER target)
            {
            foreach (var cerD in cersD) {
                if (cerD.Key.Thumbprint != target.CER.Thumbprint) {
                    if (cerD.Value.Contains(target.CER)) {
                        var node = new HierarchyNodeCER
                            {
                            CER = cerD.Key
                            };
                        target.DescendantsCER.Add(node);
                        FillNode(cersD, crlsD, node);
                        }
                    }
                }
            if (!cersD.TryGetValue(target.CER, out var cers)) { cers = EmptyArray<X509Certificate>.Value; }
            foreach (var cerI in cers) {
                if (cerI.Thumbprint == target.CER.Thumbprint) {
                    target.IsSelfSigned = true;
                    }
                else
                    {
                    //var node = new HierarchyNodeCER {
                    //    CER = cerI
                    //    };
                    //target.DescendantsCER.Add(node);
                    //FillNode(cersD,crlsD,node);
                    }
                }
            }

        private static CertificateHierarchy GetHierarchy(
            IDictionary<X509Certificate,IList<X509Certificate>> cersD,
            IDictionary<X509CertificateRevocationList,IList<X509Certificate>> crlsD,
            X509Certificate cer)
            {
            var r = new CertificateHierarchy(cer);
            FillNode(cersD,crlsD,r);
            return r;
            }
        }

    //public class GOSTCertificateSignature2 : ReportSource
    //    {
    //    private const String XHtml = "http://www.w3.org/1999/xhtml";
    //    #region M:Build(Object,XmlWriter)
    //    public override void Build(Object source, XmlWriter target) {
    //        if (target == null) { throw new ArgumentNullException(nameof(target)); }
    //        Build(source as X509CertificateStorage, target);
    //        }
    //    #endregion
    //    #region M:Build(X509CertificateStorage,XmlWriter)
    //    private void Build(X509CertificateStorage source, XmlWriter target) {
    //        if (source == null) { throw new ArgumentNullException(nameof(source)); }
    //        if (target == null) { throw new ArgumentNullException(nameof(target)); }
    //        var cersS = source.Certificates.Where(i => IsGOST(i.SignatureAlgorithm) && !i.Issuer.Contains("ВОСХОД")).ToArray();
    //        var crlsS = source.CertificateRevocationLists.Where(i => IsGOST(i.SignatureAlgorithm) && !i.Issuer.Contains("ВОСХОД")).OrderByDescending(i => i.EffectiveDate).ToArray();
    //        var cersI = new ConcurrentDictionary<String,Boolean>();
    //        var cersP = new ConcurrentDictionary<X509Certificate,IList<X509Certificate>>();
    //        var crlsP = new ConcurrentDictionary<X509CertificateRevocationList,IList<X509Certificate>>();
    //        cersS.AsParallel().ForAll(subject => {
    //            cersI[subject.Thumbprint] = false;
    //            cersP[subject] = new List<X509Certificate>();
    //            foreach (var issuer in cersS) {
    //                using (var context = CryptographicContext.AcquireContext(issuer.SignatureAlgorithm,CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
    //                    if (context.VerifySignature(out var e, subject,issuer)) {
    //                        cersP[subject].Add(issuer);
    //                        if (subject.Thumbprint == issuer.Thumbprint) {
    //                            cersI[subject.Thumbprint] = true;
    //                            }
    //                        }
    //                    }
    //                }
    //            });
    //        crlsS.AsParallel().ForAll(subject => {
    //            crlsP[subject] = new List<X509Certificate>();
    //            foreach (var issuer in cersS) {
    //                using (var context = CryptographicContext.AcquireContext(issuer.SignatureAlgorithm,CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
    //                    if (context.VerifySignature(out var e, subject,issuer)) {
    //                        crlsP[subject].Add(issuer);
    //                        }
    //                    }
    //                }
    //            });
    //        using (target.ElementScope("html",XHtml)) {
    //            using (target.ElementScope("head",XHtml)) {
    //                using (target.ElementScope("style",XHtml)) {
    //                    target.WriteAttributeString("type","text/css");
    //                    target.WriteString(@"td.value { border: solid lightgray 1.0pt; mso-border-alt: solid windowtext .5pt; padding: 0cm 5.4pt 0cm 5.4pt; }");
    //                    target.WriteString(@"td.name  { border: solid lightgray 1.0pt; mso-border-alt: solid windowtext .5pt; padding: 0cm 5.4pt 0cm 5.4pt; width: 120px }");
    //                    target.WriteString(@"p  { font-family: Consolas;font-size: small; }");
    //                    }
    //                }
    //            using (target.ElementScope("body",XHtml)) {
    //                using (target.ElementScope("p",XHtml)) {
    //                    target.WriteString("Reverse:");
    //                    }

    //                foreach (var i in cersS) {
    //                    if (GetDescendants(crlsP,i).Count > 0) {
    //                        using (target.ElementScope("table",XHtml)) {
    //                            target.WriteAttributeString("style","border: 1px dashed #C0C0C0; width: 100%");
    //                            using (target.ElementScope("tr",XHtml)) {
    //                                using (target.ElementScope("td",XHtml)) {
    //                                    WriteCrlR(target,crlsP,i);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }

    //                using (target.ElementScope("p",XHtml)) {
    //                    target.WriteString("Forward:");
    //                    }

    //                foreach (var i in crlsS) {
    //                    using (target.ElementScope("table",XHtml)) {
    //                        target.WriteAttributeString("style","border: 1px dashed #C0C0C0; width: 100%");
    //                        using (target.ElementScope("tr",XHtml)) {
    //                            using (target.ElementScope("td",XHtml)) {
    //                                WriteCrlF(target,crlsP,i);
    //                                }
    //                            }
    //                        }
    //                    }

    //                using (target.ElementScope("p",XHtml)) {
    //                    target.WriteString("Reverse:");
    //                    }

    //                foreach (var i in cersS) {
    //                    if (GetDepthR(cersP,i) > 1) {
    //                        using (target.ElementScope("table",XHtml)) {
    //                            target.WriteAttributeString("style","border: 1px dashed #C0C0C0; width: 100%");
    //                            using (target.ElementScope("tr",XHtml)) {
    //                                using (target.ElementScope("td",XHtml)) {
    //                                    WriteCerR(target,cersP,i);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }

    //                /*
    //                using (target.ElementScope("p",XHtml)) {
    //                    target.WriteString("Forward:");
    //                    }
    //                foreach (var i in cersS) {
    //                    if (GetDepthF(cersP,i) > 2) {
    //                        using (target.ElementScope("table",XHtml)) {
    //                            target.WriteAttributeString("style","border: 1px dashed #C0C0C0; width: 100%");
    //                            using (target.ElementScope("tr",XHtml)) {
    //                                using (target.ElementScope("td",XHtml)) {
    //                                    WriteCerF(target,cersP,i);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                */
    //                }
    //            }
    //        }
    //    #endregion
    //    #region M:IsGOST(Oid):Boolean
    //    private static Boolean IsGOST(Oid oid) {
    //        if (oid != null) {
    //            switch (oid.Value) {
    //                case ObjectIdentifiers.szOID_CP_GOST_R3410:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3410EL:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3410_12_256:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3410_12_512:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3410_94_ESDH:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3410_01_ESDH:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3411_R3410:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3411_R3410EL:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3411_12_256_R3410:
    //                case ObjectIdentifiers.szOID_CP_GOST_R3411_12_512_R3410:
    //                    {
    //                    return true;
    //                    }
    //                }
    //            }
    //        return false;
    //        }
    //    #endregion

    //    private static Int32 GetDepthF(IDictionary<X509Certificate,IList<X509Certificate>> cersP, X509Certificate subject)
    //        {
    //        var o = 1;
    //        if (cersP.TryGetValue(subject, out var issuers)) {
    //            var r = 0;
    //            foreach (var issuer in issuers) {
    //                if (issuer.Thumbprint != subject.Thumbprint) {
    //                    r = Math.Max(r,GetDepthF(cersP,issuer));
    //                    }
    //                }
    //            o += r;
    //            }
    //        return o;
    //        }

    //    //private static Int32 GetDepthF(IDictionary<X509CertificateRevocationList,IList<X509Certificate>> cersP, X509CertificateRevocationList subject)
    //    //    {
    //    //    var o = 1;
    //    //    if (cersP.TryGetValue(subject, out var issuers)) {
    //    //        var r = 0;
    //    //        foreach (var issuer in issuers) {
    //    //            r = Math.Max(r,GetDepthF(cersP,issuer));
    //    //            }
    //    //        o += r;
    //    //        }
    //    //    return o;
    //    //    }

    //    private static Int32 GetDepthR(IDictionary<X509Certificate,IList<X509Certificate>> cersP, X509Certificate subject)
    //        {
    //        var o = 1;
    //        var descendants = GetDescendants(cersP,subject);
    //        var r = 0;
    //        foreach (var certificate in descendants) {
    //            r = Math.Max(r,GetDepthR(cersP,certificate));
    //            }
    //        return o + r;
    //        }

    //    private static Boolean IsSelfSigned(IDictionary<X509Certificate,IList<X509Certificate>> cersP, X509Certificate subject) {
    //        if (cersP.TryGetValue(subject, out var issuers)) {
    //            return issuers.Any(i => i.Thumbprint == subject.Thumbprint);
    //            }
    //        return false;
    //        }

    //    private static IList<X509Certificate> GetDescendants(IDictionary<X509Certificate,IList<X509Certificate>> cersP, X509Certificate certificate) {
    //        var r = new HashSet<X509Certificate>();
    //        foreach (var i in cersP.Where(i => i.Key.Thumbprint != certificate.Thumbprint)) {
    //            if ((i.Value != null) && (i.Value.Contains(certificate))) {
    //                r.Add(i.Key);
    //                }
    //            }
    //        return r.Take(2).ToArray();
    //        }

    //    private static IList<X509CertificateRevocationList> GetDescendants(IDictionary<X509CertificateRevocationList,IList<X509Certificate>> cersP, X509Certificate certificate) {
    //        var r = new HashSet<X509CertificateRevocationList>();
    //        foreach (var i in cersP) {
    //            if ((i.Value != null) && (i.Value.Contains(certificate))) {
    //                r.Add(i.Key);
    //                }
    //            }
    //        return r.OrderByDescending(i => i.EffectiveDate).Take(3).ToArray();
    //        }

    //    private static void WriteCerR(XmlWriter writer, IDictionary<X509Certificate,IList<X509Certificate>> cersP, X509Certificate subject) {
    //        using (writer.ElementScope("table",XHtml)) {
    //            var descendants = GetDescendants(cersP,subject);
    //            if (descendants.Count == 0) {
    //                using (writer.ElementScope("tr",XHtml)) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        WriteCer(writer,subject,"gray");
    //                        return;
    //                        }
    //                    }
    //                }
    //            using (writer.ElementScope("tr",XHtml)) {
    //                using (writer.ElementScope("td",XHtml)) {
    //                    writer.WriteAttributeString("colspan",(descendants.Count + 1).ToString());
    //                    writer.WriteAttributeString("align","center");
    //                    var color = IsSelfSigned(cersP,subject)
    //                        ? "green"
    //                        : "gray";
    //                    WriteCer(writer,subject,color);
    //                    }
    //                }
    //            using (writer.ElementScope("tr",XHtml)) {
    //                foreach (var certificate in descendants) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        WriteCerR(writer,cersP,certificate);
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //    private static void WriteCrlR(XmlWriter writer, IDictionary<X509CertificateRevocationList,IList<X509Certificate>> cersP, X509Certificate subject) {
    //        using (writer.ElementScope("table",XHtml)) {
    //            var descendants = GetDescendants(cersP,subject);
    //            if (descendants.Count == 0) {
    //                using (writer.ElementScope("tr",XHtml)) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        WriteCer(writer,subject,"gray");
    //                        return;
    //                        }
    //                    }
    //                }
    //            using (writer.ElementScope("tr",XHtml)) {
    //                using (writer.ElementScope("td",XHtml)) {
    //                    writer.WriteAttributeString("colspan",(descendants.Count + 1).ToString());
    //                    writer.WriteAttributeString("align","center");
    //                    WriteCer(writer,subject,"gray");
    //                    }
    //                }
    //            using (writer.ElementScope("tr",XHtml)) {
    //                foreach (var certificate in descendants) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        WriteCrl(writer,certificate,"gray","#fff9bf");
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //    private static void WriteCerF(XmlWriter writer, IDictionary<X509Certificate,IList<X509Certificate>> cersP, X509Certificate subject) {
    //        using (writer.ElementScope("table",XHtml)) {
    //            if (!cersP.TryGetValue(subject, out var issuers)) {
    //                issuers = new List<X509Certificate>();
    //                }
    //            if (issuers.Count == 0) {
    //                using (writer.ElementScope("tr",XHtml)) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        WriteCer(writer,subject,"red");
    //                        return;
    //                        }
    //                    }
    //                }
    //            if (issuers.Count == 1) {
    //                using (writer.ElementScope("tr",XHtml)) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        var color = IsSelfSigned(cersP,subject)
    //                            ? "green"
    //                            : "gray";
    //                        WriteCer(writer,subject,color);
    //                        }
    //                    foreach (var issuer in issuers.Where(i => i.Thumbprint != subject.Thumbprint)) {
    //                        using (writer.ElementScope("td",XHtml)) {
    //                            WriteCerF(writer,cersP,issuer);
    //                            }
    //                        }
    //                    return;
    //                    }
    //                }
    //            using (writer.ElementScope("tr",XHtml)) {
    //                using (writer.ElementScope("td",XHtml)) {
    //                    writer.WriteAttributeString("rowspan",(issuers.Count + 1).ToString());
    //                    var color = IsSelfSigned(cersP,subject)
    //                        ? "green"
    //                        : "gray";
    //                    WriteCer(writer,subject,color);
    //                    }
    //                }
    //            foreach (var issuer in issuers) {
    //                using (writer.ElementScope("tr",XHtml)) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        if (issuer.Thumbprint != subject.Thumbprint) 
    //                            {
    //                            WriteCerF(writer,cersP,issuer);
    //                            }
    //                        else
    //                            {
    //                            WriteCer(writer,issuer,"lightgray");
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //    private static void WriteCrlF(XmlWriter writer, IDictionary<X509CertificateRevocationList,IList<X509Certificate>> cersP, X509CertificateRevocationList subject) {
    //        using (writer.ElementScope("table",XHtml)) {
    //            if (!cersP.TryGetValue(subject, out var issuers)) {
    //                issuers = new List<X509Certificate>();
    //                }
    //            if (issuers.Count == 0) {
    //                using (writer.ElementScope("tr",XHtml)) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        WriteCrl(writer,subject,"#FF0000");
    //                        return;
    //                        }
    //                    }
    //                }
    //            if (issuers.Count == 1) {
    //                using (writer.ElementScope("tr",XHtml)) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        WriteCrl(writer,subject,"gray");
    //                        }
    //                    foreach (var issuer in issuers) {
    //                        using (writer.ElementScope("td",XHtml)) {
    //                            WriteCer(writer,issuer,"gray");
    //                            }
    //                        }
    //                    return;
    //                    }
    //                }
    //            using (writer.ElementScope("tr",XHtml)) {
    //                using (writer.ElementScope("td",XHtml)) {
    //                    WriteCrl(writer,subject,"gray");
    //                    }
    //                }
    //            foreach (var issuer in issuers) {
    //                using (writer.ElementScope("tr",XHtml)) {
    //                    using (writer.ElementScope("td",XHtml)) {
    //                        WriteCer(writer,issuer,"lightgray");
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //    private static void WriteProperty(XmlWriter writer, String name, String value) {
    //        using (writer.ElementScope("tr",XHtml)) {
    //            using (writer.ElementScope("td",XHtml)) {
    //                writer.WriteAttributeString("class","name");
    //                writer.WriteString(name);
    //                }
    //            using (writer.ElementScope("td",XHtml)) {
    //                writer.WriteAttributeString("class","value");
    //                writer.WriteString(value);
    //                }
    //            }
    //        }

    //    private static void WriteProperty(XmlWriter writer, String name, String value, String color) {
    //        using (writer.ElementScope("tr",XHtml)) {
    //            using (writer.ElementScope("td",XHtml)) {
    //                writer.WriteAttributeString("class","name");
    //                writer.WriteAttributeString("style",$"color: {color}");
    //                writer.WriteString(name);
    //                }
    //            using (writer.ElementScope("td",XHtml)) {
    //                writer.WriteAttributeString("class","value");
    //                writer.WriteAttributeString("style",$"color: {color}");
    //                writer.WriteString(value);
    //                }
    //            }
    //        }

    //    private static void WriteCer(XmlWriter writer, X509Certificate value, String bordercolor) {
    //        WriteCer(writer,value, bordercolor, "white");
    //        }

    //    private static void WriteCer(XmlWriter writer, X509Certificate value, String bordercolor, String background) {
    //        using (writer.ElementScope("table",XHtml)) {
    //            writer.WriteAttributeString("style",$"font-family: Consolas; width: 450px; border-collapse: collapSe; mso-yfti-tbllook: 1184; mso-padding-alt: 0cm 5.4pt 0cm 5.4pt; background-color: {background}; border: 2px solid {bordercolor}; color: {bordercolor}; font-size: small;");
    //            WriteProperty(writer,"NotBefore",value.NotBefore.ToString("G"));
    //            WriteProperty(writer,"NotAfter",value.NotAfter.ToString("G"));
    //            WriteProperty(writer,"SerialNumber",value.SerialNumber);
    //            WriteProperty(writer,"Subject",value.Subject);
    //            WriteProperty(writer,"Issuer",value.Issuer);
    //            var SKI = value.Source.Extensions.OfType<CertificateSKI>().FirstOrDefault();
    //            var AKI = value.Source.Extensions.OfType<CertificateAKI>().FirstOrDefault();
    //            if (SKI != null) {
    //                WriteProperty(writer,"SKI",SKI.KeyIdentifier.ToString("x"));
    //                }
    //            if (AKI != null) {
    //                if (AKI.KeyIdentifier != null)      { WriteProperty(writer,"AKI",AKI.KeyIdentifier.ToString("x")); }
    //                if (AKI.SerialNumber != null)       { WriteProperty(writer,"AKI{SerialNumber}",AKI.SerialNumber); }
    //                if (AKI.CertificateIssuer != null)  { WriteProperty(writer,"AKI{Issuer}",AKI.CertificateIssuer.ToString()); }
    //                }
    //            var PKU = value.Source.Extensions.OfType<CertificatePrivateKeyUsagePeriodExtension>().FirstOrDefault();
    //            if ((PKU != null) && (PKU.NotAfter!= null)) {
    //                var color = (PKU.NotAfter.Value >= DateTime.Now)
    //                    ? "green"
    //                    : "red";
    //                WriteProperty(writer,"PrivateKeyNotAfter",PKU.NotAfter.Value.ToString("G"),color);
    //                }
    //            }
    //        }

    //    private static void WriteCrl(XmlWriter writer, X509CertificateRevocationList value, String bordercolor) {
    //        WriteCrl(writer, value, bordercolor, "white");
    //        }

    //    private static void WriteCrl(XmlWriter writer, X509CertificateRevocationList value, String bordercolor, String background) {
    //        //writer.WriteString(value.ToString());
    //        //return;
    //        using (writer.ElementScope("table",XHtml)) {
    //            var color = (value.NextUpdate.HasValue && (value.NextUpdate.Value > DateTime.Now))
    //                ? "green"
    //                : "red";
    //            writer.WriteAttributeString("style",$"font-family: Consolas; width: 450px; border-collapse: collapSe; mso-yfti-tbllook: 1184; mso-padding-alt: 0cm 5.4pt 0cm 5.4pt; background-color: {background}; border: 2px solid {bordercolor}; color: {bordercolor}; font-size: small;");
    //            WriteProperty(writer,"EffectiveDate",value.EffectiveDate.ToString("G"));
    //            WriteProperty(writer,"NotAfter",value.NextUpdate.HasValue
    //                ? value.NextUpdate.Value.ToString("G")
    //                : "{none}",color);
    //            WriteProperty(writer,"Issuer",value.Issuer);
    //            var SKI = value.Source.Extensions.OfType<CertificateSKI>().FirstOrDefault();
    //            var AKI = value.Source.Extensions.OfType<CertificateAKI>().FirstOrDefault();
    //            if (SKI != null) {
    //                WriteProperty(writer,"SKI",SKI.KeyIdentifier.ToString("x"));
    //                }
    //            if (AKI != null) {
    //                if (AKI.KeyIdentifier != null)      { WriteProperty(writer,"AKI",AKI.KeyIdentifier.ToString("x")); }
    //                if (AKI.SerialNumber != null)       { WriteProperty(writer,"AKI{SerialNumber}",AKI.SerialNumber); }
    //                if (AKI.CertificateIssuer != null)  { WriteProperty(writer,"AKI{Issuer}",AKI.CertificateIssuer.ToString()); }
    //                }
    //            }
    //        }
    //    }
    }
