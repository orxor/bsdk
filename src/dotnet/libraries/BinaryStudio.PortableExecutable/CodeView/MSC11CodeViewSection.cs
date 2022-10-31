using System;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class MSC11CodeViewSection : CodeViewSection
        {
        public override CV_SIGNATURE Signature { get { return CV_SIGNATURE.CV_SIGNATURE_C11; }}
        public unsafe MSC11CodeViewSection(CommonObjectFileSource o, Byte* BaseAddress, Byte* VirtualAddress, IMAGE_SECTION_HEADER* Section, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory)
            : base(o, BaseAddress, VirtualAddress, Section,ImageDebugDirectory)
            {
            Console.Error.WriteLine($"{Signature}");
            }
        }
    }