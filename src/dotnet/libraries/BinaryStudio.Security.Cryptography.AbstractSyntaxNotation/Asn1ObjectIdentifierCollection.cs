using System.Collections.Generic;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1ObjectIdentifierCollection : Asn1ReadOnlyCollection<Asn1ObjectIdentifier>
        {
        public Asn1ObjectIdentifierCollection(IList<Asn1ObjectIdentifier> source)
            : base(source)
            {
            }

        public Asn1ObjectIdentifierCollection(IEnumerable<Asn1ObjectIdentifier> source)
            : base(source)
            {
            }
        }
    }
