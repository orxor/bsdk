using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    /*
     * {iso(1) identified-organization(3) dod(6) internet(1) private(4) enterprise(1) 311 21 1}
     * 1.3.6.1.4.1.311.21.1
     * /ISO/Identified-Organization/6/1/4/1/311/21/1
     * Certificate services Certification Authority (CA) version
     * */
    [UsedImplicitly]
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_CERTSRV_CA_VERSION)]
    internal sealed class CertificateCAVersion : CertificateExtension
        {
        public Version Version { get; }

        #region ctor{CertificateExtension}
        internal CertificateCAVersion(CertificateExtension source)
            : base(source)
            {
            Version = new Version(0, 0);
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    var value = ((Asn1Integer)octet[0]).Content.ToArray();
                    if (value.Length == 1)
                        {
                        Version = new Version(value[0], 0);
                        }
                    else if (value.Length == 2)
                        {
                        var major = (((UInt16)value[0]) << 8) | (UInt16)value[1];
                        Version = new Version(major, 0);
                        }
                    else if (value.Length == 3)
                        {
                        var major = (((UInt16)value[1]) << 8) | (UInt16)value[2];
                        var minor = value[0];
                        Version = new Version(major, minor);
                        }
                    else if (value.Length == 4)
                        {
                        var major = (((UInt16)value[2]) << 8) | (UInt16)value[3];
                        var minor = (((UInt16)value[0]) << 8) | (UInt16)value[1];
                        Version = new Version(major, minor);
                        }
                    }
                }
            }
        #endregion
        public CertificateCAVersion(Version version)
            :base(ObjectIdentifiers.szOID_CERTSRV_CA_VERSION,false)
            {
            if (version == null) { throw new ArgumentException(nameof(version)); }
            Version = version;
            IList<Byte> r = new List<Byte>();
            r.Add((Byte)((version.Minor >> 8) & 0xff));
            r.Add((Byte)((version.Minor >> 0) & 0xff));
            r.Add((Byte)((version.Major >> 8) & 0xff));
            r.Add((Byte)((version.Major >> 0) & 0xff));
            Body = new Asn1OctetString(new Asn1Integer(r.ToArray()));
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString() {
            return Version.ToString();
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteIndent();
                writer.WriteComment($" {OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)} ");
                writer.WriteValue(nameof(Identifier), Identifier.ToString());
                writer.WriteValue(nameof(IsCritical), IsCritical);
                writer.WriteValue(nameof(Version), Version.ToString());
                }
            }
        }
    }