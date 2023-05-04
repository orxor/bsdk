using System;

namespace BinaryStudio.Services
    {
    public interface LastErrorService
        {
        /**
         * <summary>
         * Retrieves the calling thread's last-error code value. The last-error code is maintained on a per-thread
         * basis. Multiple threads do not overwrite each other's last-error code.
         * </summary>
         * <returns>The return value is the calling thread's last-error code.</returns>
         * <remarks>
         *   <a href="https://learn.microsoft.com/en-us/windows/win32/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>
         * </remarks>
         */
        Int32 GetLastError();
        }
    }