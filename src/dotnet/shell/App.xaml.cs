using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.DiagnosticServices.Tracing;
using BinaryStudio.PlatformComponents;
using CefSharp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
    {
    /// <summary>Raises the <see cref="E:System.Windows.Application.Startup"/> event.</summary>
    /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
    protected override void OnStartup(StartupEventArgs e)
        {
        AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        base.OnStartup(e);
        }

    protected override void OnActivated(EventArgs e) {
        if (!Cef.IsInitialized) {
            var settings = new CefSettings {
                BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    Environment.Is64BitProcess ? "x64" : "x86",
                    "CefSharp.BrowserSubprocess.exe")
                };
            Cef.Initialize(settings);
            }
        base.OnActivated(e);
        }

    private Assembly OnAssemblyResolve(Object sender, ResolveEventArgs args) {
        Debug.Print($"RequestingAssembly:{args.Name}");
        if (args.Name.StartsWith("CefSharp")) {
            var assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
            var architectureSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                Environment.Is64BitProcess ? "x64" : "x86",
                assemblyName);
            return File.Exists(architectureSpecificPath)
                ? Assembly.LoadFile(architectureSpecificPath)
                : null;
            }
        return null;
        }

    /// <summary>Raises the <see cref="E:System.Windows.Application.Exit"/> event.</summary>
    /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs"/> that contains the event data.</param>
    protected override void OnExit(ExitEventArgs e)
        {
        var builder = new StringBuilder();
        using (var writer = new StringWriter(builder)) {
            TraceScope.WriteTo(writer);
            }
        PlatformContext.Logger.Log(LogLevel.Trace, $"\n{builder}");
        base.OnExit(e);
        }
    }
