using System;
using System.Windows.Input;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI
    {
    public class PlatformCommands
        {
        public static readonly RoutedCommand CopyToXaml = new RoutedUICommand(nameof(CopyToXaml),nameof(CopyToXaml),typeof(PlatformCommands));
        }
    }