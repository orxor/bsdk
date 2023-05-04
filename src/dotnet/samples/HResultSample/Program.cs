using System;
using System.Globalization;
using System.Linq;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;

namespace HResultSample
    {
    internal class Program
        {
        private static void EnumReport<T>(CultureInfo culture)
            where T: Enum
            {
            foreach (var name in Enum.GetNames(typeof(T)).OrderBy(i=>{
                    return unchecked((UInt32)(Int32)(Enum.Parse(typeof(T), i)));
                    }))
                {
                var scode = unchecked((UInt32)(Int32)(Enum.Parse(typeof(T), name)));
                var value = HResultException.FormatMessage(scode, culture);
                Console.Out.WriteLine($"{{{scode.ToString("x8")}}}:{{{name}}}:{{{value}}}");
                //Console.Out.WriteLine($"  <Message Language=\"0x0409\" SCode=\"0x{scode.ToString("x8")}\">{value}</Message>");
                }
            }

        static void Main(string[] args)
            {
            var culture = CultureInfo.GetCultureInfo("en-US");
            Console.OutputEncoding = Encoding.UTF8;
            EnumReport<HResult>(culture);
            EnumReport<Win32ErrorCode>(culture);
            }
        }
    }
