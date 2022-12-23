using System;
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
            #else
            DefaultContext= new SCryptographicContext();
            #endif
            }
        }
    }
