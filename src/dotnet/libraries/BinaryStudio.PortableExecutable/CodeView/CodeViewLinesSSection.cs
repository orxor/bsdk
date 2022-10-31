using BinaryStudio.PortableExecutable.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewLinesSSection : CodeViewPrimarySSection
        {
        private const UInt16 CV_LINES_HAVE_COLUMNS = 0x0001;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct HEADER
            {
            public readonly UInt32 offCon;
            public readonly UInt16 segCon;
            public readonly UInt16 flags;
            public readonly UInt32 cbCon;
            }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct FILEBLOCK
            {
            public readonly UInt32 fileid;
            public readonly Int32 nLines;
            public readonly Int32 cbFileBlock;
            }

        public override DEBUG_S Type { get { return DEBUG_S.DEBUG_S_LINES; }}
        public Boolean HasColumns { get; }
        public UInt32 offCon { get; }
        public UInt16 segCon { get; }
        public UInt32 cbCon { get; }
        public UInt16 flags { get; }
        public IList<CodeViewLinesSSectionFileBlock> Blocks { get; }

        internal unsafe CodeViewLinesSSection(CodeViewSection section, Int32 offset, Byte* content, Int32 length)
            : base(section, offset, content, length)
            {
            var r = content;
            #if DEBUG
            Debug.Assert(length >= sizeof(HEADER));
            #endif
            if (length < sizeof(HEADER)) { throw new ArgumentOutOfRangeException(nameof(length)); }
            var blocks = new List<CodeViewLinesSSectionFileBlock>();
            var H = (HEADER*)r;
            var LA = r + length;
            var hascolumns = HasColumns = ((H->flags & CV_LINES_HAVE_COLUMNS) == CV_LINES_HAVE_COLUMNS);
            offCon = H->offCon;
            segCon = H->segCon;
            cbCon = H->cbCon;
            flags = H->flags;
            r += sizeof(HEADER);
            #if DEBUG
            length -= sizeof(HEADER);
            #endif
            while (r < LA) {
                var block = (FILEBLOCK*)r;
                var B = new CodeViewLinesSSectionFileBlock(block->fileid); 
                r += sizeof(FILEBLOCK);
                blocks.Add(B);
                if (block->nLines > 0) {
                    var LI = (CV_Line_t*)r;
                    var CO = hascolumns
                        ? (CV_Column_t*)(r + sizeof(CV_Line_t)*block->nLines)
                        : null;
                    for (var i = 0; i < block->nLines; i++) {
                        B.Add(LI, H->offCon);
                        LI++;
                        if (hascolumns)
                            {
                            B.Add(CO);
                            CO++;
                            }
                        }
                    }
                r += block->cbFileBlock - sizeof(FILEBLOCK);
                #if DEBUG
                length -= sizeof(FILEBLOCK);
                length -= block->cbFileBlock - sizeof(FILEBLOCK);
                #endif
                }
            #if DEBUG
            Debug.Assert(length == 0);
            #endif
            Blocks = blocks.ToArray();
            }
        }
    }