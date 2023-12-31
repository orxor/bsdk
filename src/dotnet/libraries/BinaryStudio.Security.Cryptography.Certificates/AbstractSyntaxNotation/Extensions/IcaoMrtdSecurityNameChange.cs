﻿using System;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /**
     * {joint-iso-itu-t(2) international-organizations(23) 136 mrtd(1) security(1) extensions(6) nameChange(1)}
     * {2.23.136.1.1.6.1}
     * {/Joint-ISO-ITU-T/International-Organizations/136/1/1/6/1}
     *
     * id-icao                                     OBJECT IDENTIFIER ::= {2.23.136}
     * id-icao-mrtd                                OBJECT IDENTIFIER ::= {id-icao 1}
     * id-icao-mrtd-security                       OBJECT IDENTIFIER ::= {id-icao-mrtd 1}
     * id-icao-mrtd-security-extensions            OBJECT IDENTIFIER ::= {id-icao-mrtdsecurity 6}
     * id-icao-mrtd-security-extensions-nameChange OBJECT IDENTIFIER ::= {idicaomrtd-security-extensions 1}
     * nameChange EXTENSION ::=
     * {
     *   SYNTAX NULL IDENTIFIED BY id-icao-mrtd-security-extensions-nameChange
     * }
     */
    [UsedImplicitly]
    [Asn1CertificateExtension("2.23.136.1.1.6.1")]
    public class IcaoMrtdSecurityNameChange : CertificateExtension
        {
        #region ctor{CertificateExtension}
        internal IcaoMrtdSecurityNameChange(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (octet == null)           { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (octet.Count != 1)        { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (!(octet[0] is Asn1Null)) { throw new ArgumentOutOfRangeException(nameof(source)); }
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
                }
            }
        }
    }