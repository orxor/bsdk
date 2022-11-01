using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BinaryStudio.PortableExecutable
    {
    [OMFSSectionIndex(OMFSSectionIndex.SrcModule)]
    internal class OMFSSectionSrcModule : OMFSSection
        {
        private class LineInfo
            {
            public Int16 LineNumber;
            public Int32 SegmentOffset;
            }

        private class SegmentInfo
            {
            public Int16 Index;
            public Int32 StartOffset;
            public Int32 EndOffset;
            public LineInfo[] LineNumbers = EmptyArray<LineInfo>.Value;
            }

        private class FileInfo
            {
            public Int32 NameIndex;
            public Int32 Offset;
            public Int64 FileOffset;
            public SegmentInfo[] Segments = EmptyArray<SegmentInfo>.Value;
            }

        private SegmentInfo[] Segments = EmptyArray<SegmentInfo>.Value;
        private FileInfo[] Files = EmptyArray<FileInfo>.Value;

        public OMFSSectionSrcModule(OMFDirectory Directory)
            : base(Directory)
            {
            }
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.SrcModule; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            Analyze(BaseAddress,(OMFSrcModuleInfo*)Source);
            return this;
            }

        private unsafe void Analyze(Byte* BaseAddress,OMFSrcModuleInfo* Source)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            var BaseSrcFiles = (Int32*)(Source + 1);
            var SegmentAdrss = (OMFOffsetPair*)(BaseSrcFiles + Source->FileCount);
            var SegmentIndcs = (Int16*)(SegmentAdrss + Source->SegmentCount);
            Segments = new SegmentInfo[Source->SegmentCount];
            #if OMFDEBUG
            Debug.Print("FileCount:{0:x4} SegmentCount:{1:x4}",
                Source->FileCount,
                Source->SegmentCount);
            #endif
            if (Source->SegmentCount > 0) {
                #if OMFDEBUG
                Debug.Print("  Segments:");
                #endif
                for (var i = 0; i < Source->SegmentCount; i++) {
                    Segments[i] = new SegmentInfo{
                        Index = SegmentIndcs[i],
                        StartOffset = SegmentAdrss[i].StartOffset,
                        EndOffset = SegmentAdrss[i].EndOffset
                        };
                    #if OMFDEBUG
                    Debug.Print("    {0:x4}:{1:x8}-{2:x8}",
                        SegmentIndcs[i],
                        SegmentAdrss[i].StartOffset,
                        SegmentAdrss[i].EndOffset);
                    #endif
                    }
                }
            if (Source->FileCount > 0) {
                #if OMFDEBUG
                Debug.Print("  Files:");
                #endif
                Files = new FileInfo[Source->FileCount];
                for (var i = 0; i < Source->FileCount; i++) {
                    var SrcFile = (TD32SourceFileEntry*)((Byte*)Source + BaseSrcFiles[i]);
                    var SrcFileBaseSrcFiles = (Int32*)(SrcFile + 1);
                    Files[i] = new FileInfo{
                        NameIndex = SrcFile->NameIndex,
                        Offset = BaseSrcFiles[i],
                        FileOffset = (Byte*)Source - BaseAddress + BaseSrcFiles[i],
                        Segments = new SegmentInfo[SrcFile->SegmentCount]
                        };
                    #if OMFDEBUG
                    Debug.Print("    Offset:{0:x8} FileOffset:{1:x8} FileNameIndex:{2:x8}",
                        BaseSrcFiles[i],
                        (Byte*)Source - BaseAddress + BaseSrcFiles[i],
                        SrcFile->NameIndex);
                    #endif
                    for (var j = 0; j < SrcFile->SegmentCount; j++) {
                        var LineEntry = (TD32LineMappingEntry*)((Byte*)Source + SrcFileBaseSrcFiles[j]);
                        var LineEntryOffsets = (Int32*)(LineEntry + 1);
                        var LineEntryLnNmbrs = (Int16*)(LineEntryOffsets + LineEntry->PairCount);
                        Files[i].Segments[j] = new SegmentInfo{
                            LineNumbers = new LineInfo[LineEntry->PairCount]
                            };
                        #if OMFDEBUG
                        var DebugString = new StringBuilder("      Line numbers:\n     ");
                        #endif
                        for (var l = 0; l < LineEntry->PairCount; l++) {
                            Files[i].Segments[j].LineNumbers[l] = new LineInfo{
                                LineNumber = LineEntryLnNmbrs[l],
                                SegmentOffset = LineEntryOffsets[l]
                                };
                            #if OMFDEBUG
                            if ((l % 4 == 0) && (l > 0)) {
                                DebugString.AppendLine();
                                DebugString.Append("     ");
                                }
                            DebugString.AppendFormat(" {0:d5}:{1:x8}",
                                LineEntryLnNmbrs[l],
                                LineEntryOffsets[l]);
                            #endif
                            }
                        #if OMFDEBUG
                        Debug.Print(DebugString.ToString());
                        #endif
                        }
                    }
                }
            }

        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            Writer.WriteLine("{0}Segments:",LinePrefix);
            foreach (var Segment in Segments) {
                Writer.WriteLine("{0}  {{{1}}}:{2}-{3}",
                    LinePrefix,Segment.Index.ToString("x4"),
                    Segment.StartOffset.ToString("x8"),(Segment.EndOffset).ToString("x8"));
                }
            Writer.WriteLine("{0}Files:",LinePrefix);
            foreach (var File in Files) {
                Writer.WriteLine("{0}  Offset:{{{1}}} FileOffset:{{{2}}} Name:{{{3}}}:{{{4}}}",
                    LinePrefix,File.Offset.ToString("x8"),
                    File.FileOffset.ToString("x8"),File.NameIndex,
                    Directory.Names[File.NameIndex-1]);
                foreach (var Segment in File.Segments) {
                    Writer.Write("{0}  Line Numbers:\n{0} ",LinePrefix);
                    for (var i = 0; i < Segment.LineNumbers.Length; i++) {
                        if ((i % 4 == 0) && (i > 0)) {
                            Writer.WriteLine();
                            Writer.Write("{0} ", LinePrefix);
                            }
                        Writer.Write(" {0:d5}:{1:x8}",
                            Segment.LineNumbers[i].LineNumber,
                            Segment.LineNumbers[i].SegmentOffset);
                        }
                    Writer.WriteLine();
                    }
                }
            }
        }
    }