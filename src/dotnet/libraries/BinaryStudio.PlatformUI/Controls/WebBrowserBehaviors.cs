using System;
using System.Windows;
using System.Windows.Controls;

namespace BinaryStudio.PlatformUI.Controls
    {
    public class WebBrowserBehaviors
        {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(Object), typeof(WebBrowserBehaviors), new PropertyMetadata(default(Object),OnSourceChanged));
        private static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is WebBrowser WebBrowser) {
                if (e.NewValue is String String) {
                    WebBrowser.NavigateToString(String);
                    return;
                    }
                }
            }

        public static void SetSource(DependencyObject element, Object value)
            {
            element.SetValue(SourceProperty, value);
            }

        public static Object GetSource(DependencyObject element)
            {
            return element.GetValue(SourceProperty);
            }
        }
    }