using System;
using System.IO;
using System.Text.RegularExpressions;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider;

namespace BinaryStudio.Security.Cryptography
    {
    public abstract class CryptographicContext : CryptographicObject
        {
        public static CryptographicContext DefaultContext { get; }

        #region M:Dispose(Boolean)
        /// <summary>
        /// Releases the unmanaged resources used by the instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            if (disposing) {
                }
            }
        #endregion

        static CryptographicContext() {
            #if LINUX
            if (File.Exists("/etc/opt/cprocsp/config64.ini")) {
                var cnfig = File.ReadAllText("/etc/opt/cprocsp/config64.ini");
                var regex = new Regex(@"[""]libcapi20[.]so[""]\p{Zs}*[=]\p{Zs}*[""](.+libcapi20[.]so)[""]\n");
                var match = regex.Match(cnfig);
                if (match.Success) {
                    var capiso = match.Groups[1].Value;
                    if (File.Exists(capiso)) {
                        PlatformContext.Logger.Log(LogLevel.Information, $"library:{capiso}");
                        DefaultContext= new CCryptographicContext();
                        }
                    }
                }
            #else
            DefaultContext= new SCryptographicContext();
            #endif
            }
        }
    }
