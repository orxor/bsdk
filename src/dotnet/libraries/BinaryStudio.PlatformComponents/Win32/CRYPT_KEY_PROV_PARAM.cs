﻿using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_KEY_PROV_PARAM
        {
        public  unsafe CRYPT_PARAM ParameterIdentifier;
        public  unsafe Byte*  ParameterData;
        public  unsafe Int32  ParameterDataSize;
        private readonly Int32 Flags;
        }
    }