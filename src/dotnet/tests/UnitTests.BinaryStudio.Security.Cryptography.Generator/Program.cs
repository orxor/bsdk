using System;
using System.IO;
using System.Linq;
using System.Security;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;

namespace UnitTests.BinaryStudio.Security.Cryptography.Generator
    {
    internal class Program
        {
        private static void Main(String[] args)
            {
            var SecureCode = CryptographicContext.GetSecureString("SomePassword");
            var dt = DateTime.Now;
            var AlgId = ALG_ID.CALG_GR3410EL;
            DoSet(AlgId,dt,SecureCode,"R-CA",new Byte[]{ 1,2,3}, out var RootCertificate);
            DoSet(AlgId,dt,SecureCode,"I-CA",new Byte[]{ 4,5,6}, RootCertificate, out var IntermediateCertificate);
            DoSet(AlgId,dt,SecureCode,"User1",new Byte[]{  7, 8, 9}, IntermediateCertificate, out var User1);
            DoSet(AlgId,dt,SecureCode,"User2",new Byte[]{ 10,11,12}, IntermediateCertificate, out var User2);
            DoSet(AlgId,dt,SecureCode,"User3",new Byte[]{ 13,14,15}, IntermediateCertificate, out var User3);
            }

        private static void DoSet(ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, String SubjectName,Byte[] SerialNumber, out X509Certificate Certificate) {
            using (var Output = new MemoryStream()) {
                CryptographicContext.MakeCertificate(AlgId,$"CN={SubjectName}, C=ru",String.Join(String.Empty,SerialNumber.Select(i=> i.ToString("x2"))),
                    DateTime.AddYears(-1),DateTime.AddYears(5),
                    new Asn1CertificateExtension[] {
                        new CertificateSubjectKeyIdentifier(false,"89abcdeffedcba98765432100123456789abcdef")
                        },
                    Output, SecureCode,
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

        private static void DoSet(ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, String SubjectName,Byte[] SerialNumber, X509Certificate IssuerCertificate, out X509Certificate Certificate) {
            using (var Output = new MemoryStream()) {
                CryptographicContext.MakeCertificate(AlgId,$"CN={SubjectName}, C=ru",String.Join(String.Empty,SerialNumber.Select(i=> i.ToString("x2"))),
                    DateTime.AddYears(-1),DateTime.AddYears(5),null,
                    Output, SecureCode,
                    out Certificate,IssuerCertificate,false);
                using (var PrivateKeyOutput = File.Create($"{SubjectName}.pfx")) {
                    Output.Seek(0,SeekOrigin.Begin);
                    Output.CopyTo(PrivateKeyOutput);
                    }
                File.WriteAllBytes($"{SubjectName}.cer", Certificate.Bytes);
                DoInvalidSignature(Certificate,String.Join(String.Empty,SerialNumber.Reverse().Select(i=> i.ToString("x2"))),$"{SubjectName}{{InvalidSignature}}.cer");
                DoExpired(AlgId,SecureCode,Certificate,IssuerCertificate,-10,$"{SubjectName}{{Expired}}.cer");
                DoExpired(AlgId,SecureCode,Certificate,IssuerCertificate,+10,$"{SubjectName}{{Future}}.cer");
                }
            }

        #region M:DoInvalidSignature(X509Certificate,String,{out}Byte[])
        private static void DoInvalidSignature(X509Certificate SourceCertificate, String SerialNumber, out Byte[] Output) {
            Output = null;
            var Builder = Asn1Object.Load(new ReadOnlyMemoryMappingStream(SourceCertificate.Bytes)).First();
            Builder[0][SourceCertificate.Source.SerialNumberFieldIndex] = new Asn1Integer(SerialNumber);
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

        private static void MakeCRL(ALG_ID AlgId, DateTime DateTime, SecureString SecureCode, X509Certificate IssuerCertificate, out X509CertificateRevocationList CRL) {
            CRL = null;
            }

        private static void MakeSelfSignedCertificate(out X509Certificate Certificate,SecureString SecureCode, Boolean DeletePrivateKey) {
            var dt = DateTime.Now;
            CryptographicContext.MakeCertificate(ALG_ID.CALG_GR3410EL,"CN=R-CA, C=ru","010203",
                dt.AddYears(-1),dt.AddYears(10),
                new Asn1CertificateExtension[] {
                    new CertificateSubjectKeyIdentifier(false,"89abcdeffedcba98765432100123456789abcdef")
                    },
                Stream.Null, SecureCode,
                out Certificate,DeletePrivateKey);
            }
        }
    }
