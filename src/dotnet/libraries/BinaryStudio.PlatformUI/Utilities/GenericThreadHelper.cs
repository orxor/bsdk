using System;

namespace BinaryStudio.PlatformUI
    {
    internal class GenericThreadHelper : ThreadHelper
        {
        protected override IDisposable GetInvocationWrapper()
            {
            return null;
            }
        }
    }