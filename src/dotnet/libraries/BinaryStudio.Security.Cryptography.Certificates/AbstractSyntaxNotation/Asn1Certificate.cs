using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DirectoryServices;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    using CertificateSKI=CertificateSubjectKeyIdentifier;
    using CertificateAKI=CertificateAuthorityKeyIdentifier;

    /// <summary>
    /// Represents ASN.1 certificate structure.
    /// </summary>
    /// <remarks>
    /// <pre style="font-family: Consolas">
    /// Certificate  ::=  SEQUENCE  {
    ///   tbsCertificate       TBSCertificate,
    ///   signatureAlgorithm   AlgorithmIdentifier,
    ///   signatureValue       BIT STRING
    ///   }
    ///
    /// TBSCertificate  ::=  SEQUENCE  {
    ///   version         [0]  EXPLICIT Version DEFAULT v1,
    ///   serialNumber         CertificateSerialNumber,
    ///   signature            AlgorithmIdentifier,
    ///   issuer               Name,
    ///   validity             Validity,
    ///   subject              Name,
    ///   subjectPublicKeyInfo SubjectPublicKeyInfo,
    ///   issuerUniqueID  [1]  IMPLICIT UniqueIdentifier OPTIONAL, # If present, version MUST be v2 or v3
    ///   subjectUniqueID [2]  IMPLICIT UniqueIdentifier OPTIONAL, # If present, version MUST be v2 or v3
    ///   extensions      [3]  EXPLICIT Extensions OPTIONAL        # If present, version MUST be v3
    ///   }
    /// Version  ::=  INTEGER  {  v1(0), v2(1), v3(2)  }
    /// CertificateSerialNumber  ::=  INTEGER
    /// Validity ::= SEQUENCE {
    ///   notBefore      Time,
    ///   notAfter       Time
    ///   }
    /// 
    /// Time ::= CHOICE {
    ///   utcTime        UTCTime,
    ///   generalTime    GeneralizedTime
    ///   }
    /// 
    /// UniqueIdentifier  ::=  BIT STRING
    /// 
    /// SubjectPublicKeyInfo  ::=  SEQUENCE  {
    ///   algorithm            AlgorithmIdentifier,
    ///   subjectPublicKey     BIT STRING
    ///   }
    /// 
    /// Extensions  ::=  SEQUENCE SIZE (1..MAX) OF Extension
    /// 
    /// Extension  ::=  SEQUENCE  {
    ///   extnID      OBJECT IDENTIFIER,
    ///   critical    BOOLEAN DEFAULT FALSE,
    ///   extnValue   OCTET STRING
    ///                  -- contains the DER encoding of an ASN.1 value
    ///                  -- corresponding to the extension type identified
    ///                  -- by extnID
    ///   }
    /// 
    /// AlgorithmIdentifier  ::=  SEQUENCE  {
    ///   algorithm               OBJECT IDENTIFIER,
    ///   parameters              ANY DEFINED BY algorithm OPTIONAL
    ///   }
    /// </pre>
    /// </remarks>
    public class Asn1Certificate : Asn1LinkObject, IExceptionSerializable, DigestSource
        {
        public Int32 Version { get; }
        public String SerialNumber { get; }
        public String Country { get; }
        public DateTime NotBefore { get; }
        public DateTime NotAfter  { get; }
        public String Thumbprint { get {
            if (thumbprint == null) {
                using (var engine = SHA1.Create())
                using(var output = new MemoryStream()) {
                    UnderlyingObject.WriteTo(output);
                    output.Seek(0, SeekOrigin.Begin);
                    thumbprint = engine.ComputeHash(output).ToString("x");
                    }
                }
            return thumbprint;
            }}
        public X509RelativeDistinguishedNameSequence Issuer  { get; }
        public X509RelativeDistinguishedNameSequence Subject { get; }
        public Asn1CertificateExtensionCollection Extensions { get; }
        public Asn1SignatureAlgorithm SignatureAlgorithm { get; }

        internal static Boolean FilterFriendlyName(KeyValuePair<Asn1ObjectIdentifier, String> source) {
            switch (source.Key.ToString()) {
                case "2.5.4.20":
                case "2.5.4.9":
                case "1.2.840.113549.1.9.1":
                    return false;
                }
            return true;
            }

        #region M:ToString(Object):String
        internal static String ToString(String source) {
            if (source == null) { return String.Empty; }
            var value = (String)source;
            value = value.Replace("\\=", "=");
            value = value.Replace("\\,", ",");
            value = value.Replace("\"" , "");
            value = value.Replace("/", "%2f");
            value = value.Replace(":",  "%3a");
            return value.Trim();
            }
        #endregion

        public String FriendlyName { get {
            var SK = ((CertificateSKI)Extensions?.FirstOrDefault(i => i is CertificateSKI))?.KeyIdentifier?.ToString("fl");
            var AK = ((CertificateAKI)Extensions?.FirstOrDefault(i => i is CertificateAKI));
            var r = new StringBuilder(String.Join(",", Subject.Where(FilterFriendlyName).Select(i => $"{new Oid(i.Key.ToString()).FriendlyName}={ToString(i.Value.ToString())}")));
            r.Append(",{");
            var flag = false;
            if (AK != null) {
                r.Append($"AKI={AK.KeyIdentifier.ToString("fl")}");
                if (AK.SerialNumber != null) {
                    r.Append($"+{{{AK.SerialNumber}}}");
                    }
                flag = true;
                }
            if (!String.IsNullOrWhiteSpace(SK)) {
                if (flag) {
                    r.Append(",");
                    }
                r.Append($"SKI={SK}");
                flag = true;
                }
            if (flag) {
                r.Append(",");
                }
            r.Append($"SN={SerialNumber}}}");
            return r.ToString();
            }}

        public Asn1Certificate(Asn1Object o)
            : base(o)
            {
            Extensions = new Asn1CertificateExtensionCollection(EmptyArray<CertificateExtension>.Value);
            State |= ObjectState.Failed;
            if (o is Asn1Sequence u) {
                if ((u[0] is Asn1Sequence) &&
                    (u[1] is Asn1Sequence) &&
                    (u[2] is Asn1BitString))
                    {
                    var j = 0;
                    if (u[0][0] is Asn1ContextSpecificObject) {
                        Version = (Int32)(Asn1Integer)u[0][0][0];
                        j++;
                        }
                    else
                        {
                        Version = 1;
                        }
                    SerialNumber = String.Join(String.Empty,((Asn1Integer)u[0][SerialNumberFieldIndex = j]).Value.ToByteArray().Reverse().Select(i => i.ToString("x2")));
                    SignatureAlgorithm = Asn1SignatureAlgorithm.From(new Asn1SignatureAlgorithm(u[0][j + 1]));
                    Issuer  = X509RelativeDistinguishedNameSequence.Build(u[0][IssuerFieldIndex  = j + 2]);
                    Subject = X509RelativeDistinguishedNameSequence.Build(u[0][SubjectFieldIndex = j + 4]);
                    #region Validity
                    if (u[0][ValidityFieldIndex = j + 3] is Asn1Sequence)
                        {
                        NotBefore = (Asn1Time)u[0][j + 3][0];
                        NotAfter  = (Asn1Time)u[0][j + 3][1];
                        }
                    else
                        {
                        State |= ObjectState.Failed;
                        return;
                        }
                    #endregion
                    #region Extensions
                    var contextspecifics = new List<Tuple<SByte,Int32,Asn1ContextSpecificObject>>();
                    var index = 0;
                    foreach (var item in u[0]) {
                        if (item is Asn1ContextSpecificObject i) {
                            contextspecifics.Add(Tuple.Create(i.Type,index,i));
                            }
                        index++;
                        }
                    var specific = contextspecifics.FirstOrDefault(i => i.Item1 == 3);
                    if (!ReferenceEquals(specific, null)) {
                        ExtensionsFieldIndex = specific.Item2;
                        Extensions = new Asn1CertificateExtensionCollection(specific.Item3[0].Select(i => CertificateExtension.From(new CertificateExtension(i))));
                        }
                    #endregion
                    Country = GetCountry(Subject) ?? GetCountry(Issuer);
                    State &= ~ObjectState.Failed;
                    }
                }
            }

        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Version),Version);
                writer.WriteValueIfNotNull(nameof(Country),Country);
                writer.WriteValue(nameof(NotBefore),NotBefore);
                writer.WriteValue(nameof(NotAfter),NotAfter);
                writer.WriteValue(nameof(SerialNumber),SerialNumber);
                writer.WriteValue(nameof(Subject),Subject);
                writer.WriteValue(nameof(Issuer),Issuer);
                writer.WriteValue(nameof(Thumbprint),Thumbprint);
                writer.WriteValueIfNotNull(nameof(Extensions),Extensions);
                }
            }
        #endregion
        #region M:IExceptionSerializable.WriteTo(TextWriter)
        void IExceptionSerializable.WriteTo(TextWriter target) {
            using (var writer = new DefaultJsonWriter(new JsonTextWriter(target){
                    Formatting = Formatting.Indented,
                    Indentation = 2,
                    IndentChar = ' '
                    })) {
                ((IExceptionSerializable)this).WriteTo(writer);
                }
            }
        #endregion
        #region M:IExceptionSerializable.WriteTo(IJsonWriter)
        void IExceptionSerializable.WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteValue(nameof(NotBefore),NotBefore);
                writer.WriteValue(nameof(NotAfter),NotAfter);
                writer.WriteValue(nameof(SerialNumber),SerialNumber);
                writer.WriteValue(nameof(Subject),Subject);
                writer.WriteValue(nameof(Issuer),Issuer);
                }
            }
        #endregion

        private static String GetCountry(X509RelativeDistinguishedNameSequence source) {
            var o = source.TryGetValue("2.5.4.6", out var r)
                ? r.ToString().ToLower()
                : null;
            if (o != null) {
                if (o.Length == 3) {
                    o = IcaoCountry.ThreeLetterCountries[o];
                    }
                }
            return o;
            }

        public IEnumerable<Byte[]> DigestSource { get{
            yield break;
            }}

        private String thumbprint;
        internal Int32 SubjectFieldIndex = -1;
        internal Int32 IssuerFieldIndex = -1;
        internal Int32 SerialNumberFieldIndex = -1;
        internal Int32 ValidityFieldIndex = -1;
        internal Int32 ExtensionsFieldIndex = -1;
        }
    }