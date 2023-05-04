using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    public class NEMetadataObject : MetadataObject
        {
        public OMFDirectory DebugDirectory { get;private set; }
        public Int32 FileAlignmentUnitSize { get { return 512; }}
        public CV_CPU_TYPE CPU;

        internal NEMetadataObject(MetadataScope scope, MetadataObjectIdentity identity)
            : base(scope, identity)
            {
            CPU = CV_CPU_TYPE.CPU_PENTIUMIII;
            }

        #region M:LoadCore(IntPtr[],Int64)
        /// <summary>Loads content from specified source.</summary>
        /// <param name="source">Content specific source addresses depending on its type.</param>
        /// <param name="length">Length of content.</param>
        protected override unsafe void LoadCore(IntPtr[] source, Int64 length) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source.Length == 0) { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (length < 2) { throw new ArgumentOutOfRangeException(nameof(length)); }
            Load((Byte*)source[0],(IMAGE_NE_HEADER*)source[1],length);
            }
        #endregion
        #region M:LoadSection(CancellationToken,IntPtr,IMAGE_NE_HEADER,NE_SECTION_HEADER):Task
        private unsafe Task LoadSection(CancellationToken Cancellation,Byte* BaseAddress, IMAGE_NE_HEADER* ImageFileHeader, NE_SECTION_HEADER* SectionHeader) {
            return Task.Factory.StartNew(() => {

                }, Cancellation);
            }
        #endregion
        #region M:LoadSections(CancellationToken,IntPtr,IMAGE_NE_HEADER,Int64):Task
        private unsafe Task LoadSections(CancellationToken Cancellation, Byte* BaseAddress, IMAGE_NE_HEADER* ImageFileHeader,Int64 Size) {
            return Task.Factory.StartNew(() => {
                var Sections = new IntPtr[ImageFileHeader->SegmentCount];
                var Tasks = new List<Task>();
                var Section = (NE_SECTION_HEADER*)((Byte*)ImageFileHeader + ImageFileHeader->SegmentTableOffset);
                var LastSegmentEndOffset = 0;
                for (var i = 0; i < ImageFileHeader->SegmentCount; i++) {
                    Sections[i] = (IntPtr)Section;
                    Debug.Print("{0:D2}:{1:x8}-{2:x8}",i,
                        Section->LogicalOffset*FileAlignmentUnitSize,
                        ((Int32)Section->LogicalOffset)*FileAlignmentUnitSize + Section->Length - 1);
                    LastSegmentEndOffset = Math.Max(LastSegmentEndOffset,((Int32)Section->LogicalOffset)*FileAlignmentUnitSize + Section->Length - 1);
                    Tasks.Add(LoadSection(Cancellation,BaseAddress,ImageFileHeader,Section));
                    Section++;
                    }
                Debug.Print("LastSegmentEndOffset:{0:x8}",LastSegmentEndOffset);
                #if NET40
                Task.WaitAll(Tasks.ToArray());
                #else
                Task.WhenAll(Tasks);
                #endif
                },Cancellation);
            }
        #endregion
        #region M:LoadDebugInfo(CancellationToken,IntPtr,IMAGE_NE_HEADER,Int64):Task
        private unsafe Task LoadDebugInfo(CancellationToken Cancellation, Byte* BaseAddress, IMAGE_NE_HEADER* ImageFileHeader,Int64 Size) {
            return Task.Factory.StartNew(() => {
                var EndOfDebugData = (Byte*)(BaseAddress + Size - sizeof(OMFDirectorySignatureHeader) + sizeof(IMAGE_DOS_HEADER));
                var SignatureHeader = (OMFDirectorySignatureHeader*)EndOfDebugData;
                try
                    {
                    foreach (var type in typeof(OMFDirectory).Assembly.GetTypes()) {
                        var key = type.GetCustomAttributes(false).OfType<OMFDirectorySignatureAttribute>().FirstOrDefault();
                        if (key != null) {
                            if (key.Signature == SignatureHeader->Signature) {
                                var BegOfDebugData = EndOfDebugData - SignatureHeader->Offset + sizeof(OMFDirectorySignatureHeader);
                                var Directory = (OMFDirectory)Activator.CreateInstance(type,
                                    (IntPtr)BaseAddress,
                                    (IntPtr)BegOfDebugData,
                                    (IntPtr)EndOfDebugData);
                                Directory.CPU = CPU;
                                Directory.Analyze();
                                using (var writer = new StreamWriter(File.Create("my.dump")))
                                    {
                                    Directory.WriteTo(writer,String.Empty,0);
                                    }
                                DebugDirectory = Directory;
                                break;
                                }
                            }
                        }
                    }
                catch (Exception e)
                    {
                    Debug.Print(Exceptions.ToString(e));
                    throw;
                    }
                Debug.Print("LoadDebugInfo:Complete");
                }, Cancellation);
            }
        #endregion

        protected unsafe void Load(Byte* BaseAddress, IMAGE_NE_HEADER* ImageFileHeader, Int64 Size) {
            var Cancellation = default(CancellationToken);
            Load(Cancellation,BaseAddress,ImageFileHeader,Size,
                LoadSections,
                LoadDebugInfo).Wait(Cancellation);
            return;
            }

        #region M:Load(CancellationToken,IntPtr,IMAGE_NE_HEADER,Int64,LoadRoutine[]):Task
        private unsafe delegate Task LoadRoutine(CancellationToken Cancellation, Byte* BaseAddress, IMAGE_NE_HEADER* ImageFileHeader,Int64 Size);
        private static unsafe Task Load(CancellationToken Cancellation, Byte* BaseAddress, IMAGE_NE_HEADER* ImageFileHeader,Int64 Size, params LoadRoutine[] args) {
            return Task.Factory.StartNew(() => {
                var Tasks = new List<Task>();
                foreach (var Task in args) {
                    Tasks.Add(Task(Cancellation,BaseAddress,ImageFileHeader,Size));
                    }
                #if NET40
                Task.WaitAll(Tasks.ToArray());
                #else
                Task.WhenAll(Tasks).Wait(Cancellation);
                #endif
                }, Cancellation);
            }
        #endregion
        }
    }