using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    public unsafe delegate void CERT_SERVER_OCSP_RESPONSE_UPDATE_CALLBACK(CERT_CHAIN_CONTEXT* ChainContext,CERT_SERVER_OCSP_RESPONSE_CONTEXT* ServerOcspResponseContext,CRL_CONTEXT* NewCrlContext,CRL_CONTEXT* PrevCrlContext,IntPtr Arg,Int32 WriteOcspFileError);
    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_SERVER_OCSP_RESPONSE_OPEN_PARA
        {
        public Int32 Size;
        public Int32 Flags;
        public Int32 UsedSize;
        public IntPtr OcspDirectory;
        public IntPtr UpdateCallback;    //CERT_SERVER_OCSP_RESPONSE_UPDATE_CALLBACK
        public IntPtr UpdateCallbackArg;
        }
    }