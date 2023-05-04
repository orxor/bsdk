using BinaryStudio.PortableExecutable.Win32;
using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class MSC06CodeViewSection : CodeViewSection
        {
        public override CV_SIGNATURE Signature { get { return CV_SIGNATURE.CV_SIGNATURE_C6; }}
        public unsafe MSC06CodeViewSection(CommonObjectFileSource o, Byte* BaseAddress, Byte* VirtualAddress, IMAGE_SECTION_HEADER* Section, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory)
            : base(o, BaseAddress, VirtualAddress, Section, ImageDebugDirectory)
            {
            Console.Error.WriteLine($"{Signature}");
            }
        }
    }