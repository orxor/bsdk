using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    internal static class Extensions
        {
        #region M:ToString({this}Byte[],String):String
        public static String ToString(this Byte[] source, String format) {
            if (source == null) { return null; }
            var c = source.Length;
            #if NET35
            switch (format)
                {
                case "X" : { return String.Join(String.Empty, source.Select(i => i.ToString("X2")).ToArray()); }
                case "x" : { return String.Join(String.Empty, source.Select(i => i.ToString("x2")).ToArray()); }
                case "FL":
                    {
                    return (c > 1)
                        ? $"{source[0]:X2}{source[c - 1]:X2}"
                        : String.Join(String.Empty, source.Select(i => i.ToString("X2")).ToArray());
                    }
                }
            #else
            switch (format)
                {
                case "X" : { return String.Join(String.Empty, source.Select(i => i.ToString("X2"))); }
                case "x" : { return String.Join(String.Empty, source.Select(i => i.ToString("x2"))); }
                case "FL":
                    {
                    return (c > 1)
                        ? $"{source[0]:X2}{source[c - 1]:X2}"
                        : String.Join(String.Empty, source.Select(i => i.ToString("X2")));
                    }
                case "fl":
                    {
                    return (c > 1)
                        ? $"{source[0]:x2}{source[c - 1]:x2}"
                        : String.Join(String.Empty, source.Select(i => i.ToString("x2")));
                    }
                }
            #endif
            return source.ToString();
            }
        #endregion
        #region M:ForAll<T>({this}IEnumerable<T>,Action<T>)
        public static void ForAll<T>(this IEnumerable<T> values, Action<T> action) {
            if (values == null) { throw new ArgumentNullException(nameof(values)); }
            if (action == null) { throw new ArgumentNullException(nameof(action)); }
            foreach (var item in values) {
                action(item);
                }
            }
        #endregion
        }
    }