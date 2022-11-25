using System.Windows.Input;

namespace PlatformUISample
    {
    public class LocalCommands
        {
        public static readonly RoutedUICommand CopyToXaml = new RoutedUICommand(nameof(CopyToXaml),nameof(CopyToXaml), typeof(LocalCommands));
        }
    }