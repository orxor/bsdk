using System.IO;
using BinaryStudio.Serialization;

namespace BinaryStudio.DiagnosticServices
    {
    public interface IExceptionSerializable
        {
        void WriteTo(TextWriter writer);
        void WriteTo(IJsonWriter writer);
        }
    }