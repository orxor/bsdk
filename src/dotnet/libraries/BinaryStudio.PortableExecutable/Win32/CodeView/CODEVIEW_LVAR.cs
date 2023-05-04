using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CODEVIEW_LVAR
        {
        public readonly Int32 Offset;               // first code address where var is live.
        public readonly Int16 Segment;
        public readonly CODEVIEW_LVARFLAGS Flags;   // local var flags.
        }
    }