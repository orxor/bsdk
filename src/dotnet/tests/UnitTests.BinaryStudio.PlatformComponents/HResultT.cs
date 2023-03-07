using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.PlatformComponents;

namespace UnitTests.BinaryStudio.PlatformComponents
    {
    [TestClass]
    public class HResultT
        {
        private static readonly CultureInfo en = CultureInfo.GetCultureInfo("en-US");
        public void Setup()
            {
            }

        #region M:FormatMessageHResult
        [TestMethod]
        public void FormatMessageHResult() {
            #if NET40 || NET47 || NET45
            foreach (var name in Enum.GetNames(typeof(HResult))) {
                var scode = (HResult)Enum.Parse(typeof(HResult),name);
                var value = HResultException.FormatMessage(scode);
                Console.WriteLine($@"""{scode}"":""{value}""");
                Assert.IsNotNull(value);
                }
            #else
            foreach (var name in Enum.GetNames<HResult>()) {
                var scode = Enum.Parse<HResult>(name);
                var value = HResultException.FormatMessage(scode,en);
                Console.WriteLine($@"""0x{(Int32)scode:X8}"";""{name}"";""{value}""");
                Assert.IsNotNull(value);
                }
            #endif
            }
        #endregion
        #region M:FormatMessageWin32ErrorCode
        [TestMethod]
        [Ignore]
        public void FormatMessageWin32ErrorCode() {
            #if NET40 || NET47 || NET45
            foreach (var name in Enum.GetNames(typeof(Win32ErrorCode))) {
                var scode = (Win32ErrorCode)Enum.Parse(typeof(Win32ErrorCode),name);
                var value = HResultException.FormatMessage(scode);
                Assert.IsNotNull(value);
                }
            #else
            foreach (var name in Enum.GetNames<Win32ErrorCode>()) {
                var scode = Enum.Parse<Win32ErrorCode>(name);
                var value = HResultException.FormatMessage(scode);
                Assert.IsNotNull(value);
                }
            #endif
            }
        #endregion
        #region M:FormatMessagePosixError
        [TestMethod]
        [Ignore]
        public void FormatMessagePosixError() {
            #if NET40 || NET47 || NET45
            foreach (var name in Enum.GetNames(typeof(PosixError))) {
                var scode = (PosixError)Enum.Parse(typeof(PosixError),name);
                var value = HResultException.FormatMessage(scode);
                Assert.IsNotNull(value);
                }
            #else
            foreach (var name in Enum.GetNames<PosixError>()) {
                var scode = Enum.Parse<PosixError>(name);
                var value = HResultException.FormatMessage(scode);
                //Console.WriteLine($"{name}:{value}");
  //              Console.WriteLine($@"  <data name=""{name}"" xml:space=""preserve"">
  //  <value>{value}</value>
  //</data>");
                Assert.IsNotNull(value);
                }
            #endif
            }
        #endregion
        }
    }
