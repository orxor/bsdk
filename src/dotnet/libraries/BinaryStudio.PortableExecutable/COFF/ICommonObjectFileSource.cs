using System;
using System.Runtime.InteropServices;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.PortableExecutable
    {
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport, Guid("2f857d78-03a7-40fe-9500-cf2d2f235b4c")]
    public interface ICommonObjectFileSource
        {
        [PreserveSig] HRESULT Load([MarshalAs(UnmanagedType.SafeArray)] IntPtr[] Source,Int64 Size);
        }
    }