using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class CertificateRevocationListEntry : Asn1LinkObject
        {
        public String SerialNumber { get; }
        public DateTime RevocationDate { get; }
        public IList<CertificateExtension> Extensions { get; }

        #region ctor{Asn1Object}
        internal CertificateRevocationListEntry(Asn1Object source)
            : base(source)
            {
            SerialNumber = String.Join(String.Empty, ((Asn1Integer)source[0]).Value.ToByteArray().Select(i => i.ToString("X2")));
            RevocationDate = (Asn1Time)source[1];
            if (source.Count > 2) {
                Extensions = source[2].Select(
                    i => CertificateExtension.From(
                        new CertificateExtension(i))).ToArray();
                }
            }
        #endregion
        #region ctor{X509Certificate,DateTime,X509CrlReason}
        public CertificateRevocationListEntry(X509Certificate Certificate,DateTime InvalidityDate, X509CrlReason ReasonCode)
            : base(new Asn1PrivateObject(0))
            {
            SerialNumber = Certificate.SerialNumber;
            RevocationDate = InvalidityDate;
            Extensions = new CertificateExtension[]{
                new CRLReason(ReasonCode) 
                };
            }
        #endregion

        public override void WriteTo(Stream target, Boolean force = false) {
            var r = new Asn1Sequence {
                new Asn1Integer(SerialNumber),
                new Asn1UtcTime(RevocationDate),
                new Asn1Sequence(Extensions)
                };
            r.WriteTo(target, true);
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return SerialNumber;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteValue(nameof(SerialNumber), SerialNumber);
                writer.WriteValue(nameof(RevocationDate), RevocationDate.ToString("O"));
                writer.WriteValueIfNotNull(nameof(Extensions),Extensions);
                }
            }
        }
    }
