﻿using System;
using System.Linq;
using System.Numerics;
using BinaryStudio.IO;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="INTEGER"/> type.
    /// </summary>
    public sealed class Asn1Integer : Asn1UniversalObject,IConvertible
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.Integer"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.Integer; }}
        #if NET35
        public Byte[] Value { get;private set; }
        #else
        public BigInteger Value { get;private set; }
        #endif

        #region ctor
        internal Asn1Integer()
            {
            }
        #endregion
        #region ctor{Byte[]}
        public Asn1Integer(Byte[] value)
            {
            Value = new BigInteger(value.Reverse().ToArray());
            State |= ObjectState.Decoded;
            }
        #endregion
        #region ctor{String}
        public Asn1Integer(String value)
            :this(DecodeString(value))
            {
            }
        #endregion
        #region ctor{Integer}
        public Asn1Integer(Int32 value)
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

        #if NET35
        public static unsafe implicit operator Int32(Asn1Integer source) {
            fixed (Byte* r = source.Value) {
                return *(Int32*)r;
                }
            }
        public static implicit operator Byte[](Asn1Integer source) { return source.Value; }
        #else
        public static implicit operator Int32(Asn1Integer source) { return (Int32)source.Value; }
        public static implicit operator BigInteger(Asn1Integer source) { return source.Value; }
        #endif

        #region M:IConvertible.GetTypeCode:TypeCode
        /// <summary>Returns the <see cref="T:System.TypeCode" /> for this instance.</summary>
        /// <returns>The enumerated constant that is the <see cref="T:System.TypeCode" /> of the class or value type that implements this interface.</returns>
        /// <filterpriority>2</filterpriority>
        TypeCode IConvertible.GetTypeCode()
            {
            return System.TypeCode.Object;
            }
        #endregion
        #region M:IConvertible.ToBoolean(IFormatProvider):Boolean
        /// <summary>Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.</summary>
        /// <returns>A Boolean value equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        Boolean IConvertible.ToBoolean(IFormatProvider provider)
            {
            return ToUInt64(provider) != 0;
            }
        #endregion
        #region M:IConvertible.ToChar(IFormatProvider):Char
        /// <summary>Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.</summary>
        /// <returns>A Unicode character equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        Char IConvertible.ToChar(IFormatProvider provider)
            {
            #if NET35
            return ((IConvertible)(Value[0])).ToChar(provider);
            #else
            return ((IConvertible)(UInt16)Value).ToChar(provider);
            #endif
            }
        #endregion
        #region M:IConvertible.ToSByte(IFormatProvider):SByte
        /// <summary>Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.</summary>
        /// <returns>An 8-bit signed integer equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        SByte IConvertible.ToSByte(IFormatProvider provider)
            {
            #if NET35
            return (SByte)Value[0];
            #else
            return (SByte)Value;
            #endif
            }
        #endregion
        #region M:IConvertible.ToByte(IFormatProvider):Byte
        /// <summary>Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.</summary>
        /// <returns>An 8-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        Byte IConvertible.ToByte(IFormatProvider provider)
            {
            #if NET35
            return (Byte)Value[0];
            #else
            return (Byte)Value;
            #endif
            }
        #endregion
        #region M:IConvertible.ToInt16(IFormatProvider):Int16
        /// <summary>Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.</summary>
        /// <returns>An 16-bit signed integer equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        unsafe Int16 IConvertible.ToInt16(IFormatProvider provider)
            {
            #if NET35
            fixed (Byte* r = Value)
                {
                return *(Int16*)r;
                }
            #else
            return (Int16)Value;
            #endif
            }
        #endregion
        #region M:IConvertible.ToUInt16(IFormatProvider):UInt16
        /// <summary>Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.</summary>
        /// <returns>An 16-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        unsafe UInt16 IConvertible.ToUInt16(IFormatProvider provider)
            {
            #if NET35
            fixed (Byte* r = Value)
                {
                return *(UInt16*)r;
                }
            #else
            return (UInt16)Value;
            #endif
            }
        #endregion
        #region M:IConvertible.ToInt32(IFormatProvider):Int32
        /// <summary>Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.</summary>
        /// <returns>An 32-bit signed integer equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        Int32 IConvertible.ToInt32(IFormatProvider provider)
            {
            return ToInt32();
            }
        public unsafe Int32 ToInt32()
            {
            #if NET35
            fixed (Byte* r = Value)
                {
                return *(Int32*)r;
                }
            #else
            return (Int32)Value;
            #endif
            }
        #endregion
        #region M:IConvertible.ToUInt32(IFormatProvider):UInt32
        /// <summary>Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.</summary>
        /// <returns>An 32-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        public unsafe UInt32 ToUInt32(IFormatProvider provider)
            {
            #if NET35
            fixed (Byte* r = Value)
                {
                return *(UInt32*)r;
                }
            #else
            return (UInt32)Value;
            #endif
            }
        #endregion
        #region M:IConvertible.ToInt64(IFormatProvider)
        /// <summary>Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.</summary>
        /// <returns>An 64-bit signed integer equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        unsafe Int64 IConvertible.ToInt64(IFormatProvider provider)
            {
            #if NET35
            fixed (Byte* r = Value)
                {
                return *(Int64*)r;
                }
            #else
            return (Int64)Value;
            #endif
            }
        #endregion
        #region M:IConvertible.ToUInt64(IFormatProvider):UInt64
        /// <summary>Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.</summary>
        /// <returns>An 64-bit unsigned integer equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        public unsafe UInt64 ToUInt64(IFormatProvider provider) {
            #if NET35
            fixed (Byte* r = Value)
                {
                return *(UInt64*)r;
                }
            #else
            return (UInt64)Value;
            #endif
            }
        #endregion
        #region M:IConvertible.ToSingle(IFormatProvider):Single
        /// <summary>Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.</summary>
        /// <returns>A single-precision floating-point number equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        Single IConvertible.ToSingle(IFormatProvider provider)
            {
            return ((IConvertible)this).ToInt64(provider);
            }
        #endregion
        #region M:IConvertible.ToDouble(IFormatProvider):Double
        /// <summary>Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.</summary>
        /// <returns>A double-precision floating-point number equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        Double IConvertible.ToDouble(IFormatProvider provider)
            {
            return ((IConvertible)this).ToInt64(provider);
            }
        #endregion
        #region M:IConvertible.ToDecimal(IFormatProvider):Decimal
        /// <summary>Converts the value of this instance to an equivalent <see cref="T:System.Decimal" /> number using the specified culture-specific formatting information.</summary>
        /// <returns>A <see cref="T:System.Decimal" /> number equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        Decimal IConvertible.ToDecimal(IFormatProvider provider)
            {
            return ((IConvertible)this).ToInt64(provider);
            }
        #endregion
        #region M:IConvertible.ToDateTime(IFormatProvider):DateTime
        /// <summary>Converts the value of this instance to an equivalent <see cref="T:System.DateTime" /> using the specified culture-specific formatting information.</summary>
        /// <returns>A <see cref="T:System.DateTime" /> instance equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            {
            return new DateTime(((IConvertible)this).ToInt64(provider));
            }
        #endregion
        #region M:IConvertible.ToString(IFormatProvider):String
        /// <summary>Converts the value of this instance to an equivalent <see cref="T:System.String" /> using the specified culture-specific formatting information.</summary>
        /// <returns>A <see cref="T:System.String" /> instance equivalent to the value of this instance.</returns>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        String IConvertible.ToString(IFormatProvider provider)
            {
            return ToString();
            }
        #endregion
        #region M:IConvertible.ToType(Type,IFormatProvider):Object
        /// <summary>Converts the value of this instance to an <see cref="T:System.Object" /> of the specified <see cref="T:System.Type" /> that has an equivalent value, using the specified culture-specific formatting information.</summary>
        /// <returns>An <see cref="T:System.Object" /> instance of type <paramref name="type" /> whose value is equivalent to the value of this instance.</returns>
        /// <param name="type">The <see cref="T:System.Type" /> to which the value of this instance is converted. </param>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information. </param>
        /// <filterpriority>2</filterpriority>
        Object IConvertible.ToType(Type type, IFormatProvider provider)
            {
             return DefaultToType(this, type, provider);
            }
        #endregion

        internal static Object DefaultToType(IConvertible value, Type type, IFormatProvider provider)
            {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (value.GetType() == type) { return value; }
            if (type == typeof(Boolean))  { return value.ToBoolean(provider);  }
            if (type == typeof(Char))     { return value.ToChar(provider);     }
            if (type == typeof(SByte))    { return value.ToSByte(provider);    }
            if (type == typeof(Byte))     { return value.ToByte(provider);     }
            if (type == typeof(Int16))    { return value.ToInt16(provider);    }
            if (type == typeof(UInt16))   { return value.ToUInt16(provider);   }
            if (type == typeof(Int32))    { return value.ToInt32(provider);    }
            if (type == typeof(UInt32))   { return value.ToUInt32(provider);   }
            if (type == typeof(Int64))    { return value.ToInt64(provider);    }
            if (type == typeof(UInt64))   { return value.ToUInt64(provider);   }
            if (type == typeof(Single))   { return value.ToSingle(provider);   }
            if (type == typeof(Double))   { return value.ToDouble(provider);   }
            if (type == typeof(Decimal))  { return value.ToDecimal(provider);  }
            if (type == typeof(DateTime)) { return value.ToDateTime(provider); }
            if (type == typeof(String))   { return value.ToString(provider);   }
            if (type == typeof(Object))   { return value;                      }
            if (type == typeof(Enum))     { return (Enum)value;                }
            throw new InvalidCastException();
            }

        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Type), TypeCode);
                if (Offset >= 0) { writer.WriteValue(nameof(Offset), Offset); }
                writer.WriteValue(nameof(Value), ToString());
                }
            }
        #endregion
        }
    }