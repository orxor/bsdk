using System;
using System.Globalization;
using System.Numerics;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    [UsedImplicitly]
    [Asn1CertificateExtension("1.2.840.113556.1.5.284.7")]
    internal class DRSJoinType : CertificateExtension
        {
        public Int32 Value { get; }

        #region ctor{CertificateExtension}
        internal DRSJoinType(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    Value = (Int32)(new BigInteger(octet[0].Content.ToArray()));
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
                writer.WriteValue(nameof(Value), (Value == '1')
                    ? "Joined"
                    : "Registered");
                }
            }
        }
    }
