using System.IO;
using Newtonsoft.Json;

namespace BinaryStudio.DiagnosticServices
    {
    public interface IExceptionSerializable
        {
        void WriteTo(TextWriter target);
        void WriteTo(JsonWriter writer, JsonSerializer serializer);
        }
    }