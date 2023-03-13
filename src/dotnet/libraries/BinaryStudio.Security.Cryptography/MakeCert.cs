using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Internal;

namespace BinaryStudio.Security.Cryptography
    {
    using CRYPT_DATA_BLOB = CRYPT_BLOB;
    public partial class CryptographicContext
        {
        #region M:MakeCert(ALG_ID,String,String,DateTime,DateTime,IList<Asn1CertificateExtension>,Stream,SecureString,{out}X509Certificate,Boolean)
        private const Int32 EXPORT_PRIVATE_KEYS = 0x0004;
        public static unsafe void MakeCert(ALG_ID AlgId,String SubjectName,String SerialNumber,
            DateTime NotBefore,DateTime NotAfter,IList<Asn1CertificateExtension> Extensions,
            Stream PrivateKeyStream, SecureString SecureString,out X509Certificate Certificate,
            Boolean DeletePrivateKey)
            {
            if (SubjectName == null) { throw new ArgumentNullException(nameof(SubjectName)); }
            Certificate = null;
            var ProviderType = ProviderTypeFromAlgId(AlgId);
            if (ProviderType == CRYPT_PROVIDER_TYPE.AUTO) { throw new NotSupportedException(); }
            switch (ProviderType) {
                case CRYPT_PROVIDER_TYPE.PROV_GOST_2001_DH:
                case CRYPT_PROVIDER_TYPE.PROV_GOST_2012_256:
                case CRYPT_PROVIDER_TYPE.PROV_GOST_2012_512:
                    {
                    var entries = (ICryptoAPI)DefaultContext.GetService(typeof(ICryptoAPI));
                    var container = $@"\\.\REGISTRY\{Guid.NewGuid().ToString("D").ToLowerInvariant()}";
                    using (var contextS = AcquireContext(ProviderType, container,CryptographicContextFlags.CRYPT_NEWKEYSET)) {
                        contextS.SecureCode = SecureString;
                        var flags = (PrivateKeyStream != null) ? CryptGenKeyFlags.CRYPT_EXPORTABLE : CryptGenKeyFlags.CRYPT_NONE;
                        String algid;
                        switch (AlgId) {
                            case ALG_ID.CALG_GR3410_12_256: algid = ObjectIdentifiers.szOID_CP_GOST_R3411_12_256_R3410; break;
                            case ALG_ID.CALG_GR3410_12_512: algid = ObjectIdentifiers.szOID_CP_GOST_R3411_12_512_R3410; break;
                            case ALG_ID.CALG_GR3410EL:      algid = ObjectIdentifiers.szOID_CP_GOST_R3411_R3410EL;      break;
                            default: throw new InvalidOperationException();
                            }
                        using (var key = CryptKey.GenKey(contextS, ALG_ID.AT_SIGNATURE, flags)) {
                            Certificate = contextS.MakeCert(key,SubjectName,SerialNumber,algid,NotBefore,NotAfter,Extensions);
                            key.Certificate = Certificate;
                            if (PrivateKeyStream != null) {
                                using (var manager = new LocalMemoryManager())
                                using (var store = new X509CertificateStorage(X509StoreName.Memory)) {
                                    store.Add(Certificate);
                                    var pfx = new CRYPT_DATA_BLOB();
                                    var psw = Marshal.SecureStringToGlobalAllocUnicode(SecureString);
                                    try
                                        {
                                        Validate(entries.CertExportCertStore(store.Handle,ref pfx,psw,IntPtr.Zero,EXPORT_PRIVATE_KEYS));
                                        pfx.Data = (Byte*)manager.Alloc(pfx.Size);
                                        Validate(entries.CertExportCertStore(store.Handle,ref pfx,psw,IntPtr.Zero,EXPORT_PRIVATE_KEYS));
                                        PrivateKeyStream.Write(pfx.Data,pfx.Size);
                                        }
                                    finally
                                        {
                                        Marshal.ZeroFreeGlobalAllocUnicode(psw);
                                        }
                                    }
                                }
                            }
                        }
                    if (DeletePrivateKey) {
                        Validate(entries.CryptAcquireContext(out var r, container, null,
                            (Int32)ProviderType,(Int32)CryptographicContextFlags.CRYPT_DELETEKEYSET));
                        }
                    }
                    break;
                default: throw new NotSupportedException();
                }
            }
        #endregion
        #region M:MakeCert(CryptKey,String,String,DateTime,DateTime,IList<Asn1CertificateExtension>):X509Certificate
        private unsafe X509Certificate MakeCert(CryptKey Key, String SubjectName, String AlgId,DateTime NotBefore,DateTime NotAfter,IList<Asn1CertificateExtension> Extensions) {
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
                                Extensions[i].Body[0].WriteTo(MemoryStream);
                                var block = MemoryStream.ToArray();
                                File.WriteAllBytes("x.bin",block);
                                ExtensionsA[i].Value.Size = block.Length;
                                ExtensionsA[i].Value.Data = (Byte*)manager.Alloc(block);
                                }
                            }
                        return new X509Certificate(Validate(entries.CertCreateSelfSignCertificate(
                            Handle,ref SubjectIssuerBlob,0,null,&AlgIdT,
                            &NotBeforeT,&NotAfterT,&ExtensionsE),
                            NotZero))
                            {
                            KeySpec = Key.KeySpec,
                            Container = Key.Container
                            };
                        }
                    }
                return new X509Certificate(Validate(entries.CertCreateSelfSignCertificate(
                    Handle,ref SubjectIssuerBlob,0,null,
                    &AlgIdT,&NotBeforeT,&NotAfterT,null),NotZero))
                    {
                    KeySpec = Key.KeySpec,
                    Container = Key.Container
                    };
                }
            }
        #endregion
        #region M:MakeCert(CryptKey,String,String,String,DateTime,DateTime,IList<Asn1CertificateExtension>):X509Certificate
        private X509Certificate MakeCert(CryptKey Key, String SubjectName, String SerialNumber, String AlgId,DateTime NotBefore,DateTime NotAfter,IList<Asn1CertificateExtension> Extensions) {
            var SourceCertificate = MakeCert(Key,SubjectName,AlgId,NotBefore,NotAfter,Extensions);
            var Container = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SourceCertificate.Bytes)).First();
            if (!String.IsNullOrWhiteSpace(SerialNumber)) {
                for (var i = 0;i < Container[0].Count; i++) {
                    if (Container[0][i] is Asn1Integer) {
                        Container[0][i] = new Asn1Integer(SerialNumber);
                        break;
                        }
                    }
                using (var o = new MemoryStream()) {
                    Container[0].WriteTo(o,true);
                    using (var engine = new CryptHashAlgorithm(this, GetAlgId(SourceCertificate.HashAlgorithm))) {
                        o.Seek(0,SeekOrigin.Begin);
                        engine.Compute(o);
                        engine.SignHash(SourceCertificate.KeySpec,out var digest,out var signature);
                        Container[2]=new Asn1BitString(0,signature.Reverse().ToArray());
                        }
                    }
                using (var o = new MemoryStream()) {
                    Container.WriteTo(o,true);
                    return new X509Certificate(o.ToArray()){
                        KeySpec = SourceCertificate.KeySpec,
                        Container = SourceCertificate.Container
                        };
                    }
                }
            return SourceCertificate;
            }
        #endregion

        #region M:ProviderTypeFromAlgId(ALG_ID):CRYPT_PROVIDER_TYPE
        private static CRYPT_PROVIDER_TYPE ProviderTypeFromAlgId(ALG_ID AlgId) {
            var entries = (ICryptoAPI)DefaultContext.GetService(typeof(ICryptoAPI));
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
        }
    }