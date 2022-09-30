using System;
using System.Windows;
using Microsoft.VisualStudio.Shell;

namespace BinaryStudio.VSShellServices
    {
    public class ToolWindow : ToolWindowPane
        {
        #region P:Caption:String
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.RegisterAttached("Caption", typeof(String), typeof(ToolWindow), new PropertyMetadata(default(String)));
        public static void SetCaption(DependencyObject element, String value)
            {
            element.SetValue(CaptionProperty, value);
            }

        public static String GetCaption(DependencyObject element)
            {
            return (String) element.GetValue(CaptionProperty);
            }
        #endregion

        public ToolWindow(Type type)
            : base(null)
            {
            var r = Activator.CreateInstance(type) as DependencyObject;
            Caption = GetCaption(r);
            Content = r;
            }
        }

    public class ToolWindow<T> : ToolWindow
        where T: DependencyObject, new()
        {
        public ToolWindow()
            :base(typeof(T))
            {
            }
        }
    }