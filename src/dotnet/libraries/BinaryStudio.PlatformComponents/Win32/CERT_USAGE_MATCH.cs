using System.Runtime.InteropServices;

namespace BinaryStudio.PlatformComponents.Win32
    {
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CERT_USAGE_MATCH
        {
        public USAGE_MATCH_TYPE  Type;
        public CERT_ENHKEY_USAGE Usage;
        }
    }
