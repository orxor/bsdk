using System;
using System.IO;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    public abstract class OMFSSection : IFileDumpSupport
        {
        public CV_CPU_TYPE? CPU { get;internal set; }
        public OMFDirectory Directory { get; }
        public abstract OMFSSectionIndex SectionIndex { get; }
        public Int16 ModuleIndex { get;internal set; }
        public Int32 Offset { get;internal set; }
        public Int64 FileOffset { get;internal set; }
        public Int32 Size { get;internal set; }

        protected OMFSSection(OMFDirectory Directory)
            {
            this.Directory = Directory;
            }

        public abstract unsafe OMFSSection Analyze(Byte* BaseAddress,Byte* Source, Int32 Size);
        public virtual void ResolveReferences(OMFDirectory Directory)
            {
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return $"sst{SectionIndex}";
            }

        public virtual void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags)
            {
            }
        }
    }