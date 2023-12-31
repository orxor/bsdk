﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Certificates.Builders;
using BinaryStudio.Security.Cryptography.Specific.Fintech;
using X509Certificate = BinaryStudio.Security.Cryptography.Certificates.X509Certificate;

namespace UnitTests.BinaryStudio.Security.Cryptography.Generator
    {
    internal class Program
        {
        private static readonly Random Random = new Random();
        public static readonly Byte[] InputString = Encoding.ASCII.GetBytes("The data that can be hashed and signed.");
        private class RequestSecureCode : RequestSecureString
            {
            #if FEATURE_SECURE_STRING_PASSWORD
            public SecureString SecureCode { get; }
            public RequestSecureCode(SecureString SecureCode)
                {
                this.SecureCode = SecureCode;
                }
            #else
            public String SecureCode { get; }
            public RequestSecureCode(String SecureCode)
                {
                this.SecureCode = SecureCode;
                }
            #endif

            public HRESULT GetSecureString(CryptographicContext Context, RequestSecureStringEventArgs e)
                {
                e.SecureString = SecureCode;
                return HRESULT.S_OK;
                }
            }

        private static void Perform(ALG_ID AlgId)
            {
            try
                {
                var SecureCode = CryptographicContext.GetSecureString("12345678");
                var RequestSecureCode = new RequestSecureCode(SecureCode);
                var dt = DateTime.Now;
                var ProviderType = CRYPT_PROVIDER_TYPE.PROV_GOST_2001_DH;
                switch (AlgId)
                    {
                    case ALG_ID.CALG_GR3410EL: ProviderType = CRYPT_PROVIDER_TYPE.PROV_GOST_2001_DH; break;
                    case ALG_ID.CALG_GR3410_12_256: ProviderType = CRYPT_PROVIDER_TYPE.PROV_GOST_2012_256; break;
                    case ALG_ID.CALG_GR3410_12_512: ProviderType = CRYPT_PROVIDER_TYPE.PROV_GOST_2012_512; break;
                    }
                var Folder = $"{{{AlgId}}}";
                if (Directory.Exists(Folder)) { Directory.Delete(Folder); }
                Directory.CreateDirectory(Folder);
                DoSet(Folder,AlgId,dt,SecureCode,"R-CA",new Byte[]{ 1,2,3}, X509KeyUsageFlags.CrlSign|X509KeyUsageFlags.KeyCertSign|X509KeyUsageFlags.DigitalSignature|X509KeyUsageFlags.NonRepudiation, out var RootCertificate);
                DoSet(Folder,AlgId,dt,SecureCode,"I-CA",new Byte[]{ 4,5,6}, RootCertificate, X509KeyUsageFlags.CrlSign|X509KeyUsageFlags.KeyCertSign|X509KeyUsageFlags.DigitalSignature, false,
                    new Uri[]{ new Uri("http://localhost/R-CA.crl"),  },
                    out var IntermediateCertificate);
                DoSet(Folder,AlgId,dt,SecureCode,"User1",new Byte[]{  7, 8, 9}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature|X509KeyUsageFlags.DataEncipherment|X509KeyUsageFlags.KeyAgreement|X509KeyUsageFlags.KeyEncipherment, true, new Uri[]{ new Uri("http://localhost/I-CA.crl"),  }, out var User1);
                DoSet(Folder,AlgId,dt,SecureCode,"User2",new Byte[]{ 10,11,12}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature|X509KeyUsageFlags.DataEncipherment|X509KeyUsageFlags.KeyAgreement|X509KeyUsageFlags.KeyEncipherment, true, new Uri[]{ new Uri("http://localhost/I-CA.crl"),  }, out var User2);
                DoSet(Folder,AlgId,dt,SecureCode,"User3",new Byte[]{ 13,14,15}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature|X509KeyUsageFlags.DataEncipherment|X509KeyUsageFlags.KeyAgreement|X509KeyUsageFlags.KeyEncipherment, true, new Uri[]{ new Uri("http://localhost/I-CA.crl"),  }, out var User3);
                DoCRLSet(Folder,AlgId,dt,RequestSecureCode,RootCertificate);
                DoCRLSet(Folder,AlgId,dt,RequestSecureCode,IntermediateCertificate);
                MakeCRL(Folder,AlgId,dt,04,RequestSecureCode,RootCertificate,"R-CA{Revoked}.crl",IntermediateCertificate);
                MakeCRL(Folder,AlgId,dt,05,RequestSecureCode,IntermediateCertificate,"I-CA{Revoked}.crl",User1,User3);
                DoCmsSet(Folder,ProviderType,RequestSecureCode,CryptographicMessageFlags.Attached,new []{
                    User1,
                    User2,
                    User3
                    });
                DoCmsSet(Folder ,ProviderType,RequestSecureCode,CryptographicMessageFlags.Detached,new []{
                    User1,
                    User2,
                    User3
                    });
                DoFTSet(Folder,ProviderType,RequestSecureCode,CryptographicMessageFlags.Attached,new []{
                    User1,
                    User2,
                    User3
                    });
                DoFTSet(Folder ,ProviderType,RequestSecureCode,CryptographicMessageFlags.Detached,new []{
                    User1,
                    User2,
                    User3
                    });
                DoInvalidSignature(Folder,"{User1,IncludeSigningCertificate}.p7a","{User1,IncludeSigningCertificate,InvalidMessageSignature}.p7a");
                DoInvalidSignature(Folder,"{User1,IncludeSigningCertificate}.p7d","{User1,IncludeSigningCertificate,InvalidMessageSignature}.p7d");
                DoInvalidSignature(Folder,"{User1}.p7a","{User1,InvalidMessageSignature}.p7a");
                DoInvalidSignature(Folder,"{User1}.p7d","{User1,InvalidMessageSignature}.p7d");
                DoInvalidSignature(Folder,"{User1}.fta","{User1,InvalidMessageSignature}.fta", 16, 8);
                DoInvalidSignature(Folder,"{User1}.ftd","{User1,InvalidMessageSignature}.ftd", 16, 8);
                DoEncSet(Folder,ProviderType,new Oid(ObjectIdentifiers.szOID_CP_GOST_28147),"CP_GOST_28147",
                    new []{
                        User1,
                        User2,
                        User3
                        });
                DoEncSet(Folder,ProviderType,new Oid(ObjectIdentifiers.szOID_CP_GOST_R3412_2015_M),"CP_GOST_R3412_2015_M",
                    new []{
                        User1,
                        User2,
                        User3
                        });
                DoEncSet(Folder,ProviderType,new Oid(ObjectIdentifiers.szOID_CP_GOST_R3412_2015_K),"CP_GOST_R3412_2015_K",
                    new []{
                        User1,
                        User2,
                        User3
                        });
                var ProviderName = String.Empty;
                using (var contextS = CryptographicContext.AcquireContext(ProviderType, CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                    ProviderName = contextS.ProviderName;
                    }
                var IsViPNet = ProviderName.StartsWith("infotecs", StringComparison.OrdinalIgnoreCase);
                if (IsViPNet) {
                    var ContainerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),$@"Infotecs\Containers");
                    if (Directory.Exists(ContainerPath)) {
                        File.Delete(Path.Combine(ContainerPath,"CN=I-CA"));
                        File.Delete(Path.Combine(ContainerPath,"CN=R-CA"));
                        File.Delete(Path.Combine(ContainerPath,"CN=User1"));
                        File.Delete(Path.Combine(ContainerPath,"CN=User2"));
                        File.Delete(Path.Combine(ContainerPath,"CN=User3"));
                        }
                    }
                }
            catch (Exception e)
                {
                Console.WriteLine(Exceptions.ToString(e));
                }
            }

        private static void Main(String[] args)
            {
            //Console.WriteLine("Press [ENTER] to continue...");
            //Console.ReadLine();
            Perform(ALG_ID.CALG_GR3410EL);
            Perform(ALG_ID.CALG_GR3410_12_256);
            Perform(ALG_ID.CALG_GR3410_12_512);
            }

        private static void DoEncSet(String Folder,CRYPT_PROVIDER_TYPE ProviderType,Oid AlgId, String Prefix, X509Certificate[] certificates) {
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,IncludeSigningCertificate}}{{{Prefix}}}.enc"))) {
                    context.EncryptMessage(new MemoryStream(InputString),output,new List<X509Certificate>{
                        certificates[0]
                        },
                        AlgId,
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation|
                        CryptographicMessageFlags.Split);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,User2,User3,IncludeSigningCertificate}}{{{Prefix}}}.enc"))) {
                    context.EncryptMessage(new MemoryStream(InputString),output,new List<X509Certificate>{
                        certificates[0],
                        certificates[1],
                        certificates[2]
                        },
                        AlgId,
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation|
                        CryptographicMessageFlags.Split);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1}}{{{Prefix}}}.enc"))) {
                    context.EncryptMessage(new MemoryStream(InputString),output,new List<X509Certificate>{
                        certificates[0]
                        },
                        AlgId,
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation|
                        CryptographicMessageFlags.Split);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,User2,User3}}{{{Prefix}}}.enc"))) {
                    context.EncryptMessage(new MemoryStream(InputString),output,new List<X509Certificate>{
                        certificates[0],
                        certificates[1],
                        certificates[2]
                        },
                        AlgId,
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation|
                        CryptographicMessageFlags.Split);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,IncludeSigningCertificate,InvalidSignature}}{{{Prefix}}}.enc"))) {
                    context.EncryptMessage(new MemoryStream(InputString),output,new List<X509Certificate>{
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{InvalidSignature}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            }
                        },
                        AlgId,
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation|
                        CryptographicMessageFlags.Split);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,IncludeSigningCertificate,Expired}}{{{Prefix}}}.enc"))) {
                    context.EncryptMessage(new MemoryStream(InputString),output,new List<X509Certificate>{
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{Expired}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            }
                        },
                        AlgId,
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation|
                        CryptographicMessageFlags.Split);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,IncludeSigningCertificate,Future}}{{{Prefix}}}.enc"))) {
                    context.EncryptMessage(new MemoryStream(InputString),output,new List<X509Certificate>{
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{Future}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            }
                        },
                        AlgId,
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation|
                        CryptographicMessageFlags.Split);
                    }
                }
            DoInvalidSignature(Folder,$"{{User1,IncludeSigningCertificate}}{{{Prefix}}}.enc",$"{{User1,IncludeSigningCertificate}}{{{Prefix}}}{{InvalidMessage}}.enc", 80);
            DoInvalidSignature(Folder,$"{{User1}}{{{Prefix}}}.enc",$"{{User1}}{{{Prefix}}}{{InvalidMessage}}.enc", 80);
            }

        private static void DoFTSet(String Folder,CRYPT_PROVIDER_TYPE ProviderType, RequestSecureString RequestSecureCode,CryptographicMessageFlags flags, X509Certificate[] certificates)
            {
            var e = flags.HasFlag(CryptographicMessageFlags.Attached)
                ? "fta"
                : "ftd";
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var provider = new FintechMessageProvider(context))
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1}}.{e}"))) {
                    provider.CreateMessageSignature(new MemoryStream(InputString),output,
                        certificates[0],
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var provider = new FintechMessageProvider(context))
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,InvalidSignature}}.{e}"))) {
                    provider.CreateMessageSignature(new MemoryStream(InputString),output,
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{InvalidSignature}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            },
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var provider = new FintechMessageProvider(context))
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,Expired}}.{e}"))) {
                    provider.CreateMessageSignature(new MemoryStream(InputString),output,
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{Expired}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            },
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var provider = new FintechMessageProvider(context))
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,Future}}.{e}"))) {
                    provider.CreateMessageSignature(new MemoryStream(InputString),output,
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{Future}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            },
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            }

        private static void DoCmsSet(String Folder,CRYPT_PROVIDER_TYPE ProviderType, RequestSecureString RequestSecureCode,CryptographicMessageFlags flags, X509Certificate[] certificates)
            {
            var e = flags.HasFlag(CryptographicMessageFlags.Attached)
                ? "p7a"
                : "p7d";
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,IncludeSigningCertificate}}.{e}"))) {
                    context.CreateMessageSignature(new MemoryStream(InputString),output,new List<X509Certificate>{
                        certificates[0]
                        },
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,IncludeSigningCertificate,InvalidSignature}}.{e}"))) {
                    context.CreateMessageSignature(new MemoryStream(InputString),output,new List<X509Certificate>{
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{InvalidSignature}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            }
                        },
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,IncludeSigningCertificate,Expired}}.{e}"))) {
                    context.CreateMessageSignature(new MemoryStream(InputString),output,new List<X509Certificate>{
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{Expired}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            }
                        },
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,IncludeSigningCertificate,Future}}.{e}"))) {
                    context.CreateMessageSignature(new MemoryStream(InputString),output,new List<X509Certificate>{
                        new X509Certificate(File.ReadAllBytes(Path.Combine(Folder,"User1{Future}.cer"))){
                            Container = certificates[0].Container,
                            KeySpec = certificates[0].KeySpec
                            }
                        },
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,User2,User3,IncludeSigningCertificate}}.{e}"))) {
                    context.CreateMessageSignature(new MemoryStream(InputString),output,new List<X509Certificate>{
                        certificates[0],
                        certificates[1],
                        certificates[2]
                        },
                        flags|
                        CryptographicMessageFlags.IncludeSigningCertificate|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1}}.{e}"))) {
                    context.CreateMessageSignature(new MemoryStream(InputString),output,new List<X509Certificate>{
                        certificates[0]
                        },
                        flags|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            using (var context = CryptographicContext.AcquireContext(ProviderType,CryptographicContextFlags.CRYPT_VERIFYCONTEXT|CryptographicContextFlags.CRYPT_SILENT)) {
                using (var output = File.OpenWrite(Path.Combine(Folder,$"{{User1,User2,User3}}.{e}"))) {
                    context.CreateMessageSignature(new MemoryStream(InputString),output,new List<X509Certificate>{
                        certificates[0],
                        certificates[1],
                        certificates[2]
                        },
                        flags|
                        CryptographicMessageFlags.IndefiniteLength|
                        CryptographicMessageFlags.SkipCertificateValidation,RequestSecureCode);
                    }
                }
            }


        #if FEATURE_SECURE_STRING_PASSWORD
        private static void UpdateViPNetContainer(String Folder,SecureString SecureCode,String Container,CRYPT_PROVIDER_TYPE ProviderType, Byte[] Certificate,String Marker)
        #else
        private static void UpdateViPNetContainer(String Folder,String SecureCode,String Container,CRYPT_PROVIDER_TYPE ProviderType, Byte[] Certificate,String Marker)
        #endif
            {
            var FullContainerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),$@"Infotecs\Containers\{Container}");
            using (var context = CryptographicContext.AcquireContext(ProviderType,Container,CryptographicContextFlags.CRYPT_NONE)) {
                context.SecureCode = SecureCode;
                var keys = context.Keys.ToArray();
                using (var Key = keys.First(i => (i.Container == Container) || (i.Container.EndsWith(Container)))) {
                    Key.Context.SecureCode = SecureCode;
                    Key.Certificate = new X509Certificate(Certificate);
                    }
                }
            File.Copy(FullContainerPath,Path.Combine(Folder,$"{Marker}.cnt"),true);
            GC.Collect();
            }

        #if FEATURE_SECURE_STRING_PASSWORD
        private static void UpdateViPNetContainer(String Folder,SecureString SecureCode,String Container,CRYPT_PROVIDER_TYPE ProviderType, String Certificate)
        #else
        private static void UpdateViPNetContainer(String Folder,String SecureCode,String Container,CRYPT_PROVIDER_TYPE ProviderType, String Certificate)
        #endif
            {
            UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,
                File.ReadAllBytes(Path.Combine(Folder,Certificate)),
                Path.GetFileNameWithoutExtension(Certificate));
            }

        #if FEATURE_SECURE_STRING_PASSWORD
        private static void DoSet(String Prefix,ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, String SubjectName,Byte[] SerialNumber, X509KeyUsageFlags KeyUsage, out X509Certificate Certificate)
        #else
        private static void DoSet(String Folder,ALG_ID AlgId, DateTime DateTime, String SecureCode, String SubjectName,Byte[] SerialNumber, X509KeyUsageFlags KeyUsage, out X509Certificate Certificate)
        #endif
            {
            var Extensions = new List<CertificateExtension> {
                new Asn1CertificateBasicConstraintsExtension(X509SubjectType.CA),
                new CertificateSubjectKeyIdentifier(false,"89abcdeffedcba98765432100123456789abcdef"),
                new CertificatePoliciesExtension(
                    ObjectIdentifiers.szOID_SIGN_TOOL_KC1,
                    ObjectIdentifiers.szOID_SIGN_TOOL_KC2,
                    ObjectIdentifiers.szOID_SIGN_TOOL_KC3),
                new CertificateCAVersion(new Version(2,0))
                };
            if (KeyUsage != 0) {
                Extensions.Add(new CertificateKeyUsage(KeyUsage));
                }
            using (MemoryStream POutput = new MemoryStream(),
                                BOutput = (AlgId == ALG_ID.CALG_GR3410EL)
                                 ? new MemoryStream()
                                 : null)
                {
                CryptographicContext.MakeCertificate(AlgId,$"CN={SubjectName}",String.Join(String.Empty,SerialNumber.Select(i=> i.ToString("x2"))),
                    DateTime.AddDays(-1),DateTime.AddYears(5),
                    Extensions,POutput, BOutput, SecureCode,
                    out Certificate,false, out var Container, out var ProviderName, out var ProviderType);
                var IsViPNet = ProviderName.StartsWith("infotecs", StringComparison.OrdinalIgnoreCase);
                using (var PrivateKeyOutput = File.Create(Path.Combine(Folder,$"{SubjectName}.pfx"))) {
                    POutput.Seek(0,SeekOrigin.Begin);
                    POutput.CopyTo(PrivateKeyOutput);
                    }
                if (BOutput != null)
                    using (var PrivateKeyOutput = File.Create(Path.Combine(Folder,$"{SubjectName}.pri"))) {
                        BOutput.Seek(0,SeekOrigin.Begin);
                        BOutput.CopyTo(PrivateKeyOutput);
                        }
                if (IsViPNet) {
                    File.WriteAllBytes(Path.Combine(Folder,$"{SubjectName}.cer"), Certificate.Bytes);
                    DoInvalidSignature(Folder,Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                    DoExpired(Folder,AlgId,SecureCode,Certificate,Certificate,-10,$"{SubjectName}{{Expired}}.cer");
                    DoExpired(Folder,AlgId,SecureCode,Certificate,Certificate,+10,$"{SubjectName}{{Future}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{InvalidSignature}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{Expired}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{Future}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}.cer");
                    }
                else
                    {
                    File.WriteAllBytes(Path.Combine(Folder,$"{SubjectName}.cer"), Certificate.Bytes);
                    DoInvalidSignature(Folder,Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                    DoExpired(Folder,AlgId,SecureCode,Certificate,Certificate,-10,$"{SubjectName}{{Expired}}.cer");
                    DoExpired(Folder,AlgId,SecureCode,Certificate,Certificate,+10,$"{SubjectName}{{Future}}.cer");
                    }
                }
            }

        #if FEATURE_SECURE_STRING_PASSWORD
        private static void DoSet(String Folder,ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, String SubjectName,Byte[] SerialNumber, X509Certificate IssuerCertificate,  X509KeyUsageFlags KeyUsage, Boolean IsLeaf, Uri[] DistributionPoints, out X509Certificate Certificate)
        #else
        private static void DoSet(String Folder,ALG_ID AlgId, DateTime DateTime, String SecureCode, String SubjectName,Byte[] SerialNumber, X509Certificate IssuerCertificate,  X509KeyUsageFlags KeyUsage, Boolean IsLeaf, Uri[] DistributionPoints, out X509Certificate Certificate)
        #endif
            {
            var Extensions = new List<CertificateExtension>{
                //new CertificateExtendedKeyUsage(
                //    "1.3.6.1.5.5.7.3.2",
                //    "1.3.6.1.5.5.7.3.4",
                //    "1.3.6.1.4.1.311.10.3.12",
                //    "1.3.6.1.4.1.311.80.1",
                //    "1.3.6.1.4.1.311.10.3.4")
                };
            if (!IsLeaf) {
                Extensions.Add(new CertificateSubjectKeyIdentifier(false,"90abcdeffedcba98765432100123456789abcdef"));
                Extensions.Add(new Asn1CertificateBasicConstraintsExtension(X509SubjectType.CA,0));
                Extensions.Add(new CertificateCAVersion(new Version(2,0)));
                }
            if (KeyUsage != 0) {
                Extensions.Add(new CertificateKeyUsage(KeyUsage));
                Extensions.Add(new CertificatePoliciesExtension(
                    ObjectIdentifiers.szOID_SIGN_TOOL_KC1,
                    ObjectIdentifiers.szOID_SIGN_TOOL_KC2,
                    ObjectIdentifiers.szOID_SIGN_TOOL_KC3));
                }
            if ((DistributionPoints != null) && (DistributionPoints.Length != 0)) {
                Extensions.Add(new CRLDistributionPoints(DistributionPoints.Select(i => new DistributionPoint(i)).ToArray()));
                }
            using (MemoryStream POutput = new MemoryStream(),
                                BOutput = (AlgId == ALG_ID.CALG_GR3410EL)
                                 ? new MemoryStream()
                                 : null)
                {
                CryptographicContext.MakeCertificate(AlgId,$"CN={SubjectName}",String.Join(String.Empty,SerialNumber.Select(i=> i.ToString("x2"))),
                    DateTime.AddDays(-1),DateTime.AddYears(5),
                    Extensions,POutput,BOutput, SecureCode,
                    out Certificate,IssuerCertificate,false, out var Container, out var ProviderName, out var ProviderType);
                var IsViPNet = ProviderName.StartsWith("infotecs", StringComparison.OrdinalIgnoreCase);
                using (var PrivateKeyOutput = File.Create(Path.Combine(Folder,$"{SubjectName}.pfx"))) {
                    POutput.Seek(0,SeekOrigin.Begin);
                    POutput.CopyTo(PrivateKeyOutput);
                    }
                if (BOutput != null)
                    using (var PrivateKeyOutput = File.Create(Path.Combine(Folder,$"{SubjectName}.pri"))) {
                        BOutput.Seek(0,SeekOrigin.Begin);
                        BOutput.CopyTo(PrivateKeyOutput);
                        }
                if (IsViPNet) {
                    File.WriteAllBytes(Path.Combine(Folder,$"{SubjectName}.cer"), Certificate.Bytes);
                    DoInvalidSignature(Folder,Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                    DoExpired(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{Expired}}.cer");
                    DoExpired(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{Future}}.cer");
                    DoExpiredKey(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{ExpiredKey}}.cer");
                    DoExpiredKey(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{FutureKey}}.cer");
                    DoEKU(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,"1.3.6.1.5.5.7.3.1",$"{SubjectName}{{EKU}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{InvalidSignature}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{Expired}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{Future}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{ExpiredKey}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{FutureKey}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}{{EKU}}.cer");
                    UpdateViPNetContainer(Folder,SecureCode,Container,ProviderType,$"{SubjectName}.cer");
                    }
                else
                    {
                    File.WriteAllBytes(Path.Combine(Folder,$"{SubjectName}.cer"), Certificate.Bytes);
                    DoInvalidSignature(Folder,Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                    DoExpired(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{Expired}}.cer");
                    DoExpired(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{Future}}.cer");
                    DoExpiredKey(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{ExpiredKey}}.cer");
                    DoExpiredKey(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{FutureKey}}.cer");
                    DoEKU(Folder,AlgId,SecureCode,Certificate,IssuerCertificate,"1.3.6.1.5.5.7.3.1",$"{SubjectName}{{EKU}}.cer");
                    }
                }
            }

        #region M:DoInvalidSignature(X509Certificate,String,{out}Byte[])
        private static void DoInvalidSignature(String Folder,X509Certificate SourceCertificate, String SerialNumber, out Byte[] Output) {
            Output = null;
            var Builder = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SourceCertificate.Bytes)).First();
            var Signature = Builder[2].Content.ToArray();
            var value = Signature[0];
            Signature[0] = Signature[Signature.Length-1];
            Signature[Signature.Length-1] = value;
            Builder[2] = new Asn1BitString(0,Signature);
            using (var o = new MemoryStream()) {
                Builder.WriteTo(o,true);
                Output = o.ToArray();
                }
            }
        #endregion
        #region M:DoInvalidSignature(String,String)
        private static void DoInvalidSignature(String Folder,String InputFileName, String OutputFileName, Int32 offset = 8, Int32 size = -1) {
            var r = File.ReadAllBytes(Path.Combine(Folder,InputFileName));
            size = (size == -1)
                ? offset
                : size;
            var l = r.Length - offset;
            var buffer = new Byte[size];
            Random.NextBytes(buffer);
            for (var i = 0; i < size; i++) {
                r[l + i] = buffer[i];
                }
            File.WriteAllBytes(Path.Combine(Folder,OutputFileName), r);
            }
        #endregion
        #region M:DoInvalidSignature(X509Certificate,String,String)
        private static void DoInvalidSignature(String Folder,X509Certificate SourceCertificate, String SerialNumber, String FileName) {
            DoInvalidSignature(Folder,SourceCertificate,SerialNumber,out var o);
            File.WriteAllBytes(Path.Combine(Folder,FileName),o);
            }
        #endregion
        #region M:DoInvalidSignature(X509CertificateRevocationList,{out}Byte[])
        private static void DoInvalidSignature(String Folder,X509CertificateRevocationList Source, out Byte[] Output) {
            Output = null;
            var Builder = Asn1Object.Load(new ReadOnlyMemoryMappingStream(Source.Bytes)).First();
            var Signature = Builder[2].Content.ToArray();
            var value = Signature[0];
            Signature[0] = Signature[Signature.Length-1];
            Signature[Signature.Length-1] = value;
            Builder[2] = new Asn1BitString(0,Signature);
            using (var o = new MemoryStream()) {
                Builder.WriteTo(o,true);
                Output = o.ToArray();
                }
            }
        #endregion
        #region M:DoInvalidSignature(X509CertificateRevocationList,String)
        private static void DoInvalidSignature(String Folder,X509CertificateRevocationList Source, String FileName) {
            DoInvalidSignature(Folder,Source,out var o);
            File.WriteAllBytes(Path.Combine(Folder,FileName),o);
            }
        #endregion
        #region M:DoExpired(ALG_ID,SecureString,X509Certificate,X509Certificate,Int32,FileName)
        #if FEATURE_SECURE_STRING_PASSWORD
        private static void DoExpired(String Folder,ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,String FileName)
        #else
        private static void DoExpired(String Folder,ALG_ID AlgId,String SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,String FileName)
        #endif
            {
            DoExpired(Folder,AlgId,SecureCode,SourceCertificate,IssuerCertificate,Year,out var o);
            File.WriteAllBytes(Path.Combine(Folder,FileName),o);
            }
        #endregion
        #region M:DoExpired(ALG_ID,SecureString,X509Certificate,X509Certificate,Int32,{out}Byte[])
        #if FEATURE_SECURE_STRING_PASSWORD
        private static void DoExpired(String Folder,ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,out Byte[] Output)
        #else
        private static void DoExpired(String Folder,ALG_ID AlgId,String SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,out Byte[] Output)
        #endif
            {
            var ProviderType = CryptographicContext.ProviderTypeFromAlgId(AlgId);
            using (var context = CryptographicContext.AcquireContext(ProviderType,IssuerCertificate.Container,CryptographicContextFlags.CRYPT_SILENT)) {
                context.SecureCode = SecureCode;
                var Builder = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SourceCertificate.Bytes)).First();
                Builder[0][SourceCertificate.Source.ValidityFieldIndex][0] = new Asn1GeneralTime(SourceCertificate.NotBefore.AddYears(Year));
                Builder[0][SourceCertificate.Source.ValidityFieldIndex][1] = new Asn1GeneralTime(SourceCertificate.NotAfter.AddYears(Year));
                using (var o = new MemoryStream()) {
                    Builder[0].WriteTo(o,true);
                    using (var engine = new CryptHashAlgorithm(context, CryptographicContext.GetAlgId(IssuerCertificate.HashAlgorithm))) {
                        o.Seek(0,SeekOrigin.Begin);
                        engine.Compute(o);
                        engine.SignHash(IssuerCertificate.KeySpec,out var digest,out var signature);
                        Builder[2]=new Asn1BitString(0,signature.Reverse().ToArray());
                        }
                    }
                using (var o = new MemoryStream()) {
                    Builder.WriteTo(o,true);
                    Output = o.ToArray();
                    }
                }
            }
        #endregion
        #region M:DoExpiredKey(ALG_ID,SecureString,X509Certificate,X509Certificate,Int32,FileName)
        #if FEATURE_SECURE_STRING_PASSWORD
        private static void DoExpiredKey(String Folder,ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,String FileName)
        #else
        private static void DoExpiredKey(String Folder,ALG_ID AlgId,String SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,String FileName)
        #endif
            {
            DoExpiredKey(Folder,AlgId,SecureCode,SourceCertificate,IssuerCertificate,Year,out var o);
            File.WriteAllBytes(Path.Combine(Folder,FileName),o);
            }
        #endregion
        #region M:DoExpiredKey(ALG_ID,SecureString,X509Certificate,X509Certificate,Int32,{out}Byte[])
        #if FEATURE_SECURE_STRING_PASSWORD
        private static void DoExpiredKey(String Folder,ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,out Byte[] Output)
        #else
        private static void DoExpiredKey(String Folder,ALG_ID AlgId,String SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,out Byte[] Output)
        #endif
            {
            var ProviderType = CryptographicContext.ProviderTypeFromAlgId(AlgId);
            using (var context = CryptographicContext.AcquireContext(ProviderType,IssuerCertificate.Container,CryptographicContextFlags.CRYPT_SILENT)) {
                context.SecureCode = SecureCode;
                var Builder = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SourceCertificate.Bytes)).First();
                if (SourceCertificate.Source.ExtensionsFieldIndex != -1) {
                    Builder[0][SourceCertificate.Source.ExtensionsFieldIndex] = new Asn1ContextSpecificObject(3,
                        new Asn1Sequence(new List<Asn1Object>(SourceCertificate.Source.Extensions){
                            new CertificatePrivateKeyUsagePeriodExtension(
                                SourceCertificate.NotBefore.AddYears(Year),
                                SourceCertificate.NotBefore.AddYears(Year).AddYears(1).AddMonths(3))
                            }));
                    }
                else
                    {
                    Builder[0].Add(new Asn1ContextSpecificObject(3,
                        new Asn1Sequence(new List<Asn1Object>{
                            new CertificatePrivateKeyUsagePeriodExtension(
                                SourceCertificate.NotBefore.AddYears(Year),
                                SourceCertificate.NotBefore.AddYears(Year).AddYears(1).AddMonths(3))
                            })));
                    }
                using (var o = new MemoryStream()) {
                    Builder[0].WriteTo(o,true);
                    using (var engine = new CryptHashAlgorithm(context, CryptographicContext.GetAlgId(IssuerCertificate.HashAlgorithm))) {
                        o.Seek(0,SeekOrigin.Begin);
                        engine.Compute(o);
                        engine.SignHash(IssuerCertificate.KeySpec,out var digest,out var signature);
                        Builder[2]=new Asn1BitString(0,signature.Reverse().ToArray());
                        }
                    }
                using (var o = new MemoryStream()) {
                    Builder.WriteTo(o,true);
                    Output = o.ToArray();
                    }
                }
            }
        #endregion
        #region M:DoEKU(ALG_ID,SecureString,X509Certificate,X509Certificate,String,FileName)
        #if FEATURE_SECURE_STRING_PASSWORD
        private static void DoEKU(String Folder,ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,String EKU,String FileName)
        #else
        private static void DoEKU(String Folder,ALG_ID AlgId,String SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,String EKU,String FileName)
        #endif
            {
            DoEKU(Folder,AlgId,SecureCode,SourceCertificate,IssuerCertificate,EKU,out var o);
            File.WriteAllBytes(Path.Combine(Folder,FileName),o);
            }
        #endregion
        #region M:DoEKU(ALG_ID,SecureString,X509Certificate,X509Certificate,String,{out}Byte[])
        #if FEATURE_SECURE_STRING_PASSWORD
        private static void DoEKU(String Folder,ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,String EKU,out Byte[] Output)
        #else
        private static void DoEKU(String Folder,ALG_ID AlgId,String SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,String EKU,out Byte[] Output)
        #endif
            {
            var ProviderType = CryptographicContext.ProviderTypeFromAlgId(AlgId);
            using (var context = CryptographicContext.AcquireContext(ProviderType,IssuerCertificate.Container,CryptographicContextFlags.CRYPT_SILENT)) {
                context.SecureCode = SecureCode;
                var Builder = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SourceCertificate.Bytes)).First();
                if (SourceCertificate.Source.ExtensionsFieldIndex != -1) {
                    Builder[0][SourceCertificate.Source.ExtensionsFieldIndex] = new Asn1ContextSpecificObject(3,
                        new Asn1Sequence(new List<Asn1Object>(SourceCertificate.Source.Extensions){
                            new CertificateExtendedKeyUsage(EKU)
                            }));
                    }
                else
                    {
                    Builder[0].Add(new Asn1ContextSpecificObject(3,
                        new Asn1Sequence(new List<Asn1Object>{
                            new CertificateExtendedKeyUsage(EKU)
                            })));
                    }
                using (var o = new MemoryStream()) {
                    Builder[0].WriteTo(o,true);
                    using (var engine = new CryptHashAlgorithm(context, CryptographicContext.GetAlgId(IssuerCertificate.HashAlgorithm))) {
                        o.Seek(0,SeekOrigin.Begin);
                        engine.Compute(o);
                        engine.SignHash(IssuerCertificate.KeySpec,out var digest,out var signature);
                        Builder[2]=new Asn1BitString(0,signature.Reverse().ToArray());
                        }
                    }
                using (var o = new MemoryStream()) {
                    Builder.WriteTo(o,true);
                    Output = o.ToArray();
                    }
                }
            }
        #endregion
        #region M:MakeCRL(ALG_ID,DateTime,BigInteger,RequestSecureString,X509Certificate,{out}X509CertificateRevocationList,X509Certificate[])
        private static void MakeCRL(String Folder,ALG_ID AlgId, DateTime DateTime, BigInteger Number, RequestSecureString SecureCode, X509Certificate IssuerCertificate, out X509CertificateRevocationList CRL,X509Certificate[] certificates) {
            var Builder = new X509CertificateRevocationListBuilder(DateTime.AddMonths(-1),DateTime.AddMonths(10),Number);
            Builder.Entries.AddRange(certificates.Select(i => new CertificateRevocationListEntry(i.Source, DateTime.AddMonths(-1),X509CrlReason.Superseded)));
            Builder.Build(IssuerCertificate, SecureCode, out CRL);
            }
        #endregion
        #region M:MakeCRL(ALG_ID,DateTime,BigInteger,RequestSecureString,X509Certificate,String,{params}X509Certificate[])
        private static void MakeCRL(String Folder,ALG_ID AlgId, DateTime DateTime, BigInteger Number, RequestSecureString SecureCode, X509Certificate IssuerCertificate, String FileName,params X509Certificate[] certificates) {
            MakeCRL(Folder,AlgId,DateTime,Number,SecureCode,IssuerCertificate,out var r,certificates);
            File.WriteAllBytes(Path.Combine(Folder,FileName),r.Bytes);
            }
        #endregion

        private static void DoCRLSet(String Folder,ALG_ID AlgId, DateTime DateTime, RequestSecureString SecureCode, X509Certificate IssuerCertificate) {
            var IssuerName = IssuerCertificate.Source.Subject[ObjectIdentifiers.szOID_COMMON_NAME];
            MakeCRL(Folder,AlgId,DateTime,1,SecureCode,IssuerCertificate,out var r, new X509Certificate[0]);
            File.WriteAllBytes(Path.Combine(Folder,$"{IssuerName}.crl"),r.Bytes);
            DoInvalidSignature(Folder,r,$"{IssuerName}{{InvalidSignature}}.crl");
            MakeCRL(Folder,AlgId,DateTime.AddYears(-10),2,SecureCode,IssuerCertificate,$"{IssuerName}{{Expired}}.crl");
            MakeCRL(Folder,AlgId,DateTime.AddYears(+10),3,SecureCode,IssuerCertificate,$"{IssuerName}{{Future}}.crl");
            }

        private static void Swap(ref Byte x, ref Byte y)
            {
            var i = x;
            x = y;
            y = i;
            }
        }
    }
