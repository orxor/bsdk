using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
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

        public unsafe X509Certificate CreateSelfSignCertificate(String name) {
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
                        default: throw new InvalidOperationException();
                        }
                    var FAlgId = new CRYPT_ALGORITHM_IDENTIFIER {
                        ObjectId = (IntPtr)LocalMemoryManager.StringToMem(TAlgId,Encoding.ASCII)
                        };
                    return new X509Certificate(Validate(entries.CertCreateSelfSignCertificate(Handle,ref SubjectIssuerBlob,0,null,&FAlgId,null,null,null),NotZero));
                    }
                return new X509Certificate(Validate(entries.CertCreateSelfSignCertificate(Handle,ref SubjectIssuerBlob,0,null,null,null,null,null),NotZero));
                }
            }
        }
    }