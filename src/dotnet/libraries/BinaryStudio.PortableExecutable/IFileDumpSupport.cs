using System;
using System.IO;

namespace BinaryStudio.PortableExecutable
    {
    public interface IFileDumpSupport
        {
        void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags);
        }
    }