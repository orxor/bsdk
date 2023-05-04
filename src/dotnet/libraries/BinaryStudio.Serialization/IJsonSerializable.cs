using Newtonsoft.Json;

namespace BinaryStudio.Serialization
    {
    public interface IJsonSerializable
        {
        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        void WriteTo(IJsonWriter writer);
        }
    }
