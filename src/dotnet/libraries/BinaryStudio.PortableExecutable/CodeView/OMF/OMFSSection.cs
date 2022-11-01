using System;

namespace BinaryStudio.PortableExecutable
    {
    public abstract class OMFSSection
        {
        public OMFDirectory Directory { get; }
        public abstract OMFSSectionIndex SectionIndex { get; }
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
        }
    }