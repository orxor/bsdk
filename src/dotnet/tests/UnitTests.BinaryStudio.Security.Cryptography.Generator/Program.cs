using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Certificates.Builders;
using X509Certificate = BinaryStudio.Security.Cryptography.Certificates.X509Certificate;

namespace UnitTests.BinaryStudio.Security.Cryptography.Generator
    {
    internal class Program
        {
        private class RequestSecureCode : RequestSecureString
            {
            public SecureString SecureCode { get; }
            public RequestSecureCode(SecureString SecureCode)
                {
                this.SecureCode = SecureCode;
                }

            public HRESULT GetSecureString(CryptographicContext Context, RequestSecureStringEventArgs e)
                {
                e.SecureString = SecureCode;
                return HRESULT.S_OK;
                }
            }

        private static void Main(String[] args)
            {
            Console.WriteLine("Press [ENTER] to continue...");
            Console.ReadLine();
            try
                {
                var SecureCode = CryptographicContext.GetSecureString("12345678");
                var RequestSecureCode = new RequestSecureCode(SecureCode);
                var dt = DateTime.Now;
                var AlgId = ALG_ID.CALG_GR3410EL;
                DoSet(AlgId,dt,SecureCode,"R-CA",new Byte[]{ 1,2,3}, X509KeyUsageFlags.CrlSign|X509KeyUsageFlags.KeyCertSign|X509KeyUsageFlags.DigitalSignature|X509KeyUsageFlags.NonRepudiation, out var RootCertificate);
                DoSet(AlgId,dt,SecureCode,"I-CA",new Byte[]{ 4,5,6}, RootCertificate, X509KeyUsageFlags.CrlSign|X509KeyUsageFlags.KeyCertSign|X509KeyUsageFlags.DigitalSignature, false,
                    new Uri[]{ new Uri("http://localhost/R-CA.crl"),  },
                    out var IntermediateCertificate);
                DoSet(AlgId,dt,SecureCode,"User1",new Byte[]{  7, 8, 9}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature, true, new Uri[]{ new Uri("http://localhost/I-CA.crl"),  }, out var User1);
                DoSet(AlgId,dt,SecureCode,"User2",new Byte[]{ 10,11,12}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature, true, new Uri[]{ new Uri("http://localhost/I-CA.crl"),  }, out var User2);
                DoSet(AlgId,dt,SecureCode,"User3",new Byte[]{ 13,14,15}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature, true, new Uri[]{ new Uri("http://localhost/I-CA.crl"),  }, out var User3);
                DoCRLSet(AlgId,dt,RequestSecureCode,RootCertificate);
                DoCRLSet(AlgId,dt,RequestSecureCode,IntermediateCertificate);
                MakeCRL(AlgId,dt,04,RequestSecureCode,RootCertificate,"R-CA{Revoked}.crl",IntermediateCertificate);
                MakeCRL(AlgId,dt,05,RequestSecureCode,IntermediateCertificate,"I-CA{Revoked}.crl",User1,User3);
                }
            catch (Exception e)
                {
                Console.WriteLine(Exceptions.ToString(e));
                }
            }

        private static void UpdateViPNetContainer(SecureString SecureCode,String Container,CRYPT_PROVIDER_TYPE ProviderType, Byte[] Certificate,String Marker) {
            var FullContainerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),$@"Infotecs\Containers\{Container}");
            using (var context = CryptographicContext.AcquireContext(ProviderType,Container,CryptographicContextFlags.CRYPT_NONE)) {
                context.SecureCode = SecureCode;
                using (var Key = context.Keys.First(i => i.Container == Container)) {
                    Key.Context.SecureCode = SecureCode;
                    Key.Certificate = new X509Certificate(Certificate);
                    }
                }
            File.Copy(FullContainerPath,$"{Marker}.cnt",true);
            GC.Collect();
            }

        private static void UpdateViPNetContainer(SecureString SecureCode,String Container,CRYPT_PROVIDER_TYPE ProviderType, String Certificate) {
            UpdateViPNetContainer(SecureCode,Container,ProviderType,
                File.ReadAllBytes(Certificate),
                Path.GetFileNameWithoutExtension(Certificate));
            }

        private static void DoSet(ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, String SubjectName,Byte[] SerialNumber, X509KeyUsageFlags KeyUsage, out X509Certificate Certificate) {
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
            using (var Output = new MemoryStream()) {
                CryptographicContext.MakeCertificate(AlgId,$"CN={SubjectName}",String.Join(String.Empty,SerialNumber.Select(i=> i.ToString("x2"))),
                    DateTime.AddYears(-1),DateTime.AddYears(5),
                    Extensions,Output, SecureCode,
                    out Certificate,false, out var Container, out var ProviderName, out var ProviderType);
                var IsViPNet = ProviderName.StartsWith("infotecs", StringComparison.OrdinalIgnoreCase);
                using (var PrivateKeyOutput = File.Create($"{SubjectName}.pfx")) {
                    Output.Seek(0,SeekOrigin.Begin);
                    Output.CopyTo(PrivateKeyOutput);
                    }
                if (IsViPNet) {
                    File.Delete($"{SubjectName}.pfx");
                    File.WriteAllBytes($"{SubjectName}.cer", Certificate.Bytes);
                    DoInvalidSignature(Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                    DoExpired(AlgId,SecureCode,Certificate,Certificate,-10,$"{SubjectName}{{Expired}}.cer");
                    DoExpired(AlgId,SecureCode,Certificate,Certificate,+10,$"{SubjectName}{{Future}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{InvalidSignature}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{Expired}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{Future}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}.cer");
                    }
                else
                    {
                    File.WriteAllBytes($"{SubjectName}.cer", Certificate.Bytes);
                    DoInvalidSignature(Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                    DoExpired(AlgId,SecureCode,Certificate,Certificate,-10,$"{SubjectName}{{Expired}}.cer");
                    DoExpired(AlgId,SecureCode,Certificate,Certificate,+10,$"{SubjectName}{{Future}}.cer");
                    }
                }
            }

        private static void DoSet(ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, String SubjectName,Byte[] SerialNumber, X509Certificate IssuerCertificate,  X509KeyUsageFlags KeyUsage, Boolean IsLeaf, Uri[] DistributionPoints, out X509Certificate Certificate) {
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
            using (var Output = new MemoryStream()) {
                CryptographicContext.MakeCertificate(AlgId,$"CN={SubjectName}",String.Join(String.Empty,SerialNumber.Select(i=> i.ToString("x2"))),
                    DateTime.AddYears(-1),DateTime.AddYears(5),
                    Extensions,Output, SecureCode,
                    out Certificate,IssuerCertificate,false, out var Container, out var ProviderName, out var ProviderType);
                var IsViPNet = ProviderName.StartsWith("infotecs", StringComparison.OrdinalIgnoreCase);
                using (var PrivateKeyOutput = File.Create($"{SubjectName}.pfx")) {
                    Output.Seek(0,SeekOrigin.Begin);
                    Output.CopyTo(PrivateKeyOutput);
                    }
                if (IsViPNet) {
                    File.Delete($"{SubjectName}.pfx");
                    File.WriteAllBytes($"{SubjectName}.cer", Certificate.Bytes);
                    DoInvalidSignature(Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                    DoExpired(AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{Expired}}.cer");
                    DoExpired(AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{Future}}.cer");
                    DoExpiredKey(AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{ExpiredKey}}.cer");
                    DoExpiredKey(AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{FutureKey}}.cer");
                    DoEKU(AlgId,SecureCode,Certificate,IssuerCertificate,"1.3.6.1.5.5.7.3.1",$"{SubjectName}{{EKU}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{InvalidSignature}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{Expired}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{Future}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{ExpiredKey}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{FutureKey}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}{{EKU}}.cer");
                    UpdateViPNetContainer(SecureCode,Container,ProviderType,$"{SubjectName}.cer");
                    var FullContainerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),$@"Infotecs\Containers\{Container}");
                    //File.Delete(FullContainerPath);
                    }
                else
                    {
                    File.WriteAllBytes($"{SubjectName}.cer", Certificate.Bytes);
                    DoInvalidSignature(Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                    DoExpired(AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{Expired}}.cer");
                    DoExpired(AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{Future}}.cer");
                    DoExpiredKey(AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{ExpiredKey}}.cer");
                    DoExpiredKey(AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{FutureKey}}.cer");
                    DoEKU(AlgId,SecureCode,Certificate,IssuerCertificate,"1.3.6.1.5.5.7.3.1",$"{SubjectName}{{EKU}}.cer");
                    }
                }
            }

        #region M:DoInvalidSignature(X509Certificate,String,{out}Byte[])
        private static void DoInvalidSignature(X509Certificate SourceCertificate, String SerialNumber, out Byte[] Output) {
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
        #region M:DoInvalidSignature(X509Certificate,String,String)
        private static void DoInvalidSignature(X509Certificate SourceCertificate, String SerialNumber, String FileName) {
            DoInvalidSignature(SourceCertificate,SerialNumber,out var o);
            File.WriteAllBytes(FileName,o);
            }
        #endregion
        #region M:DoInvalidSignature(X509CertificateRevocationList,{out}Byte[])
        private static void DoInvalidSignature(X509CertificateRevocationList Source, out Byte[] Output) {
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
        private static void DoInvalidSignature(X509CertificateRevocationList Source, String FileName) {
            DoInvalidSignature(Source,out var o);
            File.WriteAllBytes(FileName,o);
            }
        #endregion
        #region M:DoExpired(ALG_ID,SecureString,X509Certificate,X509Certificate,Int32,FileName)
        private static void DoExpired(ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,String FileName)
            {
            DoExpired(AlgId,SecureCode,SourceCertificate,IssuerCertificate,Year,out var o);
            File.WriteAllBytes(FileName,o);
            }
        #endregion
        #region M:DoExpired(ALG_ID,SecureString,X509Certificate,X509Certificate,Int32,{out}Byte[])
        private static void DoExpired(ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,out Byte[] Output)
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
        private static void DoExpiredKey(ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,String FileName)
            {
            DoExpiredKey(AlgId,SecureCode,SourceCertificate,IssuerCertificate,Year,out var o);
            File.WriteAllBytes(FileName,o);
            }
        #endregion
        #region M:DoExpiredKey(ALG_ID,SecureString,X509Certificate,X509Certificate,Int32,{out}Byte[])
        private static void DoExpiredKey(ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,Int32 Year,out Byte[] Output)
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
        private static void DoEKU(ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,String EKU,String FileName)
            {
            DoEKU(AlgId,SecureCode,SourceCertificate,IssuerCertificate,EKU,out var o);
            File.WriteAllBytes(FileName,o);
            }
        #endregion
        #region M:DoEKU(ALG_ID,SecureString,X509Certificate,X509Certificate,String,{out}Byte[])
        private static void DoEKU(ALG_ID AlgId,SecureString SecureCode, X509Certificate SourceCertificate,X509Certificate IssuerCertificate,String EKU,out Byte[] Output)
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
        private static void MakeCRL(ALG_ID AlgId, DateTime DateTime, BigInteger Number, RequestSecureString SecureCode, X509Certificate IssuerCertificate, out X509CertificateRevocationList CRL,X509Certificate[] certificates) {
            var Builder = new X509CertificateRevocationListBuilder(DateTime.AddMonths(-1),DateTime.AddMonths(10),Number);
            Builder.Entries.AddRange(certificates.Select(i => new CertificateRevocationListEntry(i.Source, DateTime.AddMonths(-1),X509CrlReason.Superseded)));
            Builder.Build(IssuerCertificate, SecureCode, out CRL);
            }
        #endregion
        #region M:MakeCRL(ALG_ID,DateTime,BigInteger,RequestSecureString,X509Certificate,String,{params}X509Certificate[])
        private static void MakeCRL(ALG_ID AlgId, DateTime DateTime, BigInteger Number, RequestSecureString SecureCode, X509Certificate IssuerCertificate, String FileName,params X509Certificate[] certificates) {
            MakeCRL(AlgId,DateTime,Number,SecureCode,IssuerCertificate,out var r,certificates);
            File.WriteAllBytes(FileName,r.Bytes);
            }
        #endregion

        private static void DoCRLSet(ALG_ID AlgId, DateTime DateTime, RequestSecureString SecureCode, X509Certificate IssuerCertificate) {
            var IssuerName = IssuerCertificate.Source.Subject[ObjectIdentifiers.szOID_COMMON_NAME];
            MakeCRL(AlgId,DateTime,1,SecureCode,IssuerCertificate,out var r, new X509Certificate[0]);
            File.WriteAllBytes($"{IssuerName}.crl",r.Bytes);
            DoInvalidSignature(r,$"{IssuerName}{{InvalidSignature}}.crl");
            MakeCRL(AlgId,DateTime.AddYears(-10),2,SecureCode,IssuerCertificate,$"{IssuerName}{{Expired}}.crl");
            MakeCRL(AlgId,DateTime.AddYears(+10),3,SecureCode,IssuerCertificate,$"{IssuerName}{{Future}}.crl");
            }
        }
    }
