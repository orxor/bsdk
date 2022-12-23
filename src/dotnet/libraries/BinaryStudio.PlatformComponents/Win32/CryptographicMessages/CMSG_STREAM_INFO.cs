using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    public unsafe delegate bool PFN_CMSG_STREAM_OUTPUT(IntPtr arg, byte* data, uint size, bool final);
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CMSG_STREAM_INFO
        {
        public uint Content;
        public PFN_CMSG_STREAM_OUTPUT StreamOutput;
        public IntPtr Arg;

        public CMSG_STREAM_INFO(uint content, PFN_CMSG_STREAM_OUTPUT streamoutput, IntPtr arg)
            {
            Content = content;
            StreamOutput = streamoutput;
            Arg = arg;
            }

        public CMSG_STREAM_INFO(uint content, PFN_CMSG_STREAM_OUTPUT streamoutput)
            {
            Content = content;
            StreamOutput = streamoutput;
            Arg = IntPtr.Zero;
            }
        }
    }
