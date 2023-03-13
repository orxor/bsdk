using BinaryStudio.Security.Cryptography;
using System;
#if !NET35
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryStudio.PlatformComponents.Win32
    {
    public class HResultException : COMException
        {
        public HResultException(Int32 code, CultureInfo culture)
            :base(FormatMessage(unchecked((UInt32)code),culture), code)
            {
            }

        public HResultException(HRESULT code, CultureInfo culture)
            :this((Int32)code, culture)
            {
            }

        public HResultException(HRESULT code)
            :this(code, null)
            {
            }

        public HResultException(Int32 code)
            :this(code, null)
            {
            }

        #region M:FormatMessage(UInt32,IntPtr,CultureInfo):String
        #if !LINUX
        protected internal static unsafe String FormatMessage(UInt32 SCode, IntPtr Module, CultureInfo Culture) {
            void* o;
            var LangId = ((((UInt32)(SUBLANG_DEFAULT)) << 10) | (UInt32)(LANG_NEUTRAL));
            if (Culture != null) {
                LangId = unchecked((UInt32)(Culture.LCID));
                }
            var Flags = (Module == IntPtr.Zero)
                ? FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_SYSTEM|FORMAT_MESSAGE_IGNORE_INSERTS
                : FORMAT_MESSAGE_ALLOCATE_BUFFER|FORMAT_MESSAGE_FROM_HMODULE|FORMAT_MESSAGE_IGNORE_INSERTS;
            if (FormatMessage(Flags,Module,SCode,
                LangId,&o, 0, IntPtr.Zero))
                {
                try
                    {
                    var r = (new String((Char*)o)).
                        Replace("\n", " ").
                        Replace("\r", " ").
                        Replace("  ", " ").
                        Trim();
                    return r;
                    }
                finally
                    {
                    LocalFree(o);
                    }
                }
            return null;
            }
        #endif
        #endregion
        #region M:FormatMessage(UInt32,String,CultureInfo):String
        #if !LINUX
        protected internal static String FormatMessage(UInt32 SCode, String Module, CultureInfo Culture) {
            if (Module == null) { throw new ArgumentNullException(nameof(Module)); }
            #if NET35
            if (String.IsNullOrEmpty(Module)) { throw new ArgumentOutOfRangeException(nameof(Module)); }
            #else
            if (String.IsNullOrWhiteSpace(Module)) { throw new ArgumentOutOfRangeException(nameof(Module)); }
            #endif
            lock(Libraries) {
                if (!Libraries.TryGetValue(Module,out var Library)) {
                    Library = LoadLibrary(Module);
                    if (Library == IntPtr.Zero) {
                        EnumProcessModules(out var Modules);
                        var ModuleNames = new String[Modules.Length];
                        for (var i = 0; i < Modules.Length; i++) {
                            ModuleNames[i] = GetModuleFileName(Modules[i]);
                            }
                        Library = LoadLibrary(Module,ModuleNames,Culture);
                        }
                    if (Library != IntPtr.Zero) {
                        Libraries.Add(Module,Library);
                        }
                    }
                if (Library != IntPtr.Zero) {
                    return FormatMessage(SCode,Library,Culture);
                    }
                }
            return null;
            }
        #endif
        #endregion
        #region M:FormatMessage(UInt32,CultureInfo):String
        public static String FormatMessage(UInt32 SCode, CultureInfo Culture = null) {
            #if LINUX
            var FacilityI = (SCode >> 16) & 0x1fff;
            var FacilityE = (FACILITY)FacilityI;
            var SCodeE = (HResult)(unchecked((Int32)SCode));
            var SCodeS = SCodeE.ToString();
            var o = (SCode >= 0xffff) || (SCode < 0)
                    ? $"HRESULT:{{{SCodeE}:{FacilityE}}}"
                    : $"WIN32:{{{(Win32ErrorCode)SCode}}}";
            if ((FacilityE == FACILITY.PSX) && (SCodeS.StartsWith("PSX_"))) {
                o = $"POSIX:{{{SCodeS.Substring(4)}}}";
                }
            var r = (SCode >= 0xffff) || (SCode < 0)
                ? Properties.HResult.ResourceManager.GetString(((HResult)SCode).ToString(),Culture)
                : Properties.Win32ErrorCode.ResourceManager.GetString(((Win32ErrorCode)SCode).ToString(),Culture);
            #if DEBUG
            return (r != null)
                ? $"{o}:{r}"
                : o;
            #else
            return (r != null) ? r : o;
            #endif
            #else
            var r = FormatMessage(SCode,IntPtr.Zero,Culture);
            if (r != null) {  return r; }
            var assembly = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            for (;;) {
                var FacilityI = (SCode >> 16) & 0x1fff;
                var FacilityE = (FACILITY)FacilityI;
                var SCodeE = (HRESULT)(unchecked((Int32)SCode));
                var Code = SCode & 0xffff;
                switch (FacilityI) {
                    case FACILITY_MEDIASERVER: { r = FormatMessage(SCode,"wmerror.dll",Culture); } break;
                    case FACILITY_GRAPHICS:
                        {
                        r = FormatMessage(SCode & 0x7fffffff,IntPtr.Zero,Culture) ??
                            FormatMessage(SCode | 0x80000000,IntPtr.Zero,Culture);
                        }
                        break;
                    case FACILITY_URT:         { r = LoadString("mscorrc.dll",(SCode & 0xffff) + 0x6000,Culture); } break;
                    }
                if (r != null) {  return r; }
                r = r ?? Properties.HResult.ResourceManager.GetString(((HRESULT)SCode).ToString(),Culture);
                r = r ?? FormatMessage(SCode,assembly, Culture);
                r = r ?? FormatMessage(SCode,assembly, English);
                if (r == null) {
                    r = (SCode >= 0xffff) || (SCode < 0)
                        ? $"HRESULT:{{{SCodeE}}},Facility:{{{FacilityE}}}"
                        : $"WIN32:{{{(Win32ErrorCode)SCode}}}";
                    Console.Error.WriteLine($"{{{SCode.ToString("x8")}}}:{r}");
                    }
                return r;
                }
            #endif
            }
        #endregion
        #region M:FormatMessage(Int32,CultureInfo):String
        public static String FormatMessage(Int32 SCode, CultureInfo Culture = null) {
            return FormatMessage(unchecked((UInt32)SCode), Culture);
            }
        #endregion
        #region M:FormatMessage(HResult,CultureInfo):String
        public static String FormatMessage(HRESULT SCode, CultureInfo Culture = null) {
            return FormatMessage((Int32)SCode, Culture);
            }
        #endregion
        #region M:FormatMessage(Win32ErrorCode,CultureInfo):String
        public static String FormatMessage(Win32ErrorCode SCode, CultureInfo Culture = null) {
            return FormatMessage((Int32)SCode, Culture);
            }
        #endregion
        #region M:FormatMessage(PosixError,CultureInfo):String
        public static String FormatMessage(PosixError SCode, CultureInfo Culture = null) {
            var o = $"{SCode}";
            #if LINUX
            var r = StrError(SCode);
            if (String.IsNullOrWhiteSpace(r)) {
                r = null;
                }
            else if (!r.EndsWith('.'))
                {
                r = $"{r}.";
                }
            return r ?? FormatMessage(0x90000000|(unchecked((UInt32)SCode)),Culture);
            #else
            return FormatMessage(0x90000000|(unchecked((UInt32)SCode)),Culture);
            #endif
            }
        #endregion

        private const UInt32 FORMAT_MESSAGE_IGNORE_INSERTS  = 0x00000200;
        private const UInt32 FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        private const UInt32 FORMAT_MESSAGE_FROM_SYSTEM     = 0x00001000;
        private const UInt32 FORMAT_MESSAGE_FROM_HMODULE    = 0x00000800;
        private const UInt32 FORMAT_MESSAGE_ARGUMENT_ARRAY  = 0x00002000;

        private const UInt32 LANG_NEUTRAL                   = 0x00;
        private const UInt32 SUBLANG_DEFAULT                = 0x01;
        private const Int32  FACILITY_NT_BIT                = 0x10000000;
        private const Int32  SEVERITY_SUCCESS     = 0;
        private const Int32  SEVERITY_ERROR       = 1;
        private const Int32  FACILITY_MEDIASERVER           = 0x000d;
        private const Int32  FACILITY_URT                   = 0x0013;
        private const Int32  FACILITY_GRAPHICS              = 0x0026;

        #if !LINUX
        [DllImport("kernel32.dll", SetLastError = true)] internal static extern unsafe IntPtr LocalFree(void* handle);
        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Unicode, SetLastError = true)] private static extern unsafe Boolean FormatMessage(UInt32 flags, IntPtr source,  UInt32 dwMessageId, UInt32 dwLanguageId, void* lpBuffer, Int32 nSize, IntPtr[] arguments);
        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Unicode, SetLastError = true)] private static extern unsafe Boolean FormatMessage(UInt32 flags, IntPtr source,  UInt32 dwMessageId, UInt32 dwLanguageId, void* lpBuffer, Int32 nSize, IntPtr arguments);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr LoadLibrary(String filename);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern Int32 GetModuleFileName(IntPtr Module,StringBuilder FileName, Int32 Length);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern Int32 LoadString(IntPtr Module,UInt32 Identifier,StringBuilder FileName, Int32 Length);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "K32EnumProcessModules")] private static extern IntPtr EnumProcessModules(IntPtr Process,[MarshalAs(UnmanagedType.LPArray)] IntPtr[] Modules, Int32 ModulesCount, out Int32 CountRequired);
        #endif

        #region M:GetModuleFileName(IntPtr):String
        #if !LINUX
        private static String GetModuleFileName(IntPtr Module) {
            var o = new StringBuilder(260);
            while (true) {
                var r = GetModuleFileName(Module, o, o.Capacity);
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
        #endif
        #endregion
        #region M:LoadString(IntPtr,UInt32):String
        #if !LINUX
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
        #endif
        #endregion
        #region M:LoadString(String,UInt32,CultureInfo):String
        #if !LINUX
        private static String LoadString(String Module,UInt32 Identifier,CultureInfo Culture) {
            if (Module == null) { throw new ArgumentNullException(nameof(Module)); }
            #if NET35
            if (String.IsNullOrEmpty(Module)) { throw new ArgumentOutOfRangeException(nameof(Module)); }
            #else
            if (String.IsNullOrWhiteSpace(Module)) { throw new ArgumentOutOfRangeException(nameof(Module)); }
            #endif
            lock(Libraries) {
                if (!Libraries.TryGetValue(Module,out var Library)) {
                    Library = LoadLibrary(Module);
                    if (Library == IntPtr.Zero) {
                        EnumProcessModules(out var Modules);
                        var ModuleNames = new String[Modules.Length];
                        for (var i = 0; i < Modules.Length; i++) {
                            ModuleNames[i] = GetModuleFileName(Modules[i]);
                            }
                        Library = LoadLibrary(Module,ModuleNames,Culture);
                        }
                    if (Library != IntPtr.Zero) {
                        Libraries.Add(Module,Library);
                        }
                    }
                if (Library != IntPtr.Zero) {
                    return LoadString(Library,Identifier);
                    }
                }
            return null;
            }
        #endif
        #endregion
        #region M:LoadLibrary(String,String[],CultureInfo):IntPtr
        #if !LINUX
        private static IntPtr LoadLibrary(String Module,String[] Modules,CultureInfo Culture) {
            #if NET40 || NET35
            Culture = Culture ?? CultureInfo.CurrentCulture;
            #else
            Culture = Culture
                ?? CultureInfo.DefaultThreadCurrentCulture
                ?? CultureInfo.CurrentCulture;
            #endif
            var IetfLanguageTag = Culture.IetfLanguageTag;
            var TwoLetterISOLanguageName = Culture.TwoLetterISOLanguageName;
            var Library = LoadLibrary(Module);
            if (Library == IntPtr.Zero) {
                for (var i = 0; i < Modules.Length; i++) {
                    if (Modules[i] != null) {
                        if (Modules[i].EndsWith(Module, StringComparison.OrdinalIgnoreCase)) {
                            Library = LoadLibrary(Modules[i]);
                            break;
                            }
                        }
                    }
                if (Library == IntPtr.Zero) {
                    for (var i = 0; i < Modules.Length; i++) {
                                                    Library = LoadLibrary(Path.Combine(Path.Combine(Path.GetDirectoryName(Modules[i]),IetfLanguageTag),Module));
                        if (Library == IntPtr.Zero) Library = LoadLibrary(Path.Combine(Path.Combine(Path.GetDirectoryName(Modules[i]),TwoLetterISOLanguageName),Module));
                        if (Library == IntPtr.Zero) Library = LoadLibrary(Path.Combine(Path.GetDirectoryName(Modules[i]),Module));
                        if (Library != IntPtr.Zero) {
                            break;
                            }
                        }
                    }
                }
            return Library;
            }
        #endif
        #endregion
        #region M:EnumProcessModules({out}IntPtr[])
        #if !LINUX
        private static void EnumProcessModules(out IntPtr[] Modules) {
            EnumProcessModules(GetCurrentProcess(),null,0,out var CountRequired);
            EnumProcessModules(GetCurrentProcess(),Modules = new IntPtr[CountRequired],CountRequired,out CountRequired);
            }
        #endif
        #endregion

        public static Exception GetExceptionForHR(HRESULT scode) { return GetExceptionForHR((Int32)scode, null); }
        public static Exception GetExceptionForHR(Int32 scode, CultureInfo culture) {
            if ((scode > 0xFFFF) || (scode < 0)) {
                switch ((HRESULT)scode) {
                    case HRESULT.CERT_E_CHAINING :            return new CertificateRevocationException((HRESULT)scode);
                    case HRESULT.CRYPT_E_NO_REVOCATION_CHECK: return new CertificateRevocationException((HRESULT)scode);
                    }
                }
            else
                {
                switch ((Win32ErrorCode)scode) {
                    case Win32ErrorCode.ERROR_ACCESS_DENIED: return new UnauthorizedAccessException(FormatMessage(unchecked((UInt32)scode), culture));
                    case Win32ErrorCode.ERROR_BUSY:          return new ResourceIsBusyException((HRESULT)scode);
                    }

                }
            return new HResultException(scode, culture);
            }

        #if !NET35
        private static readonly IDictionary<String,IntPtr> Libraries = new ConcurrentDictionary<String,IntPtr>(StringComparer.OrdinalIgnoreCase);
        #else
        private static readonly IDictionary<String,IntPtr> Libraries = new Dictionary<String,IntPtr>(StringComparer.OrdinalIgnoreCase);
        #endif
        private static readonly CultureInfo English = CultureInfo.GetCultureInfo("en-US");

        #if LINUX
        [DllImport("System.Native", EntryPoint = "SystemNative_StrErrorR")] private unsafe static extern IntPtr StrError(PosixError e, [MarshalAs(UnmanagedType.LPArray)] Byte[] buffer, int buffersize);
        private static String StrError(PosixError errnum) {
            for (var i = 1024;; i *= 2) {
                var r = new Byte[i];
                var n = StrError(errnum, r, r.Length);
                var e = (PosixError)Marshal.GetLastWin32Error();
                if (e == PosixError.ERANGE) { continue; }
                var m = (n != IntPtr.Zero)
                    ? Marshal.PtrToStringAnsi(n)
                    : Encoding.ASCII.GetString(r);
                return m;
                }
            }
        #endif
            
        public static Exception GetExceptionForHR(PosixError scode) {
            var message = FormatMessage(scode);
            switch (scode) {
                case PosixError.EINVAL: { return new ArgumentException(); }
                case PosixError.EPERM:
                case PosixError.ENOENT:
                case PosixError.ESRCH:
                case PosixError.EINTR:
                case PosixError.EIO:
                case PosixError.ENXIO:
                case PosixError.E2BIG:
                case PosixError.ENOEXEC:
                case PosixError.EBADF:
                case PosixError.ECHILD:
                case PosixError.EAGAIN:
                case PosixError.ENOMEM:
                case PosixError.EACCES:
                case PosixError.EFAULT:
                case PosixError.ENOTBLK:
                case PosixError.EBUSY:
                case PosixError.EEXIST:
                case PosixError.EXDEV:
                case PosixError.ENODEV:
                case PosixError.ENOTDIR:
                case PosixError.EISDIR:
                case PosixError.ENFILE:
                case PosixError.EMFILE:
                case PosixError.ENOTTY:
                case PosixError.ETXTBSY:
                case PosixError.EFBIG:
                case PosixError.ENOSPC:
                case PosixError.ESPIPE:
                case PosixError.EROFS:
                case PosixError.EMLINK:
                case PosixError.EPIPE:
                case PosixError.EDOM:
                case PosixError.ERANGE:
                default : { return new PlatformException(scode,message); }
                }
            }
        }
    }