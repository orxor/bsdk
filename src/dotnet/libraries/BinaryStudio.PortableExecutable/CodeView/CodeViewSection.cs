using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        internal unsafe CodeViewSection(CommonObjectFileSource o, Byte* BaseAddress, Byte* VirtualAddress, IMAGE_SECTION_HEADER* Section, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory)
            : base(o, BaseAddress, Section)
            {
            Sections = new List<CodeViewPrimarySSection>();
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
            var BegOfDebugData = VirtualAddress + ImageDebugDirectory->AddressOfRawData;
            var Header = (CodeViewDirectoryHeader*)BegOfDebugData;
            switch (Header->Signature) {
                case CodeViewDirectorySignature.NB00:
                case CodeViewDirectorySignature.NB01:
                case CodeViewDirectorySignature.NB02:
                case CodeViewDirectorySignature.NB03:
                case CodeViewDirectorySignature.NB04:
                case CodeViewDirectorySignature.NB05:
                case CodeViewDirectorySignature.NB06:
                case CodeViewDirectorySignature.NB07:
                case CodeViewDirectorySignature.NB08:
                case CodeViewDirectorySignature.NB09:
                case CodeViewDirectorySignature.FB09:
                case CodeViewDirectorySignature.FB0A:
                    {

                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
                }
            }

        public override void WriteTo(IJsonWriter writer)
            {
            }
        }
    }