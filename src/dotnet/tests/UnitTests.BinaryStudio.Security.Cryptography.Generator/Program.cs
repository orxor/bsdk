using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Security.Cryptography.X509Certificates;
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
            var SecureCode = CryptographicContext.GetSecureString("12345678");
            var RequestSecureCode = new RequestSecureCode(SecureCode);
            var dt = DateTime.Now;
            var AlgId = ALG_ID.CALG_GR3410EL;
            DoSet(AlgId,dt,SecureCode,"R-CA",new Byte[]{ 1,2,3}, X509KeyUsageFlags.CrlSign|X509KeyUsageFlags.KeyCertSign|X509KeyUsageFlags.DigitalSignature, out var RootCertificate);
            DoSet(AlgId,dt,SecureCode,"I-CA",new Byte[]{ 4,5,6}, RootCertificate, X509KeyUsageFlags.CrlSign|X509KeyUsageFlags.KeyCertSign|X509KeyUsageFlags.DigitalSignature, false, out var IntermediateCertificate);
            DoSet(AlgId,dt,SecureCode,"User1",new Byte[]{  7, 8, 9}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature, true, out var User1);
            DoSet(AlgId,dt,SecureCode,"User2",new Byte[]{ 10,11,12}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature, true, out var User2);
            DoSet(AlgId,dt,SecureCode,"User3",new Byte[]{ 13,14,15}, IntermediateCertificate,X509KeyUsageFlags.DigitalSignature, true, out var User3);
            DoCRLSet(AlgId,dt,RequestSecureCode,RootCertificate);
            DoCRLSet(AlgId,dt,RequestSecureCode,IntermediateCertificate);
            MakeCRL(AlgId,dt,04,RequestSecureCode,RootCertificate,"R-CA{Revoked}.crl",IntermediateCertificate);
            MakeCRL(AlgId,dt,05,RequestSecureCode,IntermediateCertificate,"I-CA{Revoked}.crl",User1,User3);
            }

        private static void DoSet(ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, String SubjectName,Byte[] SerialNumber, X509KeyUsageFlags KeyUsage, out X509Certificate Certificate) {
            var Extensions = new List<CertificateExtension> {
                new Asn1CertificateBasicConstraintsExtension(X509SubjectType.CA),
                new CertificateSubjectKeyIdentifier(false,"89abcdeffedcba98765432100123456789abcdef"),
                };
            if (KeyUsage != 0) {
                Extensions.Add(new CertificateKeyUsage(KeyUsage));
                }
            using (var Output = new MemoryStream()) {
                CryptographicContext.MakeCertificate(AlgId,$"CN={SubjectName}",String.Join(String.Empty,SerialNumber.Select(i=> i.ToString("x2"))),
                    DateTime.AddYears(-1),DateTime.AddYears(5),
                    Extensions,Output, SecureCode,
                    out Certificate,false);
                using (var PrivateKeyOutput = File.Create($"{SubjectName}.pfx")) {
                    Output.Seek(0,SeekOrigin.Begin);
                    Output.CopyTo(PrivateKeyOutput);
                    }
                File.WriteAllBytes($"{SubjectName}.cer", Certificate.Bytes);
                DoInvalidSignature(Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                DoExpired(AlgId,SecureCode,Certificate,Certificate,-10,$"{SubjectName}{{Expired}}.cer");
                DoExpired(AlgId,SecureCode,Certificate,Certificate,+10,$"{SubjectName}{{Future}}.cer");
                }
            }

        private static void DoSet(ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, String SubjectName,Byte[] SerialNumber, X509Certificate IssuerCertificate,  X509KeyUsageFlags KeyUsage, Boolean IsLeaf, out X509Certificate Certificate) {
            var Extensions = new List<CertificateExtension>();
            if (!IsLeaf) {
                Extensions.Add(new CertificateSubjectKeyIdentifier(false,"90abcdeffedcba98765432100123456789abcdef"));
                Extensions.Add(new Asn1CertificateBasicConstraintsExtension(X509SubjectType.CA,0));
                }
            if (KeyUsage != 0) {
                Extensions.Add(new CertificateKeyUsage(KeyUsage));
                }
            using (var Output = new MemoryStream()) {
                CryptographicContext.MakeCertificate(AlgId,$"CN={SubjectName}",String.Join(String.Empty,SerialNumber.Select(i=> i.ToString("x2"))),
                    DateTime.AddYears(-1),DateTime.AddYears(5),
                    Extensions,Output, SecureCode,
                    out Certificate,IssuerCertificate,false);
                using (var PrivateKeyOutput = File.Create($"{SubjectName}.pfx")) {
                    Output.Seek(0,SeekOrigin.Begin);
                    Output.CopyTo(PrivateKeyOutput);
                    }
                File.WriteAllBytes($"{SubjectName}.cer", Certificate.Bytes);
                DoInvalidSignature(Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                DoExpired(AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{Expired}}.cer");
                DoExpired(AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{Future}}.cer");
                DoExpiredKey(AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{ExpiredKey}}.cer");
                DoExpiredKey(AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{FutureKey}}.cer");
                DoEKU(AlgId,SecureCode,Certificate,IssuerCertificate,"1.3.6.1.5.5.7.3.1",$"{SubjectName}{{EKU}}.cer");
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

        private static void MakeSelfSignedCertificate(out X509Certificate Certificate,SecureString SecureCode, Boolean DeletePrivateKey) {
            var dt = DateTime.Now;
            CryptographicContext.MakeCertificate(ALG_ID.CALG_GR3410EL,"CN=R-CA, C=ru","010203",
                dt.AddYears(-1),dt.AddYears(10),
                new CertificateExtension[] {
                    new CertificateSubjectKeyIdentifier(false,"89abcdeffedcba98765432100123456789abcdef")
                    },
                Stream.Null, SecureCode,
                out Certificate,DeletePrivateKey);
            }

        }
    }
