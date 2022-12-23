using System;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {
    internal abstract class Asn1GeneralNameObject: Asn1LinkObject<Asn1ContextSpecificObject>,IX509GeneralName
        {
        protected internal Asn1GeneralNameObject(Asn1ContextSpecificObject source)
            : base(source)
            {
            }

        public override String ToString()
            {
            return GetType().Name;
            }

        public virtual Boolean IsEmpty
            {
            get { return false; }
            }

        X509GeneralNameType IX509GeneralName.Type { get { return InternalType; }}
        protected abstract X509GeneralNameType InternalType { get; }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            using (writer.ScopeObject()) {
                writer.WriteValue("Type", InternalType.ToString());
                writer.WriteValue("Value", ToString());
                }
            }
        }
    }