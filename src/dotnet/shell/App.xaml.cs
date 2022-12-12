using System.IO;
using System.Text;
using System.Windows;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.DiagnosticServices.Tracing;
using BinaryStudio.PlatformComponents;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
    {
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
