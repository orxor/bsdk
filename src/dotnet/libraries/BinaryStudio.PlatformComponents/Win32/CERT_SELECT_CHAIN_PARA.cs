using System;
using System.Runtime.InteropServices;
using FILETIME=System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_SELECT_CHAIN_PARA
        {
        public IntPtr hChainEngine;
        public unsafe FILETIME* Time;
        public IntPtr AdditionalStore;
        public unsafe CERT_CHAIN_PARA* ChainPara;
        public Int32 Flags;
        }
    }