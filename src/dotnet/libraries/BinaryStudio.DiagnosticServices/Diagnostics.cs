using System;
using System.Reflection;
using System.Text;
#if UseWPF
using System.Windows;
#endif
using BinaryStudio.DiagnosticServices.Logging;

namespace BinaryStudio.DiagnosticServices
    {
    public static class Diagnostics
        {
        #if UseWPF
        #region P:Diagnostics.Key:Object
        public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached("Key", typeof(Object), typeof(Diagnostics), new PropertyMetadata(default(Object)));
        public static void SetKey(DependencyObject e, Object value)
            {
            e.SetValue(KeyProperty, value);
            }

        private static readonly Random rng = new Random(0);
        public static Object GetKey(DependencyObject e) {
            if (e != null) {
                var r = e.GetValue(KeyProperty);
                if (r == null) {
                    r = rng.Next(UInt16.MaxValue).ToString("x4");
                    SetKey(e, r);
                    }
                return r;
                }
            return null;
            }
        #endregion
        #endif

        public static void Print(MethodBase value) {
            var r = new StringBuilder();
            r.Append("Method:");
            if (value == null) { r.Append("{none}"); }
            else
                {
                r.Append(value.DeclaringType.Name);
                r.Append(".");
                r.Append(value.Name);
                var parameters = value.GetParameters();
                if (parameters.Length > 0) {
                    r.Append("(");
                    for (var i = 0; i < parameters.Length; i++) {
                        if (i > 0) { r.Append(","); }
                        var parameter = parameters[i];
                        var type      = parameter.ParameterType;
                        var typename  = parameter.ParameterType.Name;
                        if (parameter.IsOut)
                            {
                            r.Append("{out}");
                            r.Append(type.IsByRef ? typename.Substring(0, typename.Length - 1) : typename);
                            }
                        else if (type.IsByRef)
                            {
                            r.Append("{ref}");
                            r.Append(typename.Substring(0, typename.Length - 1));
                            }
                        else
                            {
                            r.Append(typename);
                            }
                        }
                    r.Append(")");
                    }
                }
            (new DefaultLogger()).Log(LogLevel.Debug, r.ToString());
            }
        }
    }