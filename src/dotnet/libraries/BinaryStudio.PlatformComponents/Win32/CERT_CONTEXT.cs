﻿using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CERT_CONTEXT
        {
        public readonly UInt32 CertEncodingType;
        public readonly unsafe Byte *CertEncoded;
        public readonly Int32  CertEncodedSize;
        public readonly unsafe CERT_INFO* CertInfo;
        public readonly IntPtr CertStore;
        }
    }