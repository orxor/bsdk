using System;
using System.Windows;
using System.Windows.Media;
using BinaryStudio.PlatformUI;
using BinaryStudio.PlatformUI.Models;

namespace PlatformUISample
    {
    public class ColorInfo : FrameworkElement
        {
        public ThemeResourceKey Source { get; }
        public ColorInfo(ThemeResourceKey source)
            {
            Source = source;
            SetResourceReference(ColorProperty,source);
            }

        #region P:Color:Brush
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(ColorInfo), new PropertyMetadata(default(Brush)));
        public Brush Color {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
            }
        #endregion
        }
    }