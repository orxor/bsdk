using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryStudio.PlatformComponents.Win32
    {
    public class CRYPT_OID_INFO_DBG_PROXY
        {
        public String Value { get; }
        public CRYPT_OID_INFO_DBG_PROXY(CRYPT_OID_INFO source)
            {
            Value = $"0x{source.Value.ToString("x8")}:{{{(ALG_ID)source.Value}}}";
            }
        }

    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_OID_INFO
        {
        public readonly Int32 Size;
        public readonly IntPtr OID;
        public readonly IntPtr Name;
        public readonly CRYPT_ALG_OID_GROUP_ID GroupId;
        public readonly Int32 Value;
        public readonly CRYPT_BLOB ExtraInfo;
        #if CRYPT_OID_INFO_HAS_EXTRA_FIELDS
        public readonly IntPtr CNGAlgid;
        public readonly IntPtr CNGExtraAlgid;
        #endif

        public override String ToString() {
            if (OID == IntPtr.Zero) { return "{CRYPT_OID_INFO}"; }
            var r = new StringBuilder($"{{{Marshal.PtrToStringAnsi(OID)}}}");
            if (Name != IntPtr.Zero) {
                r.Append(":{");
                r.Append(Marshal.PtrToStringUni(Name));
                r.Append("}");
                }
            return r.ToString();
            }
        }
    }