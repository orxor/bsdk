using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_EXTENSIONS
        {
        public Int32 ExtensionCount;
        public unsafe CERT_EXTENSION* Extensions;
        }
    }