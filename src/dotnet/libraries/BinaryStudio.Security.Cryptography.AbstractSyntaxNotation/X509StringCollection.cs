using System;
using System.Collections.Generic;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    public class X509StringCollection : Asn1ReadOnlyCollection<String>
        {
        public X509StringCollection(IEnumerable<String> source)
            : base(source)
            {
            }
        }
    }
