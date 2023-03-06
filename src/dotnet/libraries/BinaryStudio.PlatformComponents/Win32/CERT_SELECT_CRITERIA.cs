using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_SELECT_CRITERIA
        {
        public Int32 Type;
        public Int32 cPara;
        public unsafe IntPtr* ppPara;
        }
    }