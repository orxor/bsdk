using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace BinaryStudio.Security.Cryptography.Specific.CryptoProCSP
    {
    public class CryptoProCSPRNGSource
        {
        public String LongName { get; }
        public String ShortName { get; }
        public String Description { get; }
        public Int32 Order { get; }

        #if NET5_0
        #else
        internal CryptoProCSPRNGSource(RegistryKey source, String folder, Int32 order) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Order = order;
            var module = LoadLibrary(Path.Combine(folder,(String)source.GetValue("DLL")));
            try
                {
                LongName = LoadString(module,4096);
                ShortName = LoadString(module,4097);
                Description = LoadString(module,4098).TrimEnd('.');
                }
            finally
                {
                }
            }
        #endif

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr LoadLibrary(String filename);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern Int32 LoadString(IntPtr Module,UInt32 Identifier,StringBuilder FileName, Int32 Length);

        private static String LoadString(IntPtr Module,UInt32 Identifier) {
            var o = new StringBuilder(260);
            while (true) {
                var r = LoadString(Module, Identifier, o, o.Capacity);
                if (r == 0)
                    {
                    return null;
                    }
                if (r <= o.Capacity)
                    {
                    break;
                    }
                o.EnsureCapacity(o.Capacity * 2);
                }
            return o.ToString();
            }

        public override String ToString()
            {
            return Description;
            }
        }
    }