using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;
using Microsoft.Win32.SafeHandles;

namespace BinaryStudio.DirectoryServices
    {
    using HRESULT=HResult;
    public class PathUtils
        {
        private const Int32 MAX_PATH = 260;
        private static Boolean IsMatchPart(String pattern, String value) {
            #if NET35
            if (String.IsNullOrEmpty(pattern)) { return true; }
            #else
            if (String.IsNullOrWhiteSpace(pattern)) { return true; }
            #endif
            if ((pattern == "*") || (pattern == "*.*")) { return true; }
            if (String.Equals(pattern, value, StringComparison.OrdinalIgnoreCase)) { return true; }
            if (pattern.EndsWith  ("*") &&
                pattern.StartsWith("*"))
                {
                return value.Contains(pattern.Trim('*'));
                }
            if (pattern.EndsWith  ("*")) { return value.StartsWith(pattern.TrimEnd('*')); }
            if (pattern.StartsWith("*")) { return value.EndsWith(pattern.TrimStart('*')); }
            return false;
            }

        public static Boolean IsMatch(String pattern, String filename)
            {
            try
                {
                if ((pattern == "*.*") || (pattern == "*")) { return true; }
                var pD = Path.GetDirectoryName(pattern);
                var pF = Path.GetFileNameWithoutExtension(pattern);
                var pE = Path.GetExtension(pattern);
                var iD = Path.GetDirectoryName(filename);
                var iF = Path.GetFileNameWithoutExtension(filename);
                var iE = Path.GetExtension(filename);
                return IsMatchPart(pD,iD)
                    && IsMatchPart(pE,iE)
                    && IsMatchPart(pF,iF);
                }
            catch (Exception e)
                {
                e.Data["Pattern"] = pattern;
                e.Data["FileName"] = filename;
                throw;
                }
            }

        public static Boolean IsMatch(IList<String> patterns, String filename) {
            if (patterns == null) { throw new ArgumentNullException(nameof(patterns)); }
            return patterns.Any(pattern => IsMatch(pattern, filename));
            }

        /// <summary>
        /// Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.
        /// </summary>
        /// <param name="folder">The directory path for the file name.</param>
        /// <returns>The full path of the temporary file.</returns>
        public static String GetTempFileName(String folder) {
            return GetTempFileName(folder, "tmp");
            }

        /// <summary>
        /// Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.
        /// </summary>
        /// <param name="folder">The directory path for the file name.</param>
        /// <param name="prefix">The name prefix.</param>
        /// <returns>The full path of the temporary file.</returns>
        public static String GetTempFileName(String folder, String prefix) {
            var r = new StringBuilder(MAX_PATH);
            GetTempFileName(folder, prefix, 0, r);
            return r.ToString();
            }

        #if !LINUX
        /* <path>\<pre><uuuu>.TMP */
        private static Int32 GetTempFileName(String tmppath,String prefix,Int32 uniqueIdOrZero,[Out] StringBuilder tmpFileName) {
            if (uniqueIdOrZero == 0) { uniqueIdOrZero = (new Random()).Next(UInt16.MaxValue); }
            for (;;) {
                var target = Path.Combine(tmppath,$"{prefix}{uniqueIdOrZero & 0xffff:x4}.tmp");
                var fd = Open(target,OpenFlags.O_CREAT|OpenFlags.O_EXCL,S_IRUSR|S_IWUSR);
                if (fd == -1) {
                    uniqueIdOrZero++;
                    continue;
                    }
                Close(fd);
                break;
                }
            return uniqueIdOrZero;
            }

        [Flags]
        internal enum OpenFlags
            {
            O_RDONLY    = 0x000,
            O_WRONLY    = 0x001,
            O_RDWR      = 0x002,
            O_CLOEXEC   = 0x010,
            O_CREAT     = 0x020,
            O_EXCL      = 0x040,
            O_TRUNC     = 0x080,
            O_SYNC      = 0x100
            }

        private const Int32 S_IRWXU = 0x00700;
        private const Int32 S_IRUSR = 0x00400;
        private const Int32 S_IWUSR = 0x00200;
        private const Int32 S_IXUSR = 0x00100;
        private const Int32 S_IRWXG = 0x00070;
        private const Int32 S_IRGRP = 0x00040;
        private const Int32 S_IWGRP = 0x00020;
        private const Int32 S_IXGRP = 0x00010;
        private const Int32 S_IRWXO = 0x00007;
        private const Int32 S_IROTH = 0x00004;
        private const Int32 S_IWOTH = 0x00002;
        private const Int32 S_IXOTH = 0x00001;

        [DllImport("c", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl, EntryPoint ="open64")] private static extern Int32 Open(String filename, OpenFlags flags, Int32 mode);
        [DllImport("c", CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl, EntryPoint ="close")]  private static extern Int32 Close(Int32 fd);
        #else
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)] private static extern Int32 GetTempFileName(String tmppath,String prefix,Int32 uniqueIdOrZero,[Out] StringBuilder tmpFileName);
        #endif

        public static Boolean IsSame(String x, String y)
            {
            if (String.Equals(x, y)) { return true; }
            if ((x == null) || (y == null)) { return false; }
            if (x.StartsWith(@"\\?\")) { x = x.Substring(4); }
            if (y.StartsWith(@"\\?\")) { y = y.Substring(4); }
            if (String.Equals(x, y)) { return true; }
            return false;
            }

        public static HRESULT SetLastAccessTime(String path, DateTime value)
            {
            try
                {
                File.SetLastAccessTime(path,value);
                return HRESULT.S_OK;
                }
            catch (Exception e)
                {
                return (HRESULT)Marshal.GetHRForException(e);
                }
            }

        public static HRESULT SetLastWriteTime(String path, DateTime value)
            {
            try
                {
                File.SetLastWriteTime(path,value);
                return HRESULT.S_OK;
                }
            catch (Exception e)
                {
                return (HRESULT)Marshal.GetHRForException(e);
                }
            }

        public static HRESULT SetCreationTime(String path, DateTime value)
            {
            try
                {
                File.SetCreationTime(path,value);
                return HRESULT.S_OK;
                }
            catch (Exception e)
                {
                return (HRESULT)Marshal.GetHRForException(e);
                }
            }
        }
    }