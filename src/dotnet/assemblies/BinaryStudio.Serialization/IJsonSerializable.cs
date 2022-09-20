using Newtonsoft.Json;

namespace BinaryStudio.Serialization
    {
    public interface IJsonSerializable
        {
        void WriteTo(IJsonWriter writer);
        }
    }
