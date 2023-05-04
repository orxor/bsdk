using System;
using System.Text;
using BinaryStudio.Security.Cryptography.Certificates;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    internal class Asn1Uri : Asn1GeneralNameObject
        {
        public String Uri { get; }
        internal Asn1Uri(Asn1ContextSpecificObject source)
            : base(source)
            {
            var strval = Encoding.UTF8.GetString(source.Content.ToArray());
            Uri = strval;
            }

        public override String ToString()
            {
            return Uri.ToString();
            }

        protected override X509GeneralNameType InternalType { get { return X509GeneralNameType.IA5String; }}
        }
    }