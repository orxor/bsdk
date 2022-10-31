using BinaryStudio.PortableExecutable.Win32;
using System;
using System.Collections.Generic;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewFrameDataSSection : CodeViewPrimarySSection
        {
        public override DEBUG_S Type { get { return DEBUG_S.DEBUG_S_FRAMEDATA; }}
        public IList<CodeViewFrameData> Frames { get; }
        internal unsafe CodeViewFrameDataSSection(CodeViewSection section, Int32 offset, Byte* content, Int32 length)
            : base(section, offset, content, length)
            {
            Frames = new List<CodeViewFrameData>();
            var F = content;
            var L = content + length;
            ReadUInt32(ref F);
            #if DEBUG
            length -= sizeof(UInt32);
            #endif
            while (F < L) {
                var H = (FRAMEDATA*)F;
                Frames.Add(new CodeViewFrameData(H));
                F += sizeof(FRAMEDATA);
                #if DEBUG
                length -= sizeof(FRAMEDATA);
                #endif
                }
            }
        }
    }