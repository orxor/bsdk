using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    public abstract class OMFSSection : IFileDumpSupport, IXmlSerializable
        {
        public CV_CPU_TYPE? CPU { get;internal set; }
        public OMFDirectory Directory { get; }
        public abstract OMFSSectionIndex SectionIndex { get; }
        public UInt16 ModuleIndex { get;internal set; }
        public Int32 RelativeOffset { get;internal set; }
        public Int64 FileOffset { get;internal set; }
        public Int32 Size { get;internal set; }

        protected OMFSSection(OMFDirectory Directory)
            {
            this.Directory = Directory;
            }

        public abstract unsafe OMFSSection Analyze(Byte* BaseAddress,Byte* Source, Int32 Size);
        public virtual void ResolveReferences(OMFDirectory Directory)
            {
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return $"sst{SectionIndex}";
            }

        public virtual void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags)
            {
            return;
            }

        #region M:ToString(Encoding,Byte*,Boolean):String
        protected static unsafe String ToString(Encoding encoding, Byte* value, Boolean lengthprefixed) {
            if (lengthprefixed) {
                var c = (Int32)(*value);
                var r = new Byte[c];
                for (var i = 0;i < c;++i) {
                    r[i] = value[i + 1];
                    }
                return encoding.GetString(r);
                }
            else
                {
                var r = new List<Byte>();
                while (*value != 0) {
                    r.Add(*value);
                    value++;
                    }
                return encoding.GetString(r.ToArray());
                }
            }
        #endregion
        #region M:ToString(Encoding,Byte*,Int32):String
        protected static unsafe String ToString(Encoding Encoding, Byte* Value, Int32 Length) {
            var r = new Byte[Length];
            for (var i = 0;i < Length;++i) {
                r[i] = Value[i];
                }
            return Encoding.GetString(r);
            }
        #endregion
        #region M:IXmlSerializable.GetSchema:XmlSchema
        /// <summary>This method is reserved and should not be used. When implementing the <see langword="IXmlSerializable"/> interface, you should return <see langword="null" /> (<see langword="Nothing" /> in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.</summary>
        /// <returns>An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.</returns>
        XmlSchema IXmlSerializable.GetSchema() {
            return null;
            }
        #endregion
        #region M:IXmlSerializable.ReadXml(XmlReader)
        /// <summary>Generates an object from its XML representation.</summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
            {
            throw new NotSupportedException();
            }
        #endregion
        #region M:WriteXml(XmlWriter)
        /// <summary>Converts an object into its XML representation.</summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public virtual void WriteXml(XmlWriter writer)
            {
            throw new NotSupportedException();
            }
        #endregion
        }
    }