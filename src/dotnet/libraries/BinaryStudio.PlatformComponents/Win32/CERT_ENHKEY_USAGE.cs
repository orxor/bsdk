﻿using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CERT_ENHKEY_USAGE
        {
        public Int32  UsageIdentifierCount;
        public IntPtr UsageIdentifierArray;
        }
    }
