using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public abstract class CodeViewSection : CommonObjectFileSection
        {
        public abstract CV_SIGNATURE Signature { get; }
        public virtual Encoding Encoding { get { return Encoding.ASCII; }}
        public virtual Boolean IsLengthPrefixedString { get { return true; }}
        public IList<CodeViewPrimarySSection> Sections { get; }
        public CV_CPU_TYPE? CPU { get;internal set; }

        internal unsafe CodeViewSection(CommonObjectFileSource o, Byte* BaseAddress, Byte* VirtualAddress, IMAGE_SECTION_HEADER* Section, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory)
            : base(o, BaseAddress, Section)
            {
            Sections = new List<CodeViewPrimarySSection>();
            CPU = o.CPU;
            try
                {
                var r = BaseAddress + Section->PointerToRawData;
                var sz = (Int32)Section->SizeOfRawData;
                r += sizeof(Int32);
                for (;(*(Int32*)r > 0) && (sz > 4);) {
                    var record = (DEBUG_SSECTION_HEADER*)r;
                    var count = (Int32)Math.Ceiling(record->Length/4.0)*4;
                    var content = (Byte*)(record + 1);
                    Sections.Add(CodeViewPrimarySSection.From(
                        this,
                        (Int32)(content - (BaseAddress + Section->PointerToRawData)),
                        record->Type,
                        record->Length,
                        content
                        ));
                    r  += sizeof(DEBUG_SSECTION_HEADER) + count;
                    sz -= sizeof(DEBUG_SSECTION_HEADER) + count;
                    }
                Sections = new List<CodeViewPrimarySSection>(Sections);
                var H = Sections.OfType<CodeViewFileHashTableSSection>().FirstOrDefault();
                if (H != null) {
                    var S = Sections.OfType<CodeViewStringTableSSection>().FirstOrDefault();
                    if (S != null) {
                        foreach (var hashvalue in H.Values) {
                            hashvalue.FileName = S.Values[hashvalue.FileNameOffset];
                            }
                        }
                    }
                }
            catch
                {
                Sections = EmptyList<CodeViewPrimarySSection>.Value;
                }
            var BegOfDebugData = VirtualAddress + ((ImageDebugDirectory->AddressOfRawData == 0) ? ImageDebugDirectory->PointerToRawData : ImageDebugDirectory->AddressOfRawData);
            var EndOfDebugData = BegOfDebugData + ImageDebugDirectory->SizeOfData;
            var Header = (OMFDirectorySignatureHeader*)BegOfDebugData;
            var Signature = Header->Signature;
            foreach (var type in typeof(OMFDirectory).Assembly.GetTypes()) {
                var key = type.GetCustomAttributes(false).OfType<OMFDirectorySignatureAttribute>().FirstOrDefault();
                if (key != null) {
                    if (key.Signature == Signature) {
                        var Directory = (OMFDirectory)Activator.CreateInstance(type,
                            (IntPtr)BaseAddress,
                            (IntPtr)BegOfDebugData,
                            (IntPtr)EndOfDebugData);
                        Directory.CPU = CPU;
                        Directory.Analyze();
                        using (var writer = new StreamWriter(File.Create("my.dump")))
                            {
                            Directory.WriteTo(writer,String.Empty,0);
                            }
                        break;
                        }
                    }
                }
            Header = (OMFDirectorySignatureHeader*)(EndOfDebugData - sizeof(OMFDirectorySignatureHeader));
            if (Header->Signature != Signature) { throw new InvalidDataException(); }
            }

        public override void WriteTo(IJsonWriter writer)
            {
            }
        }
    }