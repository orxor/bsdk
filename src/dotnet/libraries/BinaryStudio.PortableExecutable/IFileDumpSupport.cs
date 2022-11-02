using System;
using System.IO;

namespace BinaryStudio.PortableExecutable
    {
    public interface IFileDumpSupport
        {
        /// <summary>Writes DUMP with specified flags.</summary>
        /// <param name="Writer">The <see cref="TextWriter"/> to write to.</param>
        /// <param name="LinePrefix">The line prefix for formatting purposes.</param>
        /// <param name="Flags">DUMP flags.</param>
        void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags);
        }
    }