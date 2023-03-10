using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.PlatformComponents
    {
    public static class Extensions
        {
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
        }
    }