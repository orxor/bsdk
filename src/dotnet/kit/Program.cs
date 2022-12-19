using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using log4net;
using log4net.Config;

internal class Program
    {
    [MTAThread]
    private static void Main(String[] args) {
        var assembly = Assembly.GetEntryAssembly();
        var repository = LogManager.GetRepository(assembly);
        XmlConfigurator.Configure(repository,new FileInfo("log4net.config"));
        using (new ColorScope()) {
            #if !LINUX
            if (PlatformContext.IsParentProcess(Path.GetFileName(assembly.Location))) {
                FreeConsole();
                Validate(AttachConsole(-1));
                }
            #endif
            Int32 exitcode;
            #if LINUX
            using (var client = new LocalClient())
            #else
            using (var client = PlatformContext.IsRunningUnderServiceControl
                    ? (ILocalClient)(new LocalService())
                    : (ILocalClient)(new LocalClient()))
            #endif
                {
                exitcode = client.Main(args);
                }
            Environment.ExitCode = exitcode;
            }
        }

    private static void Validate(Boolean status) {
        if (!status) {
            throw HResultException.GetExceptionForHR(Marshal.GetLastWin32Error());
            }
        }

    #if !LINUX
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)] private static extern IntPtr LoadLibraryExW([In] string lpwLibFileName, [In] IntPtr hFile, [In] uint dwFlags);
    [DllImport("kernel32.dll", SetLastError = true)] private static extern Boolean FreeLibrary(IntPtr module);
    [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi)] private static extern IntPtr GetProcAddress(IntPtr module, String procedure);
    [DllImport("kernel32.dll")] private static extern Int32 GetProcessId(IntPtr handle);
    [DllImport("kernel32.dll")] private static extern IntPtr GetCurrentProcess();
    [DllImport("kernel32.dll")] private static extern Boolean AttachConsole(Int32 process);
    [DllImport("kernel32.dll")] private static extern Boolean FreeConsole();
    [DllImport("kernel32.dll")] private static extern Boolean AllocConsole();
    [DllImport("ntdll.dll", CharSet = CharSet.Auto)] private static extern unsafe UInt32 NtQueryInformationProcess(IntPtr process, Int32 iclass, void* pi, UInt32 pisz, out UInt32 r);
    #endif
    }
