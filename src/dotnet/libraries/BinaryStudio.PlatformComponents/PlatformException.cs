using System;

namespace BinaryStudio.PlatformComponents
    {
    public class PlatformException : Exception
        {
        public PosixError PosixError { get; }

        public PlatformException(PosixError e) {
            PosixError = e;
            }

        public PlatformException(PosixError e, String message)
            :base(message)
            {
            PosixError = e;
            }
        }
    }
