using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CERT_CHAIN_POLICY_PARA
        {
        public Int32 Size;
        public CERT_CHAIN_POLICY_FLAGS Flags;
        public IntPtr ExtraPolicyPara;
        }
    }
