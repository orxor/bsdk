using System;
using BinaryStudio.PlatformComponents.Win32;
using NUnit.Framework;

namespace UnitTests.BinaryStudio.PlatformComponents
    {
    public class HResultT
        {
        [SetUp]
        public void Setup()
            {
            }

        [Test]
        public void FormatMessage() {
            #if NET40
            foreach (var name in Enum.GetNames(typeof(HResult))) {
                var scode = (Int32)(HResult)Enum.Parse(typeof(HResult),name);
                var value = HResultException.FormatMessage(unchecked((UInt32)scode));
                Assert.IsNotEmpty(value);
                }
            #else
            foreach (var name in Enum.GetNames<HResult>()) {
                var scode = (Int32)(HResult)Enum.Parse<HResult>(name);
                var value = HResultException.FormatMessage(unchecked((UInt32)scode));
                Assert.IsNotEmpty(value);
                }
            #endif
            }
        }
    }
