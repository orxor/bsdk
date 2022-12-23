
using System;

namespace BinaryStudio.Security.Cryptography
    {
    public abstract class CryptographicContext : CryptographicObject
        {
        public static CryptographicFactory Factory { get; }

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

        static CryptographicContext()
            {
            Factory = new CryptographicFactory();
            }
        }
    }
