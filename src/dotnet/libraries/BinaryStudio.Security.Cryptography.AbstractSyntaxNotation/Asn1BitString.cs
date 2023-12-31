﻿using System;
using System.IO;
using BinaryStudio.IO;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="BIT STRING"/> type.
    /// </summary>
    public sealed class Asn1BitString : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.BitString"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.BitString; }}
        public Int32 UnusedBits { get; private set; }

        #region ctor
        internal Asn1BitString()
            {
            }
        #endregion
        #region ctor{Int32,Byte[]}
        public Asn1BitString(Int32 unused, Byte[] source)
            {
            UnusedBits = unused;
            content = new ReadOnlyMemoryMappingStream(source);
            length = source.Length;
            size = GetHeader().Length + 1 + length;
            State |= ObjectState.Decoded;
            }
        #endregion

        #region M:Decode:Boolean
        protected override Boolean Decode() {
            if (IsDecoded) { return true; }
            if (IsIndefiniteLength) { return base.Decode(); }
            if (Length == 0) {
                State |= ObjectState.Failed;
                return false;
                }
            UnusedBits = Content.ReadByte();
            content = Content.Clone(Length - 1);
            length = Length - 1;
            base.Decode();
            State |= ObjectState.Decoded;
            State |= ObjectState.DisposeContent;
            return true;
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Class), Class.ToString());
                writer.WriteValue(nameof(Type), Type);
                if (Offset >= 0) { writer.WriteValue(nameof(Offset), Offset); }
                writer.WriteValue(nameof(UnusedBits), UnusedBits);
                var c = Count;
                if (c > 0) {
                    writer.WritePropertyName("{Self}");
                    using (writer.Array()) {
                        foreach (var Value in this) {
                            Value.WriteTo(writer);
                            }
                        }
                    }
                else
                    {
                    writer.WriteValue(nameof(Content),Convert.ToBase64String(Content.ToArray(), Base64FormattingOptions.InsertLineBreaks).Split('\n'));
                    }
                }
            }
        #endregion

        #region M:WriteContent(Stream)
        protected override void WriteContent(Stream target, Boolean force)
            {
            target.WriteByte((Byte)UnusedBits);
            base.WriteContent(target,force);
            }
        #endregion
        }
    }