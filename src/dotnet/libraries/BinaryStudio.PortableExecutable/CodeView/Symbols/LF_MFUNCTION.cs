using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [UsedImplicitly]
    [CodeViewLeafIndex(LEAF_ENUM.LF_MFUNCTION_16)]
    internal class LF_MFUNCTION_16 : CodeViewTypeInfo
        {
        private enum CALLTYPE : byte
            {
            NEAR_C        =  0,
            FAR_C         =  1,
            NEAR_PASCAL   =  2,
            FAR_PASCAL    =  3,
            NEAR_FASTCALL =  4,
            FAR_FASTCALL  =  5,
            NEAR_STDCALL  =  7,
            FAR_STDCALL   =  8,
            NEAR_SYSCALL  =  9,
            FAR_SYSCALL   = 10,
            THIS_CALL     = 11,
            MIPS_CALL     = 12,
            GENERIC       = 13
            }

        [Flags]
        private enum ATTRIBUTES : byte
            {
            CPP_RETURN_UDT = 0x01,
            INSTANCE_COSTRUCTOR = 0x02,
            INSTANCE_COSTRUCTOR_WITH_VIRTUAL_BASE = 0x04
            }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct HEADER
            {
            private readonly LEAF_ENUM LeafIndex;
            public readonly UInt16 ReturnTypeIndex;
            public readonly UInt16 ClassTypeIndex;
            public readonly UInt16 ThisTypeIndex;
            public readonly ATTRIBUTES Attributes;
            public readonly CALLTYPE CallType;
            public readonly Int16 ParametersNumber;
            }

        private readonly UInt16 ReturnTypeIndex;
        private readonly UInt16 ClassTypeIndex;
        private readonly UInt16 ThisTypeIndex;
        private readonly CALLTYPE CallType;
        private readonly ATTRIBUTES Attributes;
        private readonly Int32 ThisAdjuster;
        private readonly IList<UInt16> Parameters = new List<UInt16>();

        public unsafe LF_MFUNCTION_16(IntPtr Content, Int32 Size)
            : base(Content, Size)
            {
            var r = (HEADER*)Content;
            var e = (Byte*)(r + 1);
            ReturnTypeIndex = r->ReturnTypeIndex;
            ClassTypeIndex  = r->ClassTypeIndex;
            ThisTypeIndex   = r->ThisTypeIndex;
            CallType        = r->CallType;
            Attributes      = r->Attributes;
            for (var i = 0; i < r->ParametersNumber; i++) {
                Parameters.Add(ReadUInt16(ref e));
                }
            ThisAdjuster = ReadInt32(ref e);
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.ScopeObject()) {
                writer.WriteValue(nameof(LeafIndex),LeafIndex);
                writer.WriteValue(nameof(CallType),CallType);
                writer.WriteValue(nameof(Attributes),Attributes);
                writer.WriteValue(nameof(ReturnTypeIndex),ReturnTypeIndex);
                writer.WriteValue(nameof(ClassTypeIndex),ClassTypeIndex);
                writer.WriteValue(nameof(ThisTypeIndex),ThisTypeIndex);
                writer.WriteValue(nameof(ThisAdjuster),ThisAdjuster);
                if (Parameters.Count > 0) {
                    writer.WritePropertyName("Parameters");
                    using (writer.ArrayObject()) {
                        foreach (var e in Parameters) {
                            writer.WriteValue(e.ToString("x4"));
                            }
                        }
                    }
                }
            }

        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            Writer.WriteLine("{0}LeafIndex:{1} CallType:{2}",LinePrefix,LeafIndex,CallType);
            Writer.WriteLine("{0}Attributes:{1}",LinePrefix,Attributes);
            Writer.WriteLine("{0}ReturnType:{1:x4} ClassType:{2:x4} ThisType:{3:x4} ThisAdjuster:{4:x8}",LinePrefix,ReturnTypeIndex,ClassTypeIndex,ThisTypeIndex,ThisAdjuster);
            var i = 0;
            foreach (var index in Parameters) {
                Writer.WriteLine("{0}  {1:x4}:{2:x4}",LinePrefix,i,index);
                i++;
                }
            }
        }
    }