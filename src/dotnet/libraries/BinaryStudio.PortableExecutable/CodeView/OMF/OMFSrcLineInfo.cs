using System;

namespace BinaryStudio.PortableExecutable
    {
    public class OMFSrcLineInfo
        {
        public Int16 LineNumber { get; }
        public Int32 SegmentOffset { get; }

        internal OMFSrcLineInfo(Int16 LineNumber, Int32 SegmentOffset)
            {
            this.LineNumber = LineNumber;
            this.SegmentOffset = SegmentOffset;
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return $"{LineNumber:d5}:{SegmentOffset:x8}";
            }
        }
    }