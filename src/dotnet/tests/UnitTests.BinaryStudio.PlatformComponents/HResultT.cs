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

        [TestMethod]
        public void FormatMessage() {
            #if NET40 || NET47
            foreach (var name in Enum.GetNames(typeof(HResult))) {
                var scode = (Int32)(HResult)Enum.Parse(typeof(HResult),name);
                var value = HResultException.FormatMessage(unchecked((UInt32)scode));
                Assert.IsNotNull(value);
                }
            #else
            foreach (var name in Enum.GetNames<HResult>()) {
                var scode = (Int32)(HResult)Enum.Parse<HResult>(name);
                var value = HResultException.FormatMessage(unchecked((UInt32)scode));
                Console.WriteLine($"{name}:{value}");
                Assert.IsNotNull(value);
                }
            #endif
            }
        }
    }
