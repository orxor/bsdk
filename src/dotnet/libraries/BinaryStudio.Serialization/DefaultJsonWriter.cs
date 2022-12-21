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

        public Formatting Formatting
            {
            get { return Writer.Formatting; }
            set { Writer.Formatting = value; }
            }

        public IDisposable ScopeObject()
            {
            return new ObjectScopeObject(Writer);
            }

        public IDisposable ArrayObject()
            {
            return new ArrayScopeObject(Writer);
            }

        #region M:IJsonWriter.WriteValue(Object)
        void IJsonWriter.WriteValue(Object value) {
            if (value != null) {
                WriteValue(value);
                }
            else
                {
                Writer.WriteNull();
                }
            }
        #endregion
        #region M:IJsonWriter.WriteComment(String)
        /// <summary>
        /// Writes a comment <c>/*...*/</c> containing the specified text.
        /// </summary>
        /// <param name="comment">Text to place inside the comment.</param>
        void IJsonWriter.WriteComment(String comment)
            {
            Writer.WriteComment(comment);
            }
        #endregion
        #region M:IJsonWriter.WriteWhitespace(String)
        /// <summary>
        /// Writes the given white space.
        /// </summary>
        /// <param name="whitespace">The string of white space characters.</param>
        void IJsonWriter.WriteWhitespace(String whitespace)
            {
            Writer.WriteWhitespace(whitespace);
            }
        #endregion
        #region M:IJsonWriter.WriteWhitespace(Int32)
        /// <summary>
        /// Writes the given white space.
        /// </summary>
        /// <param name="whitespace">The count of white space characters.</param>
        void IJsonWriter.WriteWhitespace(Int32 whitespace) {
            if (whitespace > 0) {
                Writer.WriteWhitespace(new String(' ', whitespace));
                }
            }
        #endregion
        #region M:IJsonWriter.WriteRawString(String)
        /// <summary>
        /// Writes raw JSON without changing the writer's state.
        /// </summary>
        /// <param name="value">The raw JSON to write.</param>
        void IJsonWriter.WriteRawString(String value) {
            if (!String.IsNullOrEmpty(value)) {
                Writer.WriteRaw(value);
                }
            }
        #endregion
        #region M:IJsonWriter.WriteIndent
        void IJsonWriter.WriteIndent() {
            var mi = Writer.GetType().GetMethod("WriteIndent",BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public);
            if (mi != null) {
                mi.Invoke(Writer,null);
                }
            }
        #endregion

        #region M:WriteValue(String,Object)
        public void WriteValue(String name, Object value) {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (String.IsNullOrWhiteSpace(name)) { throw new ArgumentOutOfRangeException(nameof(name)); }
            if (value != null) {
                Writer.WritePropertyName(name);
                WriteValue(value);
                }
            else
                {
                Writer.WritePropertyName(name);
                Writer.WriteNull();
                }
            }
        #endregion
        #region M:WriteValueIfNotNull(String,Object)
        public void WriteValueIfNotNull(String name, Object value) {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (String.IsNullOrWhiteSpace(name)) { throw new ArgumentOutOfRangeException(nameof(name)); }
            if (value != null) {
                Writer.WritePropertyName(name);
                WriteValue(value);
                }
            }
        #endregion
        #region M:WritePropertyName(String)
        /// <summary>
        /// Writes the property name of a name/value pair of a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void WritePropertyName(String name) {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (String.IsNullOrWhiteSpace(name)) { throw new ArgumentOutOfRangeException(nameof(name)); }
            Writer.WritePropertyName(name);
            }
        #endregion
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