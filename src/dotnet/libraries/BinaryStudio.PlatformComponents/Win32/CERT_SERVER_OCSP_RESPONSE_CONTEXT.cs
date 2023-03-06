using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_SERVER_OCSP_RESPONSE_CONTEXT
        {
        public Int32 Size;
        public unsafe Byte* EncodedOcspResponseBytes;
        public Int32 EncodedOcspResponseLength;
        }
    }