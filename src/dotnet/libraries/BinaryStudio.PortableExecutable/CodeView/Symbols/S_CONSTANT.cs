using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using JetBrains.Annotations;

// ReSharper disable VirtualMemberCallInConstructor

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_CONSTANT)]
    internal class S_CONSTANT : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_CONSTANT; }}
        public LEAF_ENUM? FieldValueType { get; }
        public Int16 FieldTypeIndex { get; }
        public String FieldName { get; }
        public Byte[] FieldValue { get; }

        public unsafe S_CONSTANT(CodeViewSymbolsSSection section, Int32 offset, IntPtr content, Int32 length)
            : base(section, offset, content, length)
            {
            FieldValue = EmptyArray<Byte>.Value;
            var r = (DEBUG_S_CONSTANT_HEADER*)content;
            FieldTypeIndex = r->FieldTypeIndex;
            var leaftype = &(r->FieldValue);
            var value = (Byte*)(leaftype + 1);
            if (*leaftype < LEAF_ENUM.LF_NUMERIC) {
                FieldValue = ToArray((Byte*)leaftype, sizeof(Int16));
                }
            else
                {
                FieldValueType = *leaftype;
                switch (*leaftype) {
                    case LEAF_ENUM.LF_CHAR:
                        {
                        FieldValue = ToArray((Byte*)value, 1);
                        }
                        break;
                    case LEAF_ENUM.LF_SHORT:
                    case LEAF_ENUM.LF_USHORT:
                        {
                        FieldValue = ToArray((Byte*)value, 2);
                        }
                        break;
                    case LEAF_ENUM.LF_LONG:
                    case LEAF_ENUM.LF_ULONG:
                    case LEAF_ENUM.LF_REAL32:
                        {
                        FieldValue = ToArray((Byte*)value, 4);
                        }
                        break;
                    case LEAF_ENUM.LF_REAL48:
                        {
                        FieldValue = ToArray((Byte*)value, 6);
                        }
                        break;
                    case LEAF_ENUM.LF_REAL64:
                    case LEAF_ENUM.LF_DATE:
                    case LEAF_ENUM.LF_QUADWORD:
                    case LEAF_ENUM.LF_UQUADWORD:
                    case LEAF_ENUM.LF_COMPLEX32:
                        {
                        FieldValue = ToArray((Byte*)value, 8);
                        }
                        break;
                    case LEAF_ENUM.LF_REAL80:
                        {
                        FieldValue = ToArray((Byte*)value, 10);
                        }
                        break;
                    case LEAF_ENUM.LF_OCTWORD:
                    case LEAF_ENUM.LF_UOCTWORD:
                    case LEAF_ENUM.LF_DECIMAL:
                    case LEAF_ENUM.LF_REAL128:
                    case LEAF_ENUM.LF_COMPLEX64:
                        {
                        FieldValue = ToArray((Byte*)value, 16);
                        }
                        break;
                    case LEAF_ENUM.LF_COMPLEX80:
                        {
                        FieldValue = ToArray((Byte*)value, 20);
                        }
                        break;
                    case LEAF_ENUM.LF_COMPLEX128:
                        {
                        FieldValue = ToArray((Byte*)value, 32);
                        }
                        break;
                    //case LEAF_ENUM.LF_VARSTRING:
                    //    {
                    //    FieldValueSize = *(UInt16*)value;
                    //    }
                    //    break;
                    default: { throw new NotSupportedException(); }
                    }
                value += FieldValue.Length;
                }
            var bytes = new List<Byte>();
            while (*value != 0) {
                bytes.Add(*value);
                value++;
                }
            FieldName = Encoding.UTF8.GetString(bytes.ToArray()).Trim();
            FieldTypeIndexLength = Math.Max(FieldTypeIndexLength, FieldTypeIndex.ToString().Length);
            FieldValueTypeLength = Math.Max(FieldValueTypeLength, FieldValueType.ToString().Length);
            FieldNameLength = Math.Max(FieldNameLength, FieldName.Length);
            return;
            }

        private static Int32 FieldTypeIndexLength,FieldValueTypeLength,FieldNameLength;
        }

    [CodeViewSymbol(DEBUG_SYMBOL_INDEX.S_CONSTANT16)]
    [UsedImplicitly]
    internal class S_CONSTANT16 : CodeViewSymbol
        {
        public override DEBUG_SYMBOL_INDEX Type { get { return DEBUG_SYMBOL_INDEX.S_CONSTANT16; }}
        public LEAF_ENUM FieldValueType { get; }
        public Int16 FieldTypeIndex { get; }
        public String FieldName { get; }
        public Byte[] FieldValue { get; }

        public unsafe S_CONSTANT16(CodeViewSymbolsSSection Section, Int32 Offset, IntPtr Content, Int32 Length)
            : base(Section, Offset, Content, Length)
            {
            FieldValue = EmptyArray<Byte>.Value;
            var r = (DEBUG_S_CONSTANT_HEADER*)Content;
            FieldTypeIndex = r->FieldTypeIndex;
            var LeafType = &(r->FieldValue);
            FieldValueType = *LeafType;
            var FieldNamePointer = (Byte*)(LeafType + 1);
            switch (FieldValueType) {
                case LEAF_ENUM.LF_CHAR:
                    FieldNamePointer++;
                    break;
                }
            FieldName = ToString(Encoding,FieldNamePointer,IsLengthPrefixedString);
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="DumpFlags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags DumpFlags) {
            Writer.WriteLine("{0}Offset:{1:x8} Type:{2} TypeIndex:{3:x4} ValueType:{4} Name:{5}",
                LinePrefix,Offset,Type,FieldTypeIndex,FieldValueType,FieldName);
            }
        }
    }