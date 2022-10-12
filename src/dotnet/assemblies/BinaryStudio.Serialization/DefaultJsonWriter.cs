using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace BinaryStudio.Serialization
    {
    public class DefaultJsonWriter : IJsonWriter,IDisposable
        {
        private Boolean Disposed;
        public JsonWriter Writer { get;private set; }

        public DefaultJsonWriter(JsonWriter writer)
            {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            Writer = writer;
            }

        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the <see cref="DefaultJsonWriter"/> and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing) {
            if (!Disposed) {
                try
                    {
                    if (disposing) {
                        if (Writer != null) {
                            ((IDisposable)Writer).Dispose();
                            Writer = null;
                            }
                        }
                    }
                finally
                    {
                    Disposed = true;
                    }
                }
            }
        #endregion
        #region M:Finalize
        ~DefaultJsonWriter() {
            Dispose(false);
            }
        #endregion
        #region M:IDisposable.Dispose
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }
        #endregion

        public IDisposable ScopeObject()
            {
            return new ObjectScopeObject(Writer);
            }

        public IDisposable ArrayObject()
            {
            return new ArrayScopeObject(Writer);
            }

        public void WriteValue(String name, Object value) {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (String.IsNullOrWhiteSpace(name)) { throw new ArgumentOutOfRangeException(nameof(name)); }
            if (value != null) {
                Writer.WritePropertyName(name);
                WriteValue(value);
                }
            else
                {
                Writer.WriteNull();
                }
            }

        public void WriteValueIfNotNull(String name, Object value) {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (String.IsNullOrWhiteSpace(name)) { throw new ArgumentOutOfRangeException(nameof(name)); }
            if (value != null) {
                Writer.WritePropertyName(name);
                WriteValue(value);
                }
            }

        public void WritePropertyName(String name) {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (String.IsNullOrWhiteSpace(name)) { throw new ArgumentOutOfRangeException(nameof(name)); }
            Writer.WritePropertyName(name);
            }

        void IJsonWriter.WriteValue(Object value) {
            if (value != null) {
                WriteValue(value);
                }
            else
                {
                Writer.WriteNull();
                }
            }

        #region M:WriteValue(Object):Boolean
        private Boolean WriteValue(Object value) {
            if (value == null) { return false; }
            var status =
                WriteValue(value as IJsonSerializable) ||
                WriteValue(value as IList<String>) ||
                WriteValue(value as IList);
            if (!status) {
                var type = value.GetType();
                if (type.IsEnum) {
                    if (WriteValue(ToString((Enum)value))) { return true; }
                    }
                Writer.WriteValue(value);
                }
            return true;
            }
        #endregion
        #region M:WriteValue(IList):Boolean
        private Boolean WriteValue(IList values) {
            if (values == null) { return false; } 
            using (ArrayObject()) {
                foreach (var value in values) {
                    WriteValue(value);
                    }
                return true;
                }
            }
        #endregion
        #region M:WriteValue(IList<String>):Boolean
        private Boolean WriteValue(IList<String> values)
            {
            if (values == null) { return false; }
            using (ArrayObject()) {
                foreach (var value in values) {
                    WriteValue(value);
                    }
                return true;
                }
            }
        #endregion
        #region M:WriteValue(IJsonSerializable):Boolean
        private Boolean WriteValue(IJsonSerializable value)
            {
            if (value == null) { return false; }
            value.WriteTo(this);
            return true;
            }
        #endregion

        #region T:ArrayScopeObject
        private class ArrayScopeObject: IDisposable
            {
            private readonly JsonWriter writer;
            public ArrayScopeObject(JsonWriter writer)
                {
                this.writer = writer;
                writer.WriteStartArray();
                }

            public void Dispose()
                {
                writer.WriteEnd();
                }
            }
        #endregion
        #region T:ObjectScopeObject
        private class ObjectScopeObject: IDisposable
            {
            private readonly JsonWriter writer;
            public ObjectScopeObject(JsonWriter writer)
                {
                this.writer = writer;
                writer.WriteStartObject();
                }

            public void Dispose()
                {
                writer.WriteEndObject();
                }
            }
        #endregion
        #region M:IsNullOrEmpty(ICollection):Boolean
        private static Boolean IsNullOrEmpty(ICollection value) {
            return (value == null) || (value.Count == 0);
            }
        #endregion

        private static String ToString(Enum source) {
            var type = source.GetType();
            if (!IsNullOrEmpty(type.GetCustomAttributes(typeof(FlagsAttribute), false))) {
                return Enum.Format(type, source, "F");
                }
            var fields = type.GetFields();
            foreach (var field in fields) {
                if (field.IsLiteral) {
                    var value = field.GetValue(source);
                    if (Equals(value,source)) {
                        #if NET40
                        var DisplayName = field.GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault();
                        #else
                        var DisplayName = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));
                        #endif
                        if ((DisplayName != null) && !String.IsNullOrWhiteSpace(DisplayName.Name)) {
                            return DisplayName.Name;
                            }
                        break;
                        }
                    }
                }
            return Enum.Format(type, source, "G");;
            }
        }
    }