using System;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    public class Relocation
        {
        public Int64 Offset { get; }
        public Int32 SymbolIndex { get; }
        public Int16 Type { get; }
        public IMAGE_FILE_MACHINE Machine { get; }
        public String SymbolName { get; }

        internal unsafe Relocation(CommonObjectFileSource o, IMAGE_FILE_MACHINE machine, IMAGE_RELOCATION* source) {
            Offset = source->VirtualAddress;
            SymbolIndex = source->SymbolTableIndex;
            Type = source->Type;
            Machine = machine;
            if (o.SymbolTable.Count > 0)
                {
                SymbolName = ((Symbol)o.SymbolTable[SymbolIndex]).Name;
                }
            }
        }
    }