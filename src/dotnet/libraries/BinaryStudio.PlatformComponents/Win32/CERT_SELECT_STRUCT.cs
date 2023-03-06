using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_SELECT_STRUCT_A
        {
        public Int32 Size;
        public IntPtr Parent;
        public IntPtr Instance;
        public IntPtr TemplateName;
        public Int32 Flags;
        public IntPtr Title;
        public Int32 CertStoreCount;
        public unsafe IntPtr* CertStoreArray;
        public IntPtr PurposeOid;
        public Int32 CertContextCount;
        public unsafe CERT_CONTEXT* CertContextArray;
        public IntPtr CustData;
        public IntPtr Hook;
        public IntPtr Filter;
        public IntPtr HelpFileName;
        public Int32 HelpId;
        public IntPtr Provider;
        }
    }