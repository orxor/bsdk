using System;

namespace BinaryStudio.PortableExecutable
    {
    public class OMFSrcSegInfo
        {
        public Int16 Index { get; }
        public Int32 StartOffset { get; }
        public Int32 EndOffset { get; }
        public OMFSrcLineInfo[] LineNumbers { get;internal set; }

        internal OMFSrcSegInfo(Int16 Index,Int32 StartOffset,Int32 EndOffset)
            {
            this.Index = Index;
            this.StartOffset = StartOffset;
            this.EndOffset = EndOffset;
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return $"{Index:x4}:{StartOffset:x8}-{EndOffset:x8}";
            }
        }
    }