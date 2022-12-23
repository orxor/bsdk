using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1CertificateRevocationList : Asn1LinkObject
        {
        public Int32 Version { get; }
        public DateTime  EffectiveDate { get; }
        public DateTime? NextUpdate { get; }
        public IList<Asn1CertificateRevocationListEntry> Entries { get; }
        public X509RelativeDistinguishedNameSequence Issuer { get;private set; }
        public Asn1CertificateExtensionCollection Extensions { get; }
        public String Country { get; }

        public Asn1CertificateRevocationList(Asn1Object o) :
            base(o)
            {
            Entries = EmptyArray<Asn1CertificateRevocationListEntry>.Value;
            State |= ObjectState.Failed;
            if (o is Asn1Sequence u) {
                if (u.Count == 3) {
                    if ((u[0] is Asn1Sequence) &&
                        (u[1] is Asn1Sequence) &&
                        (u[2] is Asn1BitString))
                        {
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
                        if (o[0][i] is Asn1Sequence) {
                            var r = new List<Asn1CertificateRevocationListEntry>();
                            foreach (var e in o[0][i]) {
                                r.Add(new Asn1CertificateRevocationListEntry(e));
                                }
                            Entries = r.ToArray();
                            i++;
                            }
                        if (o[0][i] is Asn1ContextSpecificObject) {
                            var specific = (Asn1ContextSpecificObject)o[0][i];
                            if (specific.Type == 0) {
                                Extensions = new Asn1CertificateExtensionCollection(o[0][i][0].Select(x => Asn1CertificateExtension.From(new Asn1CertificateExtension(x))).ToArray());
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
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(Version),Version);
                writer.WriteValue(nameof(EffectiveDate),EffectiveDate);
                writer.WriteValueIfNotNull(nameof(NextUpdate),NextUpdate);
                writer.WriteValueIfNotNull(nameof(Country),Country);
                writer.WriteValueIfNotNull(nameof(Issuer),Issuer);
                if (!IsNullOrEmpty(Entries)) {
                    writer.WritePropertyName(nameof(Entries));
                    using (writer.ScopeObject()) {
                        writer.WriteValue(nameof(Entries.Count),Entries.Count);
                        writer.WritePropertyName("{Self}");
                        using (writer.ArrayObject()) {
                            foreach (var e in Entries) {
                                writer.WriteValue(e);
                                }
                            }
                        }
                    }
                writer.WriteValueIfNotNull(nameof(Extensions),Extensions);
                }
            }
        }
    }