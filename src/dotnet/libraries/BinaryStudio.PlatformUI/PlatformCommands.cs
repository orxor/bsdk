using System;
using System.Windows.Input;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI
    {
    public class PlatformCommands
        {
        public static readonly RoutedCommand CopyToXamlV = new RoutedUICommand(nameof(CopyToXamlV),nameof(CopyToXamlV),typeof(PlatformCommands));
        public static readonly RoutedCommand CopyToXamlE = new RoutedUICommand(nameof(CopyToXamlE),nameof(CopyToXamlE),typeof(PlatformCommands));
        }
    }