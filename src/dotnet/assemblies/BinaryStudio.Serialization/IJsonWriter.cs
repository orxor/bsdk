using System;

namespace BinaryStudio.Serialization
    {
    public interface IJsonWriter
        {
        IDisposable ScopeObject();
        IDisposable ArrayObject();
        void WriteValue(String name, Object value);
        void WriteValue(Object value);
        void WriteValueIfNotNull(String name, Object value);
        void WritePropertyName(String name);
        }
    }