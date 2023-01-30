using System;
using System.ComponentModel;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <pre style="font-family: Consolas">
    /// AlgorithmIdentifier  ::=  SEQUENCE  {
    ///   algorithm               OBJECT IDENTIFIER,
    ///   parameters              ANY DEFINED BY algorithm OPTIONAL
    ///   }
    /// </pre>
    /// </remarks>
    [DefaultProperty(nameof(Identifier))]
    public class X509AlgorithmIdentifier
        {
        public Asn1ObjectIdentifier Identifier { get; }
        public Object Parameters { get; }

        public X509AlgorithmIdentifier(Asn1Sequence source)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var c = source.Count;
            if (c == 0)                               { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (c >  2)                               { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (!(source[0] is Asn1ObjectIdentifier)) { throw new ArgumentOutOfRangeException(nameof(source)); }
            Identifier = (Asn1ObjectIdentifier)source[0];
            if (c == 2) {
                Parameters = X509PublicKeyParameters.From(
                    Identifier.ToString(),
                    source[1]);
                }
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return Identifier.ToString();
            }
        }
    }