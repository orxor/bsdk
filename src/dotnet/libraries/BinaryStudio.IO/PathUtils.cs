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

        #if LINUX
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
                tmpFileName.Append(target);
                break;
                }
            return uniqueIdOrZero;
            }

        [Flags]
        internal enum OpenFlags
            {
            O_RDONLY    = 0x000000,
            O_WRONLY    = 0x000001,
            O_RDWR      = 0x000002,
            O_CREAT     = 0x000040,
            O_EXCL      = 0x000080,
            O_NOCTTY    = 0x000100,
            O_TRUNC     = 0x000200,
            O_APPEND    = 0x000400,
            O_NONBLOCK  = 0x000800,
            O_SYNC      = 0x101000,
            O_ASYNC     = 0x002000,
            O_LARGEFILE = 0x008000,
            O_DIRECTORY = 0x010000,
            O_NOFOLLOW  = 0x020000,
            O_CLOEXEC   = 0x080000,
            O_DIRECT    = 0x004000,
            O_NOATIME   = 0x040000,
            O_PATH      = 0x200000,
            O_DSYNC     = 0x001000,
            O_TMPFILE   = (0x400000 | O_DIRECTORY),
            O_NDELAY    = O_NONBLOCK
            }

        private const Int32 __S_ISUID  = 0x0800; /* Set user ID on execution.              */
        private const Int32 __S_ISGID  = 0x0400; /* Set group ID on execution.             */
        private const Int32 __S_ISVTX  = 0x0200; /* Save swapped text after use (sticky).  */
        private const Int32 __S_IREAD  = 0x0100; /* Read by owner.                         */
        private const Int32 __S_IWRITE = 0x0080; /* Write by owner.                        */
        private const Int32 __S_IEXEC  = 0x0040; /* Execute by owner.                      */
        private const Int32 S_IRUSR    = __S_IREAD;       /* Read by owner.     */
        private const Int32 S_IWUSR    = __S_IWRITE;      /* Write by owner.    */
        private const Int32 S_IXUSR    = __S_IEXEC;       /* Execute by owner.  */
        private const Int32 S_IRGRP    = (S_IRUSR >> 3);  /* Read by group.     */
        private const Int32 S_IWGRP    = (S_IWUSR >> 3);  /* Write by group.    */
        private const Int32 S_IXGRP    = (S_IXUSR >> 3);  /* Execute by group.  */
        private const Int32 S_IROTH    = (S_IRGRP >> 3);  /* Read by others.    */
        private const Int32 S_IWOTH    = (S_IWGRP >> 3);  /* Write by others.   */
        private const Int32 S_IXOTH    = (S_IXGRP >> 3);  /* Execute by others. */
        private const Int32 S_IRWXO    = (S_IRWXG >> 3);
        private const Int32 S_IRWXU    = (__S_IREAD|__S_IWRITE|__S_IEXEC);
        private const Int32 S_IRWXG    = (S_IRWXU >> 3);

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