using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using Microsoft.Win32;

namespace BinaryStudio.Security.Cryptography.Specific.CryptoProCSP
    {
    public class CryptoProCSPCryptographicContext : CryptographicObject
        {
        public CryptographicContext Context { get; }
        public override IntPtr Handle { get { return Context.Handle; }}

        public CryptoProCSPCryptographicContext(CryptographicContext context)
            {
            Context = context;
            }

        public IEnumerable<CryptoProCSPRNGSource> RNGSources { get {
            var o = new List<CryptoProCSPRNGSource>();
            #if NET5_0
            return EmptyArray<CryptoProCSPRNGSource>.Value;
            #else
            String ImagePath = null;
            using (var r = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Crypto Pro\Cryptography\CurrentVersion\Provider", false)) {
                if (r != null) {
                    foreach (var ProviderName in r.GetSubKeyNames()) {
                        using (var ProviderKey = r.OpenSubKey(ProviderName,false)) {
                            if (ProviderKey != null) {
                                ImagePath = ProviderKey.GetValue("Image Path") as String;
                                if (!String.IsNullOrEmpty(ImagePath)) {
                                    break;
                                    }
                                }
                            }
                        }
                    }
                }
            if (ImagePath != null) {
                var Folder = Path.GetDirectoryName(ImagePath);
                using (var r = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Crypto Pro\Cryptography\CurrentVersion\Random", false)) {
                    if (r != null) {
                        foreach (var SubKeyName in r.GetSubKeyNames()) {
                            using (var SourceSubKey = r.OpenSubKey(SubKeyName,false)) {
                                using (var DefaultSourceKey = SourceSubKey?.OpenSubKey("Default",false)) {
                                    if (DefaultSourceKey != null) {
                                        var order = (Int32)DefaultSourceKey.GetValue("Level",0);
                                        o.Add(new CryptoProCSPRNGSource(SourceSubKey,Folder,order));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            return o.OrderBy(i => i.Order);
            #endif
            }}

        public X509Certificate CreateSelfSignCertificate(String Name, String SerialNumber,DateTime NotBefore, DateTime NotAfter, IList<Asn1CertificateExtension> Extensions) {
            var SourceCertificate = CreateSelfSignCertificate(Name,NotBefore,NotAfter,Extensions);
            var Container = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SourceCertificate.Bytes)).First();
            for (var i = 0;i < Container[0].Count; i++) {
                if (Container[0][i] is Asn1Integer) {
                    Container[0][i] = new Asn1Integer(SerialNumber);
                    break;
                    }
                }
            using (var o = new MemoryStream()) {
                Container[0].WriteTo(o,true);
                using (var engine = new CryptHashAlgorithm(Context, CryptographicContext.GetAlgId(SourceCertificate.HashAlgorithm))) {
                    o.Seek(0,SeekOrigin.Begin);
                    engine.Compute(o);
                    engine.SignHash(SourceCertificate.KeySpec,out var digest,out var signature);
                    Container[2]=new Asn1BitString(0,signature.Reverse().ToArray());
                    }
                }
            using (var o = new MemoryStream()) {
                Container.WriteTo(o,true);
                File.WriteAllBytes("cer2.cer",o.ToArray());
                File.WriteAllBytes("cer1.cer",SourceCertificate.Bytes);
                }
            return SourceCertificate;
            }

        public unsafe X509Certificate CreateSelfSignCertificate(String name, DateTime notbefore, DateTime notafter, IList<Asn1CertificateExtension> extensions) {
            var entries = (ICryptoAPI)Context.GetService(typeof(ICryptoAPI));
            var SubjectIssuerBlobM = Context.CertStrToName(name);
            fixed (Byte* SubjectIssuerBlobU = SubjectIssuerBlobM) {
                var SubjectIssuerBlob = new CRYPT_BLOB{
                    Size = SubjectIssuerBlobM.Length,
                    Data = SubjectIssuerBlobU
                    };
                var Container = Context.Container;
                var Keys = Context.Keys.Where(i => String.Equals(i.Container,Container)).ToArray();
                if (Keys.Length > 0) {
                    var SAlgId = Keys[0].AlgId;
                    String TAlgId;
                    switch (SAlgId) {
                        case ALG_ID.CALG_GR3410_12_256: TAlgId = ObjectIdentifiers.szOID_CP_GOST_R3411_12_256_R3410; break;
                        case ALG_ID.CALG_GR3410_12_512: TAlgId = ObjectIdentifiers.szOID_CP_GOST_R3411_12_512_R3410; break;
                        case ALG_ID.CALG_GR3410EL: TAlgId = ObjectIdentifiers.szOID_CP_GOST_R3411_R3410EL; break;
                        default: throw new InvalidOperationException();
                        }
                    var FAlgId = new CRYPT_ALGORITHM_IDENTIFIER {
                        ObjectId = (IntPtr)LocalMemoryManager.StringToMem(TAlgId,Encoding.ASCII)
                        };
                    var NotBefore = notbefore.ToSystemTime();
                    var NotAfter  = notafter.ToSystemTime();
                    if (extensions != null) {
                        using (var manager = new LocalMemoryManager()) {
                            var ExtensionsA = (CERT_EXTENSION*)manager.Alloc(extensions.Count*sizeof(CERT_EXTENSION));
                            var ExtensionsE = new CERT_EXTENSIONS {
                                ExtensionCount = extensions.Count,
                                Extensions = ExtensionsA
                                };
                            for (var i = 0; i < extensions.Count; i++) {
                                ExtensionsA[i].pszObjId = (IntPtr)manager.StringToMem(extensions[i].Identifier.Value,Encoding.ASCII);
                                ExtensionsA[i].fCritical = extensions[i].IsCritical;
                                using (var MemoryStream = new MemoryStream()) {
                                    extensions[i].Body[0].WriteTo(MemoryStream);
                                    var block = MemoryStream.ToArray();
                                    File.WriteAllBytes("x.bin",block);
                                    ExtensionsA[i].Value.Size = block.Length;
                                    ExtensionsA[i].Value.Data = (Byte*)manager.Alloc(block);
                                    }
                                }
                            return new X509Certificate(Validate(entries.CertCreateSelfSignCertificate(Handle,ref SubjectIssuerBlob,0,null,&FAlgId,&NotBefore,&NotAfter,&ExtensionsE),NotZero)){
                                KeySpec = Keys[0].KeySpec
                                };
                            }
                        }
                    return new X509Certificate(Validate(entries.CertCreateSelfSignCertificate(Handle,ref SubjectIssuerBlob,0,null,&FAlgId,&NotBefore,&NotAfter,null),NotZero))
                        {
                        KeySpec = Keys[0].KeySpec
                        };
                    }
                return new X509Certificate(Validate(entries.CertCreateSelfSignCertificate(Handle,ref SubjectIssuerBlob,0,null,null,null,null,null),NotZero));
                }
            }
        }
    }