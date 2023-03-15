using System.Collections.Generic;
using System.Globalization;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using BinaryStudio.Serialization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
{
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) keyUsage(15)}
     * 2.5.29.15
     * /Joint-ISO-ITU-T/5/29/15
     * Key usage
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension(ObjectIdentifiers.szOID_KEY_USAGE)]
    public sealed class CertificateKeyUsage : CertificateExtension
        {
        public X509KeyUsageFlags KeyUsage { get; }

        #region ctor{CertificateExtension}
        internal CertificateKeyUsage(CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0) {
                    var bitstring = octet[0] as Asn1BitString;
                    if (!ReferenceEquals(bitstring, null)) {
                        if (bitstring.Content.Length > 0) {
                            bitstring.Content.Seek(0, SeekOrigin.Begin);
                            var i = bitstring.Content.ReadByte();
                            KeyUsage = (X509KeyUsageFlags)i;
                            }
                        }
                    }
                }
            }
        #endregion
        #region ctor{X509KeyUsageFlags}
        public CertificateKeyUsage(X509KeyUsageFlags KeyUsage)
            :base(ObjectIdentifiers.szOID_KEY_USAGE,true)
            {
            this.KeyUsage = KeyUsage;
            Body = new Asn1OctetString{
                new Asn1BitString(1, new Byte[]{ (Byte)(Int32)KeyUsage })
                };
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
                var r = new List<String>();
                var n = (UInt32)KeyUsage;
                if ((n & (UInt32)X509KeyUsageFlags.EncipherOnly)     != 0) { r.Add(nameof(X509KeyUsageFlags.EncipherOnly)     ); n &= ~(UInt32)X509KeyUsageFlags.EncipherOnly;     }
                if ((n & (UInt32)X509KeyUsageFlags.CrlSign)          != 0) { r.Add(nameof(X509KeyUsageFlags.CrlSign)          ); n &= ~(UInt32)X509KeyUsageFlags.CrlSign;          }
                if ((n & (UInt32)X509KeyUsageFlags.KeyCertSign)      != 0) { r.Add(nameof(X509KeyUsageFlags.KeyCertSign)      ); n &= ~(UInt32)X509KeyUsageFlags.KeyCertSign;      }
                if ((n & (UInt32)X509KeyUsageFlags.KeyAgreement)     != 0) { r.Add(nameof(X509KeyUsageFlags.KeyAgreement)     ); n &= ~(UInt32)X509KeyUsageFlags.KeyAgreement;     }
                if ((n & (UInt32)X509KeyUsageFlags.DataEncipherment) != 0) { r.Add(nameof(X509KeyUsageFlags.DataEncipherment) ); n &= ~(UInt32)X509KeyUsageFlags.DataEncipherment; }
                if ((n & (UInt32)X509KeyUsageFlags.KeyEncipherment)  != 0) { r.Add(nameof(X509KeyUsageFlags.KeyEncipherment)  ); n &= ~(UInt32)X509KeyUsageFlags.KeyEncipherment;  }
                if ((n & (UInt32)X509KeyUsageFlags.NonRepudiation)   != 0) { r.Add(nameof(X509KeyUsageFlags.NonRepudiation)   ); n &= ~(UInt32)X509KeyUsageFlags.NonRepudiation;   }
                if ((n & (UInt32)X509KeyUsageFlags.DigitalSignature) != 0) { r.Add(nameof(X509KeyUsageFlags.DigitalSignature) ); n &= ~(UInt32)X509KeyUsageFlags.DigitalSignature; }
                if ((n & (UInt32)X509KeyUsageFlags.DecipherOnly)     != 0) { r.Add(nameof(X509KeyUsageFlags.DecipherOnly)     ); n &= ~(UInt32)X509KeyUsageFlags.DecipherOnly;     }
                if (!IsNullOrEmpty(r)) {
                    writer.WriteValue(nameof(KeyUsage), r);
                    }
                }
            }
        }
    }