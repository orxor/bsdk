using System;
using BinaryStudio.DiagnosticServices.Logging;
using log4net;

namespace UnitTests.BinaryStudio.Common
    {
    public class ClassT
        {
        public static ILogger Logger = new ClientLogger(LogManager.GetLogger(nameof(ClassT)));

        #region M:Write(ConsoleColor,String)
        protected static void Write(ConsoleColor color, String message) {
            using (new ColorScope(color)) {
                Console.Write(message);
                }
            }
        #endregion
        #region M:WriteLine(ConsoleColor,String)
        protected internal static void WriteLine(ConsoleColor color, String message) {
            using (new ColorScope(color)) {
                Console.WriteLine(message);
                }
            }
        #endregion
        }
    }