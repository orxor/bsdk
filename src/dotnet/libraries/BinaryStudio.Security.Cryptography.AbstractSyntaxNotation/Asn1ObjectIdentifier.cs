﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using BinaryStudio.IO;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    /// <summary>
    /// Represents a <see langword="OBJECT IDENTIFIER"/> type.
    /// </summary>
    public class Asn1ObjectIdentifier : Asn1UniversalObject,IEquatable<String>,IComparable<Asn1ObjectIdentifier>
        {
        /// <summary>
        /// ASN.1 universal type. Always returns <see cref="Asn1ObjectType.ObjectIdentifier"/>.
        /// </summary>
        public override Asn1ObjectType Type { get { return Asn1ObjectType.ObjectIdentifier; }}
        public Int64[] Sequence { get;private set; }
        public String FriendlyName { get {
            var value = ToString();
            var r = OID.ResourceManager.GetString(value,CultureInfo.InvariantCulture);
            #if NET35
            return (!String.IsNullOrEmpty(r))
                    ? r
                    : (new Oid(value)).FriendlyName;
            #else
            return (!String.IsNullOrWhiteSpace(r))
                    ? r
                    : (new Oid(value)).FriendlyName;
            #endif
            }}

        #region ctor
        internal Asn1ObjectIdentifier()
            {
            }
        #endregion
        #region ctor{String}
        public Asn1ObjectIdentifier(String source)
            {
            #if NET35
            if (String.IsNullOrEmpty(source)) { throw new ArgumentOutOfRangeException(nameof(source)); }
            #else
            if (String.IsNullOrWhiteSpace(source)) { throw new ArgumentOutOfRangeException(nameof(source)); }
            #endif
            Sequence = source.Split('.').Select(Int64.Parse).ToArray();
            State |= ObjectState.Decoded;
            }
        #endregion
        #region ctor{Oid}
        public Asn1ObjectIdentifier(Oid source)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Sequence = source.Value.Split('.').Select(Int64.Parse).ToArray();
            State |= ObjectState.Decoded;
            }
        #endregion

        #region M:CreateSequence(Byte[]):Int64[]
        private static Int64[] CreateSequence(Byte[] source) {
            var j = 0L;
            var values = new List<Int64>();
            for (var i = 0; i < source.LongLength; ++i) {
                var c = source[i];
                if (i == 0) {
                    if (c < 40) {
                        values.Add(0);
                        values.Add(c);
                        }
                    else if (c < 80)
                        {
                        values.Add(1);
                        values.Add(c - 40);
                        }
                    else
                        {
                        values.Add(2);
                        values.Add(c - 80);
                        }
                    }
                else
                    {
                    if (c < 128) {
                        if (j == 0) { values.Add(c); }
                        else
                            {
                            values.Add(c + j);
                            j  = 0;
                            }
                        }
                    else
                        {
                        var K = c & 0x7F;
                        j = (j == 0)
                            ? (K) * 128
                            : (K + j) * 128;
                        }
                    }
                }
            return values.ToArray();
            }
        #endregion
        #region M:ToString:String
        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            #if NET35
            return IsDecoded
                ? String.Join(".", Sequence.Select(i => i.ToString()).ToArray())
                : base.ToString();
            #else
            return IsDecoded
                ? String.Join(".", Sequence)
                : base.ToString();
            #endif
            }
        #endregion
        #region M:BuildContent
        protected override void BuildContent() {
            var InputContent = BuildContent(Sequence);
            length = InputContent.Length;
            content = new ReadOnlyMemoryMappingStream(InputContent);
            size = length + GetHeader().Length;
            }
        #endregion
        #region M:BuildContent(Int64[]):Byte[]
        private static Byte[] BuildContent(Int64[] sequence)
            {
            var i = 0;
            var r = new List<Byte>();
            var buffer = new Byte[256];
            while (i < sequence.Length) {
                var c = sequence[i];
                if (i == 0) {
                    if (c == 0) {
                        r.Add((Byte)sequence[1]);
                        i++;
                        }
                    else if (c == 1)
                        {
                        r.Add((Byte)(sequence[1] + 40));
                        i++;
                        }
                    else
                        {
                        r.Add((Byte)(sequence[1] + 80));
                        i++;
                        }
                    }
                else
                    {
                    if (c < 128)
                        {
                        r.Add((Byte)c);
                        }
                    else
                        {
                        var j = 0;
                        while (c >= 128) {
                            buffer[j] = (Byte)(c & 0x7F);
                            j++;
                            c >>= 7;
                            }
                        buffer[j] = (Byte)c;
                        var n = j;
                        for (j = n; j >= 1; j--)
                            {
                            r.Add((Byte)(buffer[j] | 0x80));
                            }
                        r.Add(buffer[0]);
                        }
                    }
                i++;
                }
            return r.ToArray();
            }
        #endregion

        public static implicit operator Oid(Asn1ObjectIdentifier source) { return (Oid)source.GetService(typeof(Oid)); }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="service"/>.-or- null if there is no service object of type <paramref name="service"/>.</returns>
        /// <filterpriority>2</filterpriority>
        public override Object GetService(Type service) {
            if (service == typeof(Asn1ObjectIdentifier)) { return this; }
            if (service == typeof(Oid)) { return new Oid(ToString(),FriendlyName); }
            return base.GetService(service);
            }

        public Int32 CompareTo(Asn1ObjectIdentifier other)
            {
            if (other == null) { return +1; }
            return ToString().CompareTo(other.ToString());
            }

        /**
         * <summary>Indicates whether the current object is equal to another string object.</summary>
         * <param name="key">An object to compare with this object.</param>
         * <returns>true if the current object is equal to the <paramref name="key"/> parameter; otherwise, false.</returns>
         * */
        public Boolean Equals(String key) {
            return (Equals(ToString(), key));
            }

        /**
         * <summary>Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Object"/>.</summary>
         * <returns>true if the specified <see cref="Object"/> is equal to the current <see cref="Object"/>; otherwise, false.</returns>
         * <param name="other">The object to compare with the current object.</param>
         * <filterpriority>2</filterpriority>
         * */
        public override Boolean Equals(Object other)
            {
            if (ReferenceEquals(this, other)) { return true; }
            if (other is String) { return Equals((String)other); }
            return base.Equals(other);
            }

        /**
         * <summary>Serves as a hash function for a particular type.</summary>
         * <returns>A hash code for the current <see cref="Object"/>.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override Int32 GetHashCode()
            {
            return ToString().GetHashCode();
            }

        protected override Boolean Decode()
            {
            if (IsDecoded) { return true; }
            if (IsIndefiniteLength) { return false; }
            Content.Seek(0, SeekOrigin.Begin);
            var r = new Byte[Length];
            Content.Read(r, 0, r.Length);
            Sequence = CreateSequence(r);
            State |= ObjectState.Decoded;
            return true;
            }

        #region M:WriteTo(IJsonWriter)
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Type), TypeCode);
                if (Offset >= 0) { writer.WriteValue(nameof(Offset), Offset); }
                writer.WriteValue("Value", ToString());
                }
            }
        #endregion
        }
    }