using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using BinaryStudio.IO;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="ENUMERATED"/> type.
    /// </summary>
    public sealed class Asn1Enum : Asn1UniversalObject
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Enum"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Enum; }}
        public BigInteger Value { get;private set; }

        #region ctor
        internal Asn1Enum()
            {
            }
        #endregion
        #region ctor{Byte[]}
        public Asn1Enum(Byte[] value)
            {
            Value = new BigInteger(value.Reverse().ToArray());
            State |= ObjectState.Decoded;
            }
        #endregion
        #region ctor{String}
        public Asn1Enum(String value)
            :this(DecodeString(value))
            {
            }
        #endregion
        #region ctor{Integer}
        public Asn1Enum(Int32 value)
            {
            Value = new BigInteger(value);
            State |= ObjectState.Decoded;
            }
        #endregion

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return Value.ToString();
            }

        #region M:BuildContent
        protected override void BuildContent() {
            var InputContent = Value.ToByteArray().Reverse().ToArray();
            length = InputContent.Length;
            content = new ReadOnlyMemoryMappingStream(InputContent);
            size = length + GetHeader().Length;
            }
        #endregion
        #region M:Decode:Boolean
        protected override Boolean Decode()
            {
            if (IsDecoded) { return true; }
            if (IsIndefiniteLength) { return false; }
            var r = new Byte[Length];
            Content.Read(r, 0, r.Length);
            #if NET35
            Value = r.Reverse().ToArray();
            #else
            Value = new BigInteger(r.Reverse().ToArray());
            #endif
            State |= ObjectState.Decoded;
            return true;
            }
        #endregion
        }

    /// <summary>
    /// Represents a <see langword="ENUMERATED"/> type.
    /// </summary>
    public sealed class Asn1Enum<E> : Asn1UniversalObject
        where E: struct, Enum
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Enum"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Enum; }}
        public E Value { get;private set; }

        #region ctor{E}
        public Asn1Enum(E value)
            {
            Value = value;
            State |= ObjectState.Decoded;
            }
        #endregion

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return Value.ToString();
            }

        #region M:BuildContent
        protected override unsafe void BuildContent() {
            var Type = Enum.GetUnderlyingType(typeof(E));
            var EnumSize = Marshal.SizeOf(Type);
            var SourceContent = new Byte[EnumSize];
            var SourceValue = ((IConvertible)Value).ToType(Type,null);
            fixed (Byte* TargetValuePtr = SourceContent) {
                Marshal.StructureToPtr(SourceValue,(IntPtr)TargetValuePtr,false);
                var TargetValue = new BigInteger(SourceContent);
                var TargetContent = TargetValue.ToByteArray().Reverse().ToArray();
                length = TargetContent.Length;
                content = new ReadOnlyMemoryMappingStream(TargetContent);
                size = length + GetHeader().Length;
                }
            }
        #endregion
        #region M:Decode:Boolean
        protected override Boolean Decode()
            {
            throw new NotSupportedException();
            }
        #endregion
        }
    }