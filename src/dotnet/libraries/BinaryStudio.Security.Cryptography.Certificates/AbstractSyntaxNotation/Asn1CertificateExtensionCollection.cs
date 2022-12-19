using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    [XmlRoot("Extensions")]
    public class Asn1CertificateExtensionCollection : ReadOnlyCollection<Asn1CertificateExtension>
        {
        public Asn1CertificateExtensionCollection(IList<Asn1CertificateExtension> list)
            : base(list)
            {
            }
        }
    }