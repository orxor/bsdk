using System;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.PlatformComponents
    {
    public static class Extensions
        {
        #region M:ToSystemTime({this}DateTime):SYSTEMTIME
        public static SYSTEMTIME ToSystemTime(this DateTime source) {
            return new SYSTEMTIME{
                Year = (Int16)source.Year,
                Month = (Int16)source.Month,
                Day = (Int16)source.Day,
                Hour = (Int16)source.Hour,
                Minute = (Int16)source.Minute,
                Second = (Int16)source.Second,
                Milliseconds = (Int16)source.Millisecond
                };
            }
        #endregion
        //#region M:ForAll<T>({this}IEnumerable<T>,Action<T>)
        //public static void ForAll<T>(this IEnumerable<T> values, Action<T> action) {
        //    if (values == null) { throw new ArgumentNullException(nameof(values)); }
        //    if (action == null) { throw new ArgumentNullException(nameof(action)); }
        //    foreach (var item in values) {
        //        action(item);
        //        }
        //    }
        //#endregion
        }
    }