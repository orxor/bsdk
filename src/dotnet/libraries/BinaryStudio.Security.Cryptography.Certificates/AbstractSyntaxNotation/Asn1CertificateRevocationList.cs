using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// CertificateList  ::=  SEQUENCE {
    ///   tbsCertList          TBSCertList,
    ///   signatureAlgorithm   AlgorithmIdentifier,
    ///   signatureValue       BIT STRING
    ///   }
    ///
    /// TBSCertList  ::=  SEQUENCE  {
    ///   version                 Version OPTIONAL, # if present, MUST be v2
    ///   signature               AlgorithmIdentifier,
    ///   issuer                  Name,
    ///   thisUpdate              Time,
    ///   nextUpdate              Time OPTIONAL,
    ///   revokedCertificates     SEQUENCE OF SEQUENCE  {
    ///     userCertificate         CertificateSerialNumber,
    ///     revocationDate          Time,
    ///     crlEntryExtensions      Extensions OPTIONAL # if present, version MUST be v2
    ///     }  OPTIONAL,
    ///   crlExtensions           [0]  EXPLICIT Extensions OPTIONAL # if present, version MUST be v2
    ///   }
    /// </summary>
    public class Asn1CertificateRevocationList : Asn1LinkObject
        {
        public Int32 Version { get; }
        public DateTime  EffectiveDate { get; }
        public DateTime? NextUpdate { get; }
        public IList<CertificateRevocationListEntry> Entries { get; }
        public X509RelativeDistinguishedNameSequence Issuer { get;private set; }
        public Asn1CertificateExtensionCollection Extensions { get; }
        public String Country { get; }

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

        public Asn1CertificateRevocationList(Asn1Object o) :
            base(o)
            {
            Entries = EmptyArray<CertificateRevocationListEntry>.Value;
            State |= ObjectState.Failed;
            if (o is Asn1Sequence u) {
                if (u.Count == 3) {
                    if ((u[0] is Asn1Sequence) &&
                        (u[1] is Asn1Sequence) &&
                        (u[2] is Asn1BitString))
                        {
                        var count = o[0].Count;
                        Version = (Int32)(Asn1Integer)u[0][0];
                        Issuer = new X509RelativeDistinguishedNameSequence(o[0][2].
                            Select(j => new KeyValuePair<Asn1ObjectIdentifier,String>(
                                (Asn1ObjectIdentifier)j[0][0], j[0][1].ToString())));
                        EffectiveDate = (Asn1Time)o[0][3];
                        var i = 4;
                        if (o[0][i] is Asn1Time) {
                            NextUpdate = (Asn1Time)o[0][4];
                            i++;
                            }
                        if ((i < count) && (o[0][i] is Asn1Sequence)) {
                            var r = new List<CertificateRevocationListEntry>();
                            foreach (var e in o[0][i]) {
                                r.Add(new CertificateRevocationListEntry(e));
                                }
                            Entries = r.ToArray();
                            i++;
                            }
                        if ((i < count) && (o[0][i] is Asn1ContextSpecificObject)) {
                            var specific = (Asn1ContextSpecificObject)o[0][i];
                            if (specific.Type == 0) {
                                Extensions = new Asn1CertificateExtensionCollection(o[0][i][0].Select(x => CertificateExtension.From(new CertificateExtension(x))).ToArray());
                                }
                            }
                        Country = GetCountry(Issuer);
                        State &= ~ObjectState.Failed;
                        }
                    }
                }
            }

        private static String GetCountry(X509RelativeDistinguishedNameSequence source) {
            return source.TryGetValue("2.5.4.6", out var r)
                ? r.ToString().ToLower()
                : null;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteValue(nameof(Version),Version);
                writer.WriteValue(nameof(EffectiveDate),EffectiveDate);
                writer.WriteValueIfNotNull(nameof(NextUpdate),NextUpdate);
                writer.WriteValueIfNotNull(nameof(Country),Country);
                writer.WriteValueIfNotNull(nameof(Issuer),Issuer);
                if (!IsNullOrEmpty(Entries)) {
                    writer.WritePropertyName(nameof(Entries));
                    using (writer.Object()) {
                        writer.WriteValue(nameof(Entries.Count),Entries.Count);
                        writer.WritePropertyName("{Self}");
                        using (writer.Array()) {
                            foreach (var e in Entries) {
                                writer.WriteValue(e);
                                }
                            }
                        }
                    }
                writer.WriteValueIfNotNull(nameof(Extensions),Extensions);
                }
            }

        private String thumbprint;
        }
    }