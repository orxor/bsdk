using BinaryStudio.Serialization;
using System;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public interface IX509GeneralName
        {
        Boolean IsEmpty { get; }
        X509GeneralNameType Type { get; }
        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        void WriteTo(IJsonWriter writer);
        }
    }
