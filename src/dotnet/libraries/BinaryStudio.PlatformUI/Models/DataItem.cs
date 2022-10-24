using System;
using System.Windows;

namespace BinaryStudio.PlatformUI.Models
    {
    public class DataItem
        {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached("Content", typeof(Object), typeof(DataItem), new PropertyMetadata(default(Object)));
        public static void SetContent(DependencyObject element, Object value)
            {
            element.SetValue(ContentProperty,value);
            }

        public static Object GetContent(DependencyObject element)
            {
            return (Object)element.GetValue(ContentProperty);
            }
        }
    }