using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class Asn1ReadOnlyCollection<T> : ReadOnlyCollection<T>
        {
        public Asn1ReadOnlyCollection(IList<T> source)
            : base(source)
            {
            }

        protected Asn1ReadOnlyCollection(IEnumerable<T> source)
            : base(source.ToList())
            {
            }
        }
    }
