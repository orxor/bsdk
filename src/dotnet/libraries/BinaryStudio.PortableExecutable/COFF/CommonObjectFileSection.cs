using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.PortableExecutable
    {
    public class CommonObjectFileSection : IJsonSerializable
        {
        public CommonObjectFileSource CommonObjectFile { get; }
        public Int64 VirtualAddress { get; }
        public Int64 PointerToRelocations { get; }
        public Int64 PointerToLineNumbers { get; }
        public Int32 NumberOfRelocations { get; }
        public Int32 NumberOfLineNumbers { get; }
        public Int32 Index { get; }
        public String Name { get; }
        public IMAGE_SCN Characteristics { get; }
        public IList<Relocation> Relocations { get; }

        internal unsafe CommonObjectFileSection(CommonObjectFileSource o, Byte* baseAddress, IMAGE_SECTION_HEADER* Section)
            {
            CommonObjectFile = o;
            Name = Section->ToString();
            VirtualAddress = Section->VirtualAddress;
            PointerToRelocations = Section->PointerToRawData;
            PointerToLineNumbers = Section->PointerToRelocations;
            NumberOfRelocations = Section->NumberOfRelocations;
            NumberOfLineNumbers = Section->NumberOfLineNumbers;
            Characteristics = Section->Characteristics;
            Relocations = EmptyList<Relocation>.Value;
            if (Section->NumberOfRelocations > 0) {
                var relocations = (IMAGE_RELOCATION*)(Section->PointerToRelocations + baseAddress);
                var r = new List<Relocation>();
                for (var i = 0; i < Section->NumberOfRelocations; i++) {
                    var relocation = &relocations[i];
                    r.Add(new Relocation(o, o.Machine, relocation));
                    }
                Relocations = new ReadOnlyCollection<Relocation>(r);
                }
            }

        public virtual void WriteTo(IJsonWriter writer)
            {
            throw new NotImplementedException();
            }
        }
    }