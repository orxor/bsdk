using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /**
     * {joint-iso-itu-t(2) international-organizations(23) 136 mrtd(1) security(1) extensions(6) documentTypeList(2)}
     * {2.23.136.1.1.6.2}
     * {/Joint-ISO-ITU-T/International-Organizations/136/1/1/6/2}
     *
     * id-icao                                     OBJECT IDENTIFIER ::= {2.23.136}
     * id-icao-mrtd                                OBJECT IDENTIFIER ::= {id-icao 1}
     * id-icao-mrtd-security                       OBJECT IDENTIFIER ::= {id-icao-mrtd 1}
     * id-icao-mrtd-security-extensions            OBJECT IDENTIFIER ::= {id-icao-mrtdsecurity 6}
     * id-icao-mrtd-security-extensions-documentTypeList OBJECT IDENTIFIER ::= {id-icao-mrtd-security-extensions 2}
     * documentTypeList EXTENSION ::=
     * {
     *   SYNTAX DocumentTypeListSyntax
     *   IDENTIFIED BY id-icao-mrtd-security-extensions-documentTypeList
     * }
     * DocumentTypeListSyntax ::= SEQUENCE
     * {
     *   version DocumentTypeListVersion,
     *   docTypeList SET OF DocumentType
     * }
     * DocumentTypeListVersion ::= INTEGER {v0(0)}
     * -- Document Type as contained in MRZ, e.g. "P" or "ID" where a
     * -- single letter denotes all document types starting with that letter
     * DocumentType ::= PrintableString(1..2)
     */
    [Asn1CertificateExtension(IcaoObjectIdentifiers.IcaoMrtdSecurityExtensionsDocumentTypeList)]
    [Asn1CertificateExtension("2.23.136.1.1.4")]
    internal class IcaoDocumentTypeList : Asn1CertificateExtension
        {
        public Int32 Version { get; }
        public IList<String> TypeList { get; }
        public IcaoDocumentTypeList(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            TypeList = new List<String>();
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    if (octet[0] is Asn1Sequence g) {
                        var n = 0;
                        if (g[n] is Asn1Integer version) {
                            Version = version;
                            n++;
                            }
                        if (g[n] is Asn1Set sequence) {
                            foreach (var i in sequence) {
                                TypeList.Add(((Asn1String)i).Value);
                                }
                            }
                        }
                    else if (octet[0] is Asn1String value)
                        {
                        TypeList.Add(value.Value);
                        }
                    else
                        {
                        Debug.Assert(false);
                        }
                    }
                }
            TypeList = new ReadOnlyCollection<String>(TypeList);
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(Version), Version);
                writer.WritePropertyName(nameof(TypeList));
                using (writer.ArrayObject()) {
                    foreach (var i in TypeList) {
                        writer.WriteValue(i);
                        }
                    }
                }
            }
        }
    }
