using System;
using System.Windows;

namespace BinaryStudio.PlatformUI.Markup
    {
    public interface IXamlSerializer : IDisposable
        {
        void Write(DependencyObject Source);
        void Write(Object Source);
        }
    }