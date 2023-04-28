using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Internal;
using BinaryStudio.Serialization;
using X509Certificate = BinaryStudio.Security.Cryptography.Certificates.X509Certificate;

namespace BinaryStudio.Security.Cryptography
    {
    using CRYPT_DATA_BLOB = CRYPT_BLOB;
    public partial class CryptographicContext
        {
        private static readonly Random random = new Random();

        #region M:MakeCertificate(CryptKey,String,String,DateTime,DateTime,IList<CertificateExtension>):X509Certificate
        private unsafe X509Certificate MakeCertificate(CryptKey Key, String SubjectName, String AlgId,DateTime NotBefore,DateTime NotAfter,IList<CertificateExtension> Extensions) {
            EnsureEntries(out var entries);
            var SubjectIssuerBlobM = CertStrToName(SubjectName);
            fixed (Byte* SubjectIssuerBlobU = SubjectIssuerBlobM) {
                var SubjectIssuerBlob = new CRYPT_BLOB{
                    Size = SubjectIssuerBlobM.Length,
                    Data = SubjectIssuerBlobU
                    };
                var AlgIdT = new CRYPT_ALGORITHM_IDENTIFIER {
                    ObjectId = (IntPtr)LocalMemoryManager.StringToMem(AlgId,Encoding.ASCII)
                    };
                var NotBeforeT = NotBefore.ToSystemTime();
                var NotAfterT  = NotAfter.ToSystemTime();
                if (Extensions != null) {
                    using (var manager = new LocalMemoryManager()) {
                        var ExtensionsA = (CERT_EXTENSION*)manager.Alloc(Extensions.Count*sizeof(CERT_EXTENSION));
                        var ExtensionsE = new CERT_EXTENSIONS {
                            ExtensionCount = Extensions.Count,
                            Extensions = ExtensionsA
                            };
                        for (var i = 0; i < Extensions.Count; i++) {
                            ExtensionsA[i].pszObjId = (IntPtr)manager.StringToMem(Extensions[i].Identifier.Value,Encoding.ASCII);
                            ExtensionsA[i].fCritical = Extensions[i].IsCritical;
                            using (var MemoryStream = new MemoryStream()) {
                                Extensions[i].BuildBody();
                                Extensions[i].Body[0].WriteTo(MemoryStream,true);
                                var block = MemoryStream.ToArray();
                                ExtensionsA[i].Value.Size = block.Length;
                                ExtensionsA[i].Value.Data = (Byte*)manager.Alloc(block);
                                }
                            }
                        var context = Key.Context;
                        var pi = new CRYPT_KEY_PROV_INFO {
                            ContainerName = (IntPtr)manager.StringToMem(Key.Container, entries.UnicodeEncoding),
                            ProviderName = (IntPtr)manager.StringToMem(context.ProviderName, entries.UnicodeEncoding),
                            ProviderFlags = context.ProviderFlags & (~CryptographicContextFlags.CRYPT_SILENT),
                            ProviderType = context.ProviderType,
                            KeySpec = Key.KeySpec
                            };
                        return (new X509Certificate(Validate(entries,entries.CertCreateSelfSignCertificate(
                            Handle,ref SubjectIssuerBlob,0,&pi,&AlgIdT,
                            &NotBeforeT,&NotAfterT,&ExtensionsE),NotZero))).
                            SetProviderInfo(Key);
                        }
                    }
                return (new X509Certificate(Validate(entries,entries.CertCreateSelfSignCertificate(
                    Handle,ref SubjectIssuerBlob,0,null,
                    &AlgIdT,&NotBeforeT,&NotAfterT,null),NotZero))).
                    SetProviderInfo(Key);
                }
            }
        #endregion
        #region M:MakeCertificate(CryptKey,String,String,String,DateTime,DateTime,IList<CertificateExtension>):X509Certificate
        private X509Certificate MakeCertificate(CryptKey Key, String SubjectName, String SerialNumber, String AlgId,DateTime NotBefore,DateTime NotAfter,IList<CertificateExtension> Extensions) {
            var SourceCertificate = MakeCertificate(Key,SubjectName,AlgId,NotBefore,NotAfter,Extensions);
            Debug.Print($"SourceCertificate:\n{SourceCertificate.Serialize()}");
            var Builder = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SourceCertificate.Bytes)).First();
            if (!String.IsNullOrWhiteSpace(SerialNumber)) {
                for (var i = 0;i < Builder[0].Count; i++) {
                    if (Builder[0][i] is Asn1Integer) {
                        Builder[0][i] = new Asn1Integer(SerialNumber);
                        break;
                        }
                    }
                using (var o = new MemoryStream()) {
                    Builder[0].WriteTo(o,true);
                    using (var engine = new CryptHashAlgorithm(this, GetAlgId(SourceCertificate.HashAlgorithm))) {
                        o.Seek(0,SeekOrigin.Begin);
                        engine.Compute(o);
                        engine.SignHash(SourceCertificate.KeySpec,out var digest,out var signature);
                        Builder[2]=new Asn1BitString(0,signature.Reverse().ToArray());
                        }
                    }
                using (var o = new MemoryStream()) {
                    Builder.WriteTo(o,true);
                    return (new X509Certificate(o.ToArray())).SetProviderInfo(Key);
                    }
                }
            return SourceCertificate;
            }
        #endregion
        #region M:MakeCertificate(ALG_ID,String,String,DateTime,DateTime,IList<CertificateExtension>,Stream,SecureString,{out}X509Certificate,Boolean)
        private const Int32 REPORT_NO_PRIVATE_KEY                   = 0x0001;
        private const Int32 REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY   = 0x0002;
        private const Int32 EXPORT_PRIVATE_KEYS                     = 0x0004;
        private const Int32 PKCS12_INCLUDE_EXTENDED_PROPERTIES      = 0x0010;
        private const Int32 PKCS12_PROTECT_TO_DOMAIN_SIDS           = 0x0020;
        private const Int32 PKCS12_EXPORT_SILENT                    = 0x0040;
        private const Int32 PKCS12_EXPORT_PBES2_PARAMS              = 0x0080; 
        private const Int32 PKCS12_DISABLE_ENCRYPT_CERTIFICATES     = 0x0100;
        private const Int32 PKCS12_ENCRYPT_CERTIFICATES             = 0x0200;
        private const Int32 PKCS12_EXPORT_ECC_CURVE_PARAMETERS      = 0x1000;
        private const Int32 PKCS12_EXPORT_ECC_CURVE_OID             = 0x2000;
        #if FEATURE_SECURE_STRING_PASSWORD
        public static unsafe void MakeCertificate(ALG_ID AlgId,String SubjectName,String SerialNumber,
            DateTime NotBefore,DateTime NotAfter,IList<CertificateExtension> Extensions,
            Stream PrivateKeyStream, SecureString SecureCode,out X509Certificate Certificate,
            Boolean DeletePrivateKey, out String Container, out String ProviderName, out CRYPT_PROVIDER_TYPE ProviderType)
        #else
        public static unsafe void MakeCertificate(ALG_ID AlgId,String SubjectName,String SerialNumber,
            DateTime NotBefore,DateTime NotAfter,IList<CertificateExtension> Extensions,
            Stream PrivateKeyStream, Stream OutputKeyStream, String SecureCode,out X509Certificate Certificate,
            Boolean DeletePrivateKey, out String Container, out String ProviderName, out CRYPT_PROVIDER_TYPE ProviderType)
        #endif
            {
            if (SubjectName == null) { throw new ArgumentNullException(nameof(SubjectName)); }
            Certificate = null;
            ProviderType = ProviderTypeFromAlgId(AlgId);
            if (ProviderType == CRYPT_PROVIDER_TYPE.AUTO) { throw new NotSupportedException(); }
            switch (ProviderType) {
                case CRYPT_PROVIDER_TYPE.PROV_GOST_2001_DH:
                case CRYPT_PROVIDER_TYPE.PROV_GOST_2012_256:
                case CRYPT_PROVIDER_TYPE.PROV_GOST_2012_512:
                    {
                    var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
                    using (var contextS = AcquireContext(ProviderType, CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                        ProviderName = contextS.ProviderName;
                        }
                    Container = ((ProviderName != null) && (ProviderName.StartsWith("infotecs", StringComparison.OrdinalIgnoreCase)))
                            ? $@"{SubjectName}"
                            : $@"\\.\REGISTRY\{Guid.NewGuid().ToString("D").ToLowerInvariant()}";
                    using (var contextS = AcquireContext(ProviderType, Container,CryptographicContextFlags.CRYPT_NEWKEYSET)) {
                        contextS.SecureCode = SecureCode;
                        var flags = CryptGenKeyFlags.CRYPT_EXPORTABLE;
                        String algid;
                        switch (AlgId) {
                            case ALG_ID.CALG_GR3410_12_256: algid = ObjectIdentifiers.szOID_CP_GOST_R3411_12_256_R3410; break;
                            case ALG_ID.CALG_GR3410_12_512: algid = ObjectIdentifiers.szOID_CP_GOST_R3411_12_512_R3410; break;
                            case ALG_ID.CALG_GR3410EL:      algid = ObjectIdentifiers.szOID_CP_GOST_R3411_R3410EL;      break;
                            default: throw new InvalidOperationException();
                            }
                        using (var key = CryptKey.GenKey(contextS, ALG_ID.AT_KEYEXCHANGE, flags)) {
                            Certificate = contextS.MakeCertificate(key,SubjectName,SerialNumber,algid,NotBefore,NotAfter,Extensions);
                            key.Certificate = Certificate;
                            #region PFX
                            if (PrivateKeyStream != null) {
                                using (var manager = new LocalMemoryManager())
                                using (var store = new X509CertificateStorage(X509StoreName.Memory)) {
                                    store.Add(Certificate);
                                    var pfx = new CRYPT_DATA_BLOB();
                                    #if FEATURE_SECURE_STRING_PASSWORD
                                    var psw = Marshal.SecureStringToGlobalAllocUnicode(SecureCode);
                                    #else
                                    var psw = (IntPtr)manager.StringToMem(SecureCode,Encoding.Unicode);
                                    #endif
                                    try
                                        {
                                        Validate(entries.CertExportCertStore(store.Handle,ref pfx,psw,EXPORT_PRIVATE_KEYS|REPORT_NO_PRIVATE_KEY|REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY|PKCS12_INCLUDE_EXTENDED_PROPERTIES));
                                        pfx.Data = (Byte*)manager.Alloc(pfx.Size);
                                        Validate(entries.CertExportCertStore(store.Handle,ref pfx,psw,EXPORT_PRIVATE_KEYS|REPORT_NO_PRIVATE_KEY|REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY|PKCS12_INCLUDE_EXTENDED_PROPERTIES));
                                        PrivateKeyStream.Write(pfx.Data,pfx.Size);
                                        }
                                    finally
                                        {
                                        #if FEATURE_SECURE_STRING_PASSWORD
                                        Marshal.ZeroFreeGlobalAllocUnicode(psw);
                                        #endif
                                        }
                                    }
                                }
                            #endregion
                            if (OutputKeyStream != null) {
                                key.ExportPrivateKey(OutputKeyStream,SecureCode);
                                }
                            }
                        }
                    if (DeletePrivateKey && (PrivateKeyStream != null)) {
                        Validate(entries.CryptAcquireContext(out var r, Container, null,
                            (Int32)ProviderType,(Int32)CryptographicContextFlags.CRYPT_DELETEKEYSET));
                        }
                    }
                    break;
                default: throw new NotSupportedException();
                }
            }
        #endregion
        #region M:MakeCertificate(ALG_ID,String,String,DateTime,DateTime,IList<CertificateExtension>,Stream,SecureString,{out}X509Certificate,X509Certificate,Boolean)
        #if FEATURE_SECURE_STRING_PASSWORD
        public static unsafe void MakeCertificate(ALG_ID AlgId,String SubjectName,String SerialNumber,
            DateTime NotBefore,DateTime NotAfter,IList<CertificateExtension> Extensions,
            Stream PrivateKeyStream, SecureString SecureCode,
            out X509Certificate Certificate,X509Certificate IssuerCertificate,
            Boolean DeletePrivateKey, out String Container, out String ProviderName, out CRYPT_PROVIDER_TYPE ProviderType)
        #else
        public static unsafe void MakeCertificate(ALG_ID AlgId,String SubjectName,String SerialNumber,
            DateTime NotBefore,DateTime NotAfter,IList<CertificateExtension> Extensions,
            Stream PrivateKeyStream, Stream OutputKeyStream, String SecureCode,
            out X509Certificate Certificate,X509Certificate IssuerCertificate,
            Boolean DeletePrivateKey, out String Container, out String ProviderName, out CRYPT_PROVIDER_TYPE ProviderType)
        #endif
            {
            if (SubjectName == null) { throw new ArgumentNullException(nameof(SubjectName)); }
            if (IssuerCertificate == null) { throw new ArgumentNullException(nameof(IssuerCertificate)); }
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            ProviderType = ProviderTypeFromAlgId(AlgId);
            if (ProviderType == CRYPT_PROVIDER_TYPE.AUTO) { throw new NotSupportedException(); }
            Certificate = null;
            var extensions = new List<CertificateExtension>(Extensions ?? EmptyArray<CertificateExtension>.Value);
            if (!extensions.Any(i => i is CertificateAuthorityKeyIdentifier)) {
                var SKI = IssuerCertificate.Source.Extensions.
                    OfType<CertificateSubjectKeyIdentifier>().
                    FirstOrDefault();
                if (SKI != null) {
                    extensions.Add(new CertificateAuthorityKeyIdentifier(IssuerCertificate.Source));
                    }
                }
            MakeCertificate(AlgId,SubjectName,SerialNumber,NotBefore,NotAfter,extensions,null,null,SecureCode,out var SubjectCertificate,false,out Container, out ProviderName, out ProviderType);
            var Builder = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SubjectCertificate.Bytes)).First();
            Builder[0][SubjectCertificate.Source.IssuerFieldIndex] = IssuerCertificate.Source.Subject.BuildSequence();
            using (var context = AcquireContext(ProviderType,IssuerCertificate.Container,CryptographicContextFlags.CRYPT_SILENT)) {
                context.SecureCode = SecureCode;
                using (var o = new MemoryStream()) {
                    Builder[0].WriteTo(o,true);
                    using (var engine = new CryptHashAlgorithm(context, IssuerCertificate.HashAlgorithm)) {
                        o.Seek(0,SeekOrigin.Begin);
                        engine.Compute(o);
                        engine.SignHash(IssuerCertificate.KeySpec,out var digest,out var signature);
                        Builder[2]=new Asn1BitString(0,signature.Reverse().ToArray());
                        }
                    }
                using (var o = new MemoryStream()) {
                    Builder.WriteTo(o,true);
                    Certificate = (new X509Certificate(o.ToArray())).SetProviderInfo(SubjectCertificate);
                    }
                }
            var containerT = Certificate.Container;
            using (var context = AcquireContext(ProviderType,Certificate.Container,CryptographicContextFlags.CRYPT_SILENT|CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                var key = context.Keys.FirstOrDefault(i => i.Container == containerT);
                if (key != null) {
                    key.Context.SecureCode = SecureCode;
                    key.Certificate = Certificate;
                    if (OutputKeyStream != null) {
                        key.ExportPrivateKey(OutputKeyStream,SecureCode);
                        }
                    }
                }
            if (PrivateKeyStream != null) {
                using (var manager = new LocalMemoryManager())
                using (var store = new X509CertificateStorage(X509StoreName.Memory)) {
                    Debug.Print($"Certificate:\n{Certificate.Serialize()}");
                    store.Add(Certificate);
                    var pfx = new CRYPT_DATA_BLOB();
                    #if FEATURE_SECURE_STRING_PASSWORD
                    var psw = Marshal.SecureStringToGlobalAllocUnicode(SecureCode);
                    #else
                    var psw = (IntPtr)manager.StringToMem(SecureCode,Encoding.Unicode);
                    #endif
                    try
                        {
                        Validate(entries.CertExportCertStore(store.Handle,ref pfx,psw,EXPORT_PRIVATE_KEYS|REPORT_NO_PRIVATE_KEY|REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY|PKCS12_INCLUDE_EXTENDED_PROPERTIES));
                        pfx.Data = (Byte*)manager.Alloc(pfx.Size);
                        Validate(entries.CertExportCertStore(store.Handle,ref pfx,psw,EXPORT_PRIVATE_KEYS|REPORT_NO_PRIVATE_KEY|REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY|PKCS12_INCLUDE_EXTENDED_PROPERTIES));
                        PrivateKeyStream.Write(pfx.Data,pfx.Size);
                        }
                    finally
                        {
                        #if FEATURE_SECURE_STRING_PASSWORD
                        Marshal.ZeroFreeGlobalAllocUnicode(psw);
                        #endif
                        }
                    }
                if (DeletePrivateKey) {
                    Validate(entries.CryptAcquireContext(out var r, Container, null,
                        (Int32)ProviderType,(Int32)CryptographicContextFlags.CRYPT_DELETEKEYSET));
                    }
                }
            }
        #endregion

        #region M:ProviderTypeFromAlgId(ALG_ID):CRYPT_PROVIDER_TYPE
        public static CRYPT_PROVIDER_TYPE ProviderTypeFromAlgId(ALG_ID AlgId) {
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            var RegisteredProviders = CryptographicContext.RegisteredProviders.ToArray();
            foreach (var type in RegisteredProviders) {
                if (entries.CryptAcquireContext(out var r, null, type.ProviderName, (Int32)type.ProviderType, (Int32)CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                    using (var context = new CryptographicContextI(r)) {
                        foreach (var alg in context.SupportedAlgorithms) {
                            if (alg.Key == AlgId) {
                                return type.ProviderType;
                                }
                            }
                        }
                    }
                }
            return CRYPT_PROVIDER_TYPE.AUTO;
            }
        #endregion
        #region M:ProviderTypeFromAlgId(Oid):CRYPT_PROVIDER_TYPE
        public static CRYPT_PROVIDER_TYPE ProviderTypeFromAlgId(Oid AlgId) {
            EnsureAlgIdCache();
            if (!SAlgId.TryGetValue(AlgId.Value,out var AlgIdI)) { AlgIdI = OidToAlgId(AlgId); }
            var entries = (CryptographicFunctions)DefaultContext.GetService(typeof(CryptographicFunctions));
            foreach (var type in RegisteredProviders) {
                if (entries.CryptAcquireContext(out var r, null, type.ProviderName, (Int32)type.ProviderType, (Int32)CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                    using (var context = new CryptographicContextI(r)) {
                        foreach (var alg in context.SupportedAlgorithms) {
                            if (alg.Key == AlgIdI) {
                                return type.ProviderType;
                                }
                            }
                        }
                    }
                }
            return CRYPT_PROVIDER_TYPE.AUTO;
            }
        #endregion

        private const Int32 PLAINTEXTKEYBLOB = 0x8;
        private const Int32 SALT_LENGTH = 16;
        private const Int32 HP_PBKDF2_SALT = 0x0017;
        }
    }