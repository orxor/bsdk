using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    /*
     * {joint-iso-itu-t(2) ds(5) certificateExtension(29) issuerAltName(18)}
     * 2.5.29.18
     * /Joint-ISO-ITU-T/5/29/18
     * Issuer alternative name
     * IETF RFC 5280
     * */
    [Asn1CertificateExtension("2.5.29.18")]
    public class Asn1IssuerAlternativeName : Asn1CertificateExtension
        {
        public IList<IX509GeneralName> AlternativeName { get; }
        public Asn1IssuerAlternativeName(Asn1CertificateExtension source)
            : base(source)
            {
            var octet = Body;
            if (!ReferenceEquals(octet, null)) {
                if (octet.Count > 0)
                    {
                    AlternativeName = new ReadOnlyCollection<IX509GeneralName>(octet[0].
                        OfType<Asn1ContextSpecificObject>().
                        Select(X509GeneralName.From).
                        Where(i => !i.IsEmpty).
                        ToArray());
                    }
                }
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return ((AlternativeName != null) && (AlternativeName.Count > 0))
                ? String.Join(";", AlternativeName.Select(i => $"{{{X509GeneralName.ToString(i.Type)}}}:{{{i}}}"))
                : "{none}";
            }
        }
    }