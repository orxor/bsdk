using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

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
    public class X509AlgorithmIdentifier : IExceptionSerializable
        {
        public Oid Identifier { get; }
        public Object Parameters { get; }

        #region ctor{Asn1Sequence}
        public X509AlgorithmIdentifier(Asn1Sequence o) {
            if (o == null) { throw new ArgumentNullException(nameof(o)); }
            var c = o.Count;
            if (c == 0) { throw new ArgumentOutOfRangeException(nameof(o)); }
            if (c >  2) { throw new ArgumentOutOfRangeException(nameof(o)); }
            if (!(o[0] is Asn1ObjectIdentifier)) { throw new ArgumentOutOfRangeException(nameof(o)); }
            Identifier = (Asn1ObjectIdentifier)o[0];
            if (c == 2) {
                Parameters = X509PublicKeyParameters.From(
                    Identifier.ToString(),
                    o[1]);
                }
            }
        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return Identifier.FriendlyName ?? Identifier.Value;
            }

        #region M:IExceptionSerializable.WriteTo(TextWriter)
        void IExceptionSerializable.WriteTo(TextWriter target) {
            using (var writer = new DefaultJsonWriter(new JsonTextWriter(target){
                    Formatting = Formatting.Indented,
                    Indentation = 2,
                    IndentChar = ' '
                    })) {
                ((IExceptionSerializable)this).WriteTo(writer);
                }
            }
        #endregion
        #region M:IExceptionSerializable.WriteTo(IJsonWriter)
        void IExceptionSerializable.WriteTo(IJsonWriter writer) {
            var x = Identifier.ToString();
            var y = Identifier.FriendlyName;
            writer.WriteValue(
                (x != y)
                ? $"{x} {{{y}}}"
                : $"{x}");
            }
        #endregion
        }
    }