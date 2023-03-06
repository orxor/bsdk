using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.BinaryStudio.Common;
using UnitTests.BinaryStudio.PlatformComponents;
using UnitTests.BinaryStudio.PortableExecutable;
using UnitTests.BinaryStudio.Security.Cryptography.Certificates;
using UnitTests.BinaryStudio.Security.Cryptography.CryptographyServiceProvider;
using UnitTests.BinaryStudio.Security.Cryptography.PersonalInformationExchangeSyntax;
using ColorScope = BinaryStudio.DiagnosticServices.ColorScope;

namespace UnitTests.BinaryStudio.Task
    {
    internal class Program
        {
        private static void Main(string[] args)
            {
            var assembly = Assembly.GetEntryAssembly();
            var repository = LogManager.GetRepository(assembly);
            XmlConfigurator.Configure(repository,new FileInfo("log4net.config"));
            for (var i = 0; i < 16;i++) {
                Write((ConsoleColor)i,$" {{{((ConsoleColor)i)}}}");
                if ((i + 1) % 8 == 0) {
                    Console.WriteLine();
                    }
                }
            for (var i = 0; i < args.Length; i++) {
                if (String.Equals(args[i],"wait",StringComparison.OrdinalIgnoreCase)) {
                    Console.WriteLine("Press [ENTER] to continue...");
                    Debug.WriteLine("Waiting for user action...");
                    Console.ReadLine();
                    }
                }

            //(new Executor()).Execute(typeof(CryptographicContextT),nameof(CryptographicContextT.DecryptFT));
            //(new Executor()).Execute(typeof(X509CertificateStorageT),nameof(X509CertificateStorageT.Certificates));
            //(new Executor()).Execute(typeof(HResultT));
            (new Executor()).Execute(typeof(CryptKeyGOST));
            //(new Executor()).Execute(typeof(PfxFileT));
            }

        #region M:Write(ConsoleColor,String)
        private static void Write(ConsoleColor color, String message) {
            using (new ColorScope(color)) {
                Console.Write(message);
                }
            }
        #endregion
        #region M:WriteLine(ConsoleColor,String)
        private static void WriteLine(ConsoleColor color, String message) {
            using (new ColorScope(color)) {
                Console.WriteLine(message);
                }
            }
        #endregion
        }
    }
