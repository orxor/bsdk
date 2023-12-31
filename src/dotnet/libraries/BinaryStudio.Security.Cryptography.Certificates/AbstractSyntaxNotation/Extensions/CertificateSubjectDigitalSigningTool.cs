﻿using System;
using System.Globalization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.szCPOID_SubjectSignTool)]
    internal sealed class Asn1CertificateSubjectDigitalSigningToolExtension : CertificateExtension
        {
        public String DigitalSigningTool { get; }

        #region ctor{CertificateExtension}
        internal Asn1CertificateSubjectDigitalSigningToolExtension(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    DigitalSigningTool = ((Asn1String)octet[0]).Value;
                    }
                }
            }
        #endregion

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return DigitalSigningTool;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                if (!String.IsNullOrEmpty(DigitalSigningTool)) { writer.WriteValue(nameof(DigitalSigningTool), DigitalSigningTool); }
                }
            }
        }
    }