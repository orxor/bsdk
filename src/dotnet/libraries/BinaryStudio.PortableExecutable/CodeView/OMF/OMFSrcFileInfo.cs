using System;

namespace BinaryStudio.PortableExecutable
    {
    public class OMFSrcFileInfo
        {
        public String Name { get; }
        public Int32 Offset { get; }
        public Int64 FileOffset { get; }
        public OMFSrcSegInfo[] Segments { get; }

        public OMFSrcFileInfo(String Name,Int32 Offset,Int64 FileOffset,Int32 SegmentCount) {
            this.Name = Name;
            this.Offset = Offset;
            this.FileOffset = FileOffset;
            Segments = new OMFSrcSegInfo[SegmentCount];
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return $"{FileOffset:x8}:{Name}";
            }
        }
    }