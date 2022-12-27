using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryStudio.PlatformComponents.Win32;

namespace UnitTests.BinaryStudio.PlatformComponents
    {
    [TestClass]
    public class HResultT
        {
        public void Setup()
            {
            }

        private void FormatMessage<T>()
            where T: struct, Enum
            {
            #if NET40 || NET47
            foreach (var name in Enum.GetNames(typeof(T))) {
                var scode = (Int32)(Object)(T)Enum.Parse(typeof(T),name);
                var value = HResultException.FormatMessage(unchecked((UInt32)scode));
                Assert.IsNotNull(value);
                }
            #else
            foreach (var name in Enum.GetNames<T>()) {
                var scode = (Int32)(Object)(T)Enum.Parse<T>(name);
                var value = HResultException.FormatMessage(unchecked((UInt32)scode));
                Console.WriteLine($"{name}:{value}");
  //              Console.WriteLine($@"  <data name=""{name}"" xml:space=""preserve"">
  //  <value>{value}</value>
  //</data>");
                Assert.IsNotNull(value);
                }
            #endif
            }

        [TestMethod]
        public void FormatMessage() {
            //FormatMessage<HResult>();
            FormatMessage<Win32ErrorCode>();
            }
        }
    }
