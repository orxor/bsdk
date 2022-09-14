﻿using System.Reflection;
using System.Text;
using BinaryStudio.DiagnosticServices.Logging;

namespace BinaryStudio.DiagnosticServices
    {
    public static class Diagnostic
        {
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