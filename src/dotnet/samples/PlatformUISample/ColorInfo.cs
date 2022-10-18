using System;
using System.Windows;
using System.Windows.Media;
using BinaryStudio.PlatformUI;
using BinaryStudio.PlatformUI.Media;
using BinaryStudio.PlatformUI.Models;

namespace PlatformUISample
    {
    public class ColorInfo : FrameworkElement
        {
        public String Description { get;set; }
        public ThemeResourceKey Source { get; }
        public ColorInfo(ThemeResourceKey source)
            {
            Source = source;
            SetResourceReference(ColorProperty,source);
            }

        public ColorInfo(HSLColor source)
            {
            Color = new SolidColorBrush((Color)source);
            Description = source.ToString();
            }

        public ColorInfo(HSVColor source)
            {
            Color = new SolidColorBrush((Color)source);
            Description = source.ToString();
            }

        public ColorInfo(Color source)
            {
            Color = new SolidColorBrush(source);
            Description = source.ToString();
            }

        #region P:Color:Brush
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(ColorInfo), new PropertyMetadata(default(Brush)));
        public Brush Color {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
            }
        #endregion
        #region P:BorderThickness:Thickness
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(ColorInfo), new PropertyMetadata(new Thickness(1)));
        public Thickness BorderThickness
            {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
            }
        #endregion
        }
    }