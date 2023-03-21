using System;
using System.Collections.Generic;

namespace BinaryStudio.Security.Cryptography
    {
    public interface DigestSource
        {
        IEnumerable<Byte[]> DigestSource { get; }
        }
    }