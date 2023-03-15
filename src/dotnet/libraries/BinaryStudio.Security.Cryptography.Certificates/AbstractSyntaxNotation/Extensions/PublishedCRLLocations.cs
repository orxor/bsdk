using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {iso(1) identified-organization(3) dod(6) internet(1) private(4) enterprise(1) 311 21 14}
     * 1.3.6.1.4.1.311.21.14
     * /ISO/Identified-Organization/6/1/4/1/311/21/14
     * szOID_CRL_SELF_CDP
     * */
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_CRL_SELF_CDP)]
    internal class Asn1PublishedCRLLocations : CertificateExtension
        {
        public IList<IX509GeneralName> PublishedCRLLocations { get; }

        #region ctor{CertificateExtension}
        internal Asn1PublishedCRLLocations(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    PublishedCRLLocations = new ReadOnlyCollection<IX509GeneralName>(octet[0][0][0][0].
                        OfType<Asn1ContextSpecificObject>().
                        Select(X509GeneralName.From).
                        Where(i => !i.IsEmpty).
                        ToArray());
                    }
                }
            }
        #endregion

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                if (PublishedCRLLocations != null)
                    {
                    writer.WritePropertyName(nameof(PublishedCRLLocations));
                    using (writer.Array()) {
                        foreach (var name in PublishedCRLLocations.OfType<IJsonSerializable>()) {
                            name.WriteTo(writer);
                            }
                        }
                    }
                }
            }
        }
    }