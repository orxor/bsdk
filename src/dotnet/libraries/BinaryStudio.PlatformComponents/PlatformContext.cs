using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.PlatformComponents.Win32;
using log4net;
using Process = BinaryStudio.PlatformComponents.Win32.Process;
#if NET40_OR_GREATER
using System.DirectoryServices.AccountManagement;
#endif

namespace BinaryStudio.PlatformComponents
    {
    public class PlatformContext
        {
        private static Boolean? sc;
        private static CultureInfo culture = CultureInfo.CurrentUICulture;
        private static readonly ILogger logger = new PlatformContextLogger(LogManager.GetLogger(nameof(PlatformContext)));

        #region P:DefaultCulture:CultureInfo
        public static CultureInfo DefaultCulture {
            get { return culture; }
            set
                {
                value = value ?? CultureInfo.CurrentUICulture;
                culture = value;
                DefaultCultureChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        #endregion

        #region P:IsRunningUnderServiceControl:Boolean
        public static Boolean IsRunningUnderServiceControl { get {
            if (sc == null) {
                sc = false;
                var processes = new Dictionary<Int64, Process>();
                foreach (var process in Process.Processes) { processes[process.UniqueProcessId] = process; }
                var i = processes[GetCurrentProcessId()];   if ((i.SessionId == 0)) {
                    i = processes[i.UniqueParentProcessId]; if ((i.SessionId == 0) && (String.Equals(i.ImageName, "services.exe", StringComparison.OrdinalIgnoreCase))) {
                    i = processes[i.UniqueParentProcessId]; if ((i.SessionId == 0) && (String.Equals(i.ImageName, "wininit.exe", StringComparison.OrdinalIgnoreCase)))
                        {
                        sc = true;
                        }}
                    }
                }
            return sc.GetValueOrDefault();
            }}
        #endregion
        #region P:IsRemoteSession:Boolean
        public static Boolean IsRemoteSession { get {
            if (GetSystemMetrics(SM_REMOTECONTROL) > 0) { return true; }
            if (GetSystemMetrics(SM_REMOTESESSION) > 0) { return true; }
            if (RegOpenKeyEx(HKEY_LOCAL_MACHINE,TERMINAL_SERVER_KEY,0,RegistrySpecificAccessRights.Read, out var key) == 0) {
                try
                    {
                    var size = sizeof(Int32);
                    if (RegQueryValueEx(key, GLASS_SESSION_ID, IntPtr.Zero, IntPtr.Zero, out var value, ref size) == 0) {
                        if (ProcessIdToSessionId(GetCurrentProcessId(), out var session)) {
                            if (session != value) { return true; }
                            }
                        }
                    }
                finally
                    {
                    RegCloseKey(key);
                    }
                }
            return false;
            }}
        #endregion
        #region P:IsRunningAsAdministrator:Boolean
        public static Boolean IsRunningAsAdministrator { get {
            var r = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return r.IsInRole(WindowsBuiltInRole.Administrator);
            }}
        #endregion

        #region M:IsParentProcess(String):Boolean
        public static Boolean IsParentProcess(String processname) {
            var processes = new Dictionary<Int64, Process>();
            foreach (var process in Process.Processes) { processes[process.UniqueProcessId] = process; }
            var
            i = processes[GetCurrentProcessId()];
            i = processes[i.UniqueParentProcessId];
            return String.Equals(i.ImageName, processname, StringComparison.OrdinalIgnoreCase);
            }
        #endregion
        #region M:ValidatePermission(WindowsBuiltInRole)
        #if NET40_OR_GREATER
        public static void ValidatePermission(WindowsBuiltInRole role)
            {
            try
                {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                using (var context = new PrincipalContext(ContextType.Machine)) {
                    using (var searcher = new PrincipalSearcher(new GroupPrincipal(context))) {
                        foreach (var i in searcher.FindAll().OfType<GroupPrincipal>()) {
                            if (i.Sid.Value == $"S-1-5-32-{(Int32)role}") {
                                ValidatePermission(i);
                                return;
                                }
                            }
                        }
                    }
                throw new ArgumentOutOfRangeException(nameof(role));
                }
            catch (SecurityException e) {
                if (String.Equals(e.Message, GetResourceString("Security_PrincipalPermission"))) {
                    throw (new PrincipalPermissionException(e.Message, e)).
                        Add("WindowsBuiltInRole", role);
                    }
                e.Add("WindowsBuiltInRole", role);
                throw;
                }
            }
        #endif
        #endregion
        #region M:ValidatePermission(GroupPrincipal)
        #if NET40_OR_GREATER
        public static void ValidatePermission(GroupPrincipal group)
            {
            try
                {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                ValidatePermission(new PrincipalPermission(null, group.Name));
                }
            catch (PrincipalPermissionException e) {
                e.Add("GroupPrincipal", group.Name);
                e.Add("GroupPrincipalSid", group.Sid.Value);
                throw;
                }
            catch (SecurityException e) {
                if (String.Equals(e.Message, GetResourceString("Security_PrincipalPermission"))) {
                    throw (new PrincipalPermissionException(e.Message, e)).
                        Add("GroupPrincipal", group.Name).
                        Add("GroupPrincipalSid", group.Sid.Value);
                    }
                e.Add("GroupPrincipal", group.Name);
                e.Add("GroupPrincipalSid", group.Sid.Value);
                throw;
                }
            }
        #endif
        #endregion
        #region M:ValidatePermission(IPermission)
        public static void ValidatePermission(IPermission permission)
            {
            if (permission == null) { throw new ArgumentNullException(nameof(permission)); }
            try
                {
                permission.Demand();
                }
            catch (PrincipalPermissionException e) {
                e.Add("Permission", permission.GetType().FullName);
                throw;
                }
            catch (SecurityException e) {
                if (String.Equals(e.Message, GetResourceString("Security_PrincipalPermission"))) {
                    throw (new PrincipalPermissionException(e.Message, e)).
                        Add("Permission", permission.GetType().FullName);
                    }
                e.Add("Permission", permission.GetType().FullName);
                throw;
                }
            }
        #endregion

        public static ILogger Logger { get{ return logger; }}
        public static event EventHandler DefaultCultureChanged;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)] private static extern Int32 GetCurrentProcessId();
        [DllImport("user32.dll")] private static extern Int32 GetSystemMetrics(Int32 index);
        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)] private static extern Int32 RegOpenKeyEx(IntPtr key, String lpSubKey, Int32 ulOptions, RegistrySpecificAccessRights samDesired, out IntPtr hkResult);
        [DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)] private static extern Int32 RegQueryValueEx(IntPtr key, String valuename, IntPtr reserved, IntPtr type, out Int32 data, ref Int32 size);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)] private static extern Boolean ProcessIdToSessionId([In] Int32 process, out Int32 session);
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)] internal static extern Int32 RegCloseKey(IntPtr key);

        private const Int32 SM_REMOTESESSION = 0x1000;
        private const Int32 SM_REMOTECONTROL = 0x2001;
        private static readonly IntPtr HKEY_LOCAL_MACHINE = (IntPtr)(unchecked((Int32)0x80000002));
        private const String TERMINAL_SERVER_KEY = "SYSTEM\\CurrentControlSet\\Control\\Terminal Server\\";
        private const String GLASS_SESSION_ID    = "GlassSessionId";

        private static String GetResourceString(String key)
            {
            return (String)typeof(Environment).GetMethod("GetResourceString",
                BindingFlags.NonPublic|BindingFlags.Static,
                null, new []{ typeof(String) }, null).Invoke(null, new Object[]{ key});
            }

        private class PlatformContextLogger : DefaultLogger
            {
            private readonly ILog log;
            private readonly ILog debugger = LogManager.GetLogger("Debug");
            public PlatformContextLogger(ILog log)
                {
                this.log = log;
                }

            public override Boolean IsEnabled(LogLevel loglevel) {
                switch (loglevel)
                    {
                    case LogLevel.Trace:       return debugger.IsDebugEnabled;
                    case LogLevel.Debug:       return debugger.IsDebugEnabled;
                    case LogLevel.Information: return log.IsInfoEnabled;
                    case LogLevel.Warning:     return log.IsWarnEnabled;
                    case LogLevel.Error:       return log.IsErrorEnabled;
                    case LogLevel.Critical:    return log.IsFatalEnabled;
                    case LogLevel.None: break;
                    default: throw new ArgumentOutOfRangeException(nameof(loglevel), loglevel, null);
                    }
                return false;
                }

            public override void Log(LogLevel loglevel, String message) {
                switch (loglevel)
                    {
                    case LogLevel.Trace:       debugger.Debug(message); break;
                    case LogLevel.Debug:       debugger.Debug(new String(' ',Debug.IndentLevel*2)+message); break;
                    case LogLevel.Information: log.Info(message);  break;
                    case LogLevel.Warning:     log.Warn(message);  break;
                    case LogLevel.Error:       log.Error(message); break;
                    case LogLevel.Critical:    log.Fatal(message); break;
                    case LogLevel.None: break;
                    default: throw new ArgumentOutOfRangeException(nameof(loglevel), loglevel, null);
                    }
                }
            }
        }
    }