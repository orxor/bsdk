using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BinaryStudio.PlatformComponents.Win32
    {
    using HRESULT=HResult;
    public class HResultException : COMException
        {
        public HResultException(Int32 code, CultureInfo culture)
            :base(FormatMessage(unchecked((UInt32)code),culture), code)
            {
            }

        public HResultException(HResult code, CultureInfo culture)
            :this((Int32)code, culture)
            {
            }

        public HResultException(HResult code)
            :this(code, null)
            {
            }

        public HResultException(Int32 code)
            :this(code, null)
            {
            }

        #region M:FormatMessage(UInt32,IntPtr,CultureInfo):String
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
            //else
            //    {
            //    var ECode = Marshal.GetLastWin32Error();
            //    if ((ECode < 0xffff) && ((Win32ErrorCode)ECode == Win32ErrorCode.ERROR_RESOURCE_TYPE_NOT_FOUND)) {
            //        var r = LoadString(Module, SCode);
            //        return r;
            //        }
            //    }
            return null;
            }
        #endregion
        #region M:FormatMessage(UInt32,String,CultureInfo):String
        protected internal static String FormatMessage(UInt32 SCode, String Module, CultureInfo Culture) {
            if (Module == null) { throw new ArgumentNullException(nameof(Module)); }
            if (String.IsNullOrWhiteSpace(Module)) { throw new ArgumentOutOfRangeException(nameof(Module)); }
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
        #endregion
        #region M:FormatMessage(UInt32,CultureInfo):String
        public static String FormatMessage(UInt32 SCode, CultureInfo Culture = null) {
            var r = FormatMessage(SCode,IntPtr.Zero,Culture);
            if (r != null) {  return r; }
            for (;;) {
                var Severity  = (SCode >> 31) & 0x0001;
                var FacilityI = (SCode >> 16) & 0x1fff;
                var FacilityE = (FACILITY)FacilityI;
                var SCodeE = (HResult)(unchecked((Int32)SCode));
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
                if (SCode >= 0xffff) {
                    if (HResultMappings.TryGetValue(SCodeE,out var SCodeM)) {
                        SCode = unchecked((UInt32)(Int32)SCodeM);
                        continue;
                        }
                    }
                r = r ?? Properties.Resources.ResourceManager.GetString(((HResult)SCode).ToString(),Culture);
                if (r == null) {
                    r = (SCode >= 0xffff) || (SCode < 0)
                        ? $"HRESULT:{{{SCodeE}}},Facility:{{{FacilityE}}}"
                        : $"{{WIN32:{(Win32ErrorCode)SCode}}}";
                    }
                return r;
                }
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
        private const Int32  FACILITY_NULL                                     = 0x0000;
        private const Int32  FACILITY_RPC                                      = 0x0001;
        private const Int32  FACILITY_DISPATCH                                 = 0x0002;
        private const Int32  FACILITY_STORAGE                                  = 0x0003;
        private const Int32  FACILITY_ITF                                      = 0x0004;
        private const Int32  FACILITY_WIN32                                    = 0x0007;
        private const Int32  FACILITY_WINDOWS                                  = 0x0008;
        private const Int32  FACILITY_SSPI                                     = 0x0009;
        private const Int32  FACILITY_SECURITY                                 = 0x0009;
        private const Int32  FACILITY_CONTROL                                  = 0x000a;
        private const Int32  FACILITY_CERT                                     = 0x000b;
        private const Int32  FACILITY_INTERNET                                 = 0x000c;
        private const Int32  FACILITY_MEDIASERVER                              = 0x000d;
        private const Int32  FACILITY_MSMQ                                     = 0x000e;
        private const Int32  FACILITY_SETUPAPI                                 = 0x000f;
        private const Int32  FACILITY_SCARD                                    = 0x0010;
        private const Int32  FACILITY_COMPLUS                                  = 0x0011;
        private const Int32  FACILITY_AAF                                      = 0x0012;
        private const Int32  FACILITY_URT                                      = 0x0013;
        private const Int32  FACILITY_ACS                                      = 0x0014;
        private const Int32  FACILITY_DPLAY                                    = 0x0015;
        private const Int32  FACILITY_UMI                                      = 0x0016;
        private const Int32  FACILITY_SXS                                      = 0x0017;
        private const Int32  FACILITY_WINDOWS_CE                               = 0x0018;
        private const Int32  FACILITY_HTTP                                     = 0x0019;
        private const Int32  FACILITY_USERMODE_COMMONLOG                       = 0x001a;
        private const Int32  FACILITY_WER                                      = 0x001b;
        private const Int32  FACILITY_USERMODE_FILTER_MANAGER                  = 0x001f;
        private const Int32  FACILITY_BACKGROUNDCOPY                           = 0x0020;
        private const Int32  FACILITY_CONFIGURATION                            = 0x0021;
        private const Int32  FACILITY_WIA                                      = 0x0021;
        private const Int32  FACILITY_STATE_MANAGEMENT                         = 0x0022;
        private const Int32  FACILITY_METADIRECTORY                            = 0x0023;
        private const Int32  FACILITY_WINDOWSUPDATE                            = 0x0024;
        private const Int32  FACILITY_DIRECTORYSERVICE                         = 0x0025;
        private const Int32  FACILITY_GRAPHICS                                 = 0x0026;
        private const Int32  FACILITY_SHELL                                    = 0x0027;
        private const Int32  FACILITY_NAP                                      = 0x0027;
        private const Int32  FACILITY_TPM_SERVICES                             = 0x0028;
        private const Int32  FACILITY_TPM_SOFTWARE                             = 0x0029;
        private const Int32  FACILITY_UI                                       = 0x002a;
        private const Int32  FACILITY_XAML                                     = 0x002b;
        private const Int32  FACILITY_ACTION_QUEUE                             = 0x002c;
        private const Int32  FACILITY_PLA                                      = 0x0030;
        private const Int32  FACILITY_WINDOWS_SETUP                            = 0x0030;
        private const Int32  FACILITY_FVE                                      = 0x0031;
        private const Int32  FACILITY_FWP                                      = 0x0032;
        private const Int32  FACILITY_WINRM                                    = 0x0033;
        private const Int32  FACILITY_NDIS                                     = 0x0034;
        private const Int32  FACILITY_USERMODE_HYPERVISOR                      = 0x0035;
        private const Int32  FACILITY_CMI                                      = 0x0036;
        private const Int32  FACILITY_USERMODE_VIRTUALIZATION                  = 0x0037;
        private const Int32  FACILITY_USERMODE_VOLMGR                          = 0x0038;
        private const Int32  FACILITY_BCD                                      = 0x0039;
        private const Int32  FACILITY_USERMODE_VHD                             = 0x003a;
        private const Int32  FACILITY_USERMODE_HNS                             = 0x003b;
        private const Int32  FACILITY_SDIAG                                    = 0x003c;
        private const Int32  FACILITY_WEBSERVICES                              = 0x003d;
        private const Int32  FACILITY_WINPE                                    = 0x003d;
        private const Int32  FACILITY_WPN                                      = 0x003e;
        private const Int32  FACILITY_WINDOWS_STORE                            = 0x003f;
        private const Int32  FACILITY_INPUT                                    = 0x0040;
        private const Int32  FACILITY_QUIC                                     = 0x0041;
        private const Int32  FACILITY_EAP                                      = 0x0042;
        private const Int32  FACILITY_WINDOWS_DEFENDER                         = 0x0050;
        private const Int32  FACILITY_OPC                                      = 0x0051;
        private const Int32  FACILITY_XPS                                      = 0x0052;
        private const Int32  FACILITY_MBN                                      = 0x0054;
        private const Int32  FACILITY_POWERSHELL                               = 0x0054;
        private const Int32  FACILITY_RAS                                      = 0x0053;
        private const Int32  FACILITY_P2P_INT                                  = 0x0062;
        private const Int32  FACILITY_P2P                                      = 0x0063;
        private const Int32  FACILITY_DAF                                      = 0x0064;
        private const Int32  FACILITY_BLUETOOTH_ATT                            = 0x0065;
        private const Int32  FACILITY_AUDIO                                    = 0x0066;
        private const Int32  FACILITY_STATEREPOSITORY                          = 0x0067;
        private const Int32  FACILITY_VISUALCPP                                = 0x006d;
        private const Int32  FACILITY_SCRIPT                                   = 0x0070;
        private const Int32  FACILITY_PARSE                                    = 0x0071;
        private const Int32  FACILITY_BLB                                      = 0x0078;
        private const Int32  FACILITY_BLB_CLI                                  = 0x0079;
        private const Int32  FACILITY_WSBAPP                                   = 0x007a;
        private const Int32  FACILITY_BLBUI                                    = 0x0080;
        private const Int32  FACILITY_USN                                      = 0x0081;
        private const Int32  FACILITY_USERMODE_VOLSNAP                         = 0x0082;
        private const Int32  FACILITY_TIERING                                  = 0x0083;
        private const Int32  FACILITY_WSB_ONLINE                               = 0x0085;
        private const Int32  FACILITY_ONLINE_ID                                = 0x0086;
        private const Int32  FACILITY_DEVICE_UPDATE_AGENT                      = 0x0087;
        private const Int32  FACILITY_DRVSERVICING                             = 0x0088;
        private const Int32  FACILITY_DLS                                      = 0x0099;
        private const Int32  FACILITY_DELIVERY_OPTIMIZATION                    = 0x00d0;
        private const Int32  FACILITY_USERMODE_SPACES                          = 0x00e7;
        private const Int32  FACILITY_USER_MODE_SECURITY_CORE                  = 0x00e8;
        private const Int32  FACILITY_USERMODE_LICENSING                       = 0x00ea;
        private const Int32  FACILITY_SOS                                      = 0x00a0;
        private const Int32  FACILITY_DEBUGGERS                                = 0x00b0;
        private const Int32  FACILITY_SPP                                      = 0x0100;
        private const Int32  FACILITY_RESTORE                                  = 0x0100;
        private const Int32  FACILITY_DMSERVER                                 = 0x0100;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_SERVER               = 0x0101;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_IMAGING              = 0x0102;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_MANAGEMENT           = 0x0103;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_UTIL                 = 0x0104;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_BINLSVC              = 0x0105;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_PXE                  = 0x0107;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_TFTP                 = 0x0108;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_TRANSPORT_MANAGEMENT = 0x0110;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_DRIVER_PROVISIONING  = 0x0116;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_MULTICAST_SERVER     = 0x0121;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_MULTICAST_CLIENT     = 0x0122;
        private const Int32  FACILITY_DEPLOYMENT_SERVICES_CONTENT_PROVIDER     = 0x0125;
        private const Int32  FACILITY_LINGUISTIC_SERVICES                      = 0x0131;
        private const Int32  FACILITY_AUDIOSTREAMING                           = 0x0446;
        private const Int32  FACILITY_TTD                                      = 0x05d2;
        private const Int32  FACILITY_ACCELERATOR                              = 0x0600;
        private const Int32  FACILITY_WMAAECMA                                 = 0x07cc;
        private const Int32  FACILITY_DIRECTMUSIC                              = 0x0878;
        private const Int32  FACILITY_DIRECT3D10                               = 0x0879;
        private const Int32  FACILITY_DXGI                                     = 0x087a;
        private const Int32  FACILITY_DXGI_DDI                                 = 0x087b;
        private const Int32  FACILITY_DIRECT3D11                               = 0x087c;
        private const Int32  FACILITY_DIRECT3D11_DEBUG                         = 0x087d;
        private const Int32  FACILITY_DIRECT3D12                               = 0x087e;
        private const Int32  FACILITY_DIRECT3D12_DEBUG                         = 0x087f;
        private const Int32  FACILITY_DXCORE                                   = 0x0880;
        private const Int32  FACILITY_LEAP                                     = 0x0888;
        private const Int32  FACILITY_AUDCLNT                                  = 0x0889;
        private const Int32  FACILITY_WINCODEC_DWRITE_DWM                      = 0x0898;
        private const Int32  FACILITY_WINML                                    = 0x0890;
        private const Int32  FACILITY_DIRECT2D                                 = 0x0899;
        private const Int32  FACILITY_DEFRAG                                   = 0x0900;
        private const Int32  FACILITY_USERMODE_SDBUS                           = 0x0901;
        private const Int32  FACILITY_JSCRIPT                                  = 0x0902;
        private const Int32  FACILITY_PIDGENX                                  = 0x0a01;
        private const Int32  FACILITY_EAS                                      = 0x0055;
        private const Int32  FACILITY_WEB                                      = 0x0375;
        private const Int32  FACILITY_WEB_SOCKET                               = 0x0376;
        private const Int32  FACILITY_MOBILE                                   = 0x0701;
        private const Int32  FACILITY_SQLITE                                   = 0x07af;
        private const Int32  FACILITY_UTC                                      = 0x07c5;
        private const Int32  FACILITY_WEP                                      = 0x0801;
        private const Int32  FACILITY_SYNCENGINE                               = 0x0802;
        private const Int32  FACILITY_XBOX                                     = 0x0923;
        private const Int32  FACILITY_GAME                                     = 0x0924;
        private const Int32  FACILITY_PIX                                      = 0x0abc;
        private const Int32  FACILITY_DLT                                      = 0x0dea;

        [DllImport("kernel32.dll", SetLastError = true)] internal static extern unsafe IntPtr LocalFree(void* handle);
        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Unicode, SetLastError = true)] private static extern unsafe Boolean FormatMessage(UInt32 flags, IntPtr source,  UInt32 dwMessageId, UInt32 dwLanguageId, void* lpBuffer, Int32 nSize, IntPtr[] arguments);
        [DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Unicode, SetLastError = true)] private static extern unsafe Boolean FormatMessage(UInt32 flags, IntPtr source,  UInt32 dwMessageId, UInt32 dwLanguageId, void* lpBuffer, Int32 nSize, IntPtr arguments);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr LoadLibrary(String filename);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern Int32 GetModuleFileName(IntPtr Module,StringBuilder FileName, Int32 Length);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)] private static extern Int32 LoadString(IntPtr Module,UInt32 Identifier,StringBuilder FileName, Int32 Length);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, EntryPoint = "K32EnumProcessModules")] private static extern IntPtr EnumProcessModules(IntPtr Process,[MarshalAs(UnmanagedType.LPArray)] IntPtr[] Modules, Int32 ModulesCount, out Int32 CountRequired);

        #region M:GetModuleFileName(IntPtr):String
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
        #endregion
        #region M:LoadString(IntPtr,UInt32):String
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
        #endregion
        #region M:LoadString(String,UInt32,CultureInfo):String
        private static String LoadString(String Module,UInt32 Identifier,CultureInfo Culture) {
            if (Module == null) { throw new ArgumentNullException(nameof(Module)); }
            if (String.IsNullOrWhiteSpace(Module)) { throw new ArgumentOutOfRangeException(nameof(Module)); }
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
        #endregion
        #region M:LoadLibrary(String,String[],CultureInfo):IntPtr
        private static IntPtr LoadLibrary(String Module,String[] Modules,CultureInfo Culture) {
            #if NET40
            Culture = Culture ?? CultureInfo.CurrentCulture;
            #else
            Culture = Culture ?? CultureInfo.DefaultThreadCurrentCulture;
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
                                                    Library = LoadLibrary(Path.Combine(Path.GetDirectoryName(Modules[i]),IetfLanguageTag,Module));
                        if (Library == IntPtr.Zero) Library = LoadLibrary(Path.Combine(Path.GetDirectoryName(Modules[i]),TwoLetterISOLanguageName,Module));
                        if (Library == IntPtr.Zero) Library = LoadLibrary(Path.Combine(Path.GetDirectoryName(Modules[i]),Module));
                        if (Library != IntPtr.Zero) {
                            break;
                            }
                        }
                    }
                }
            return Library;
            }
        #endregion
        #region M:EnumProcessModules({out}IntPtr[])
        private static void EnumProcessModules(out IntPtr[] Modules) {
            EnumProcessModules(GetCurrentProcess(),null,0,out var CountRequired);
            EnumProcessModules(GetCurrentProcess(),Modules = new IntPtr[CountRequired],CountRequired,out CountRequired);
            }
        #endregion

        public static Exception GetExceptionForHR(Int32 scode) { return GetExceptionForHR(scode, null); }
        public static Exception GetExceptionForHR(Int32 scode, CultureInfo culture) {
            if ((scode > 0xFFFF) || (scode < 0)) {
                switch ((HResult)scode) {
                    }
                }
            else
                {
                switch ((Win32ErrorCode)scode) {
                    case Win32ErrorCode.ERROR_ACCESS_DENIED: return new UnauthorizedAccessException(FormatMessage(unchecked((UInt32)scode), culture));
                    }
                }
            return new HResultException(scode, culture);
            }

        private static readonly IDictionary<String,IntPtr> Libraries = new ConcurrentDictionary<String,IntPtr>(StringComparer.OrdinalIgnoreCase);
        private static readonly IDictionary<HRESULT,HRESULT> HResultMappings = new Dictionary<HResult,HResult>();
        }
    }