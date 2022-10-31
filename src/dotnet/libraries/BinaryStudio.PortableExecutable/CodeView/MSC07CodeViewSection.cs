using System;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class MSC07CodeViewSection : CodeViewSection
        {
        public override CV_SIGNATURE Signature { get { return CV_SIGNATURE.CV_SIGNATURE_C7; }}
        public unsafe MSC07CodeViewSection(CommonObjectFileSource o, Byte* BaseAddress, Byte* VirtualAddress, IMAGE_SECTION_HEADER* Section, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory)
            : base(o, BaseAddress, VirtualAddress, Section,ImageDebugDirectory)
            {
            Console.Error.WriteLine($"{Signature}");
            }
        }
    }