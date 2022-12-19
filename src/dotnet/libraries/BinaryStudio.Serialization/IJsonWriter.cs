using System;
using Newtonsoft.Json;

namespace BinaryStudio.Serialization
    {
    public interface IJsonWriter
        {
        Formatting Formatting { get;set; }
        IDisposable ScopeObject();
        IDisposable ArrayObject();
        void WriteValue(String name, Object value);
        void WriteValue(Object value);
        void WriteValueIfNotNull(String name, Object value);
        void WritePropertyName(String name);
        void WriteComment(String comment);
        }
    }