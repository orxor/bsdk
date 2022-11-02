using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BinaryStudio.PortableExecutable.CodeView;

// ReSharper disable LocalVariableHidesMember
// ReSharper disable ParameterHidesMember

namespace BinaryStudio.PortableExecutable
    {
    public abstract class OMFDirectory : IFileDumpSupport,IServiceProvider,ICodeViewNameTable
        {
        protected readonly unsafe Byte* BaseAddress;
        protected readonly unsafe Byte* BegOfDebugData;
        protected readonly unsafe Byte* EndOfDebugData;
        protected Int32 Status;

        public abstract OMFDirectorySignature Signature { get; }
        public IList<String> Names { get;private set; }
        private IList<OMFSSection> Sections = EmptyList<OMFSSection>.Value;

        protected unsafe OMFDirectory(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            {
            this.BaseAddress = (Byte*)BaseAddress;
            this.BegOfDebugData = (Byte*)BegOfDebugData;
            this.EndOfDebugData = (Byte*)EndOfDebugData;
            Status = 0;
            Names = EmptyList<String>.Value;
            }

        protected unsafe void ValidateSignature(OMFDirectorySignatureHeader* Signature) {
            if (Signature == null) { throw new ArgumentNullException(nameof(Signature)); }
            if (Signature->Signature != this.Signature) { throw new InvalidDataException(); }
            }

        public virtual unsafe void Analyze() {
            OMFDirectorySignatureHeader* Signature;
            ValidateSignature(Signature = (OMFDirectorySignatureHeader*)BegOfDebugData);
            var Header  = (CodeViewSubsectionDirectoryHeader*)(BegOfDebugData + Signature->Offset);
            var Entries = (CodeViewSubsectionDirectoryEntry*)(Header + 1);
            #if OMFDEBUG
            for (var i = 0; i < Header->DirEntryCount; i++) {
                Debug.Print("ModuleIndex:{0:x4} Offset:{1:x8} FileOffset:{2:x8} Size:{3:x8} Type:{4}",
                    Entries[i].ModuleIndex,
                    Entries[i].Offset, BegOfDebugData + Entries[i].Offset - BaseAddress,
                    Entries[i].Size,
                    Entries[i].SDirectoryIndex);
                }
            #endif
            Status = 1;
            Sections = new List<OMFSSection>();
            for (var i = 0; i < Header->DirEntryCount; i++) {
                if (TryGetType(Entries[i].SDirectoryIndex, out var Type)) {
                    var Section = (OMFSSection)Activator.CreateInstance(Type,this);
                    Section.ModuleIndex = Entries[i].ModuleIndex;
                    Section.Offset = Entries[i].Offset;
                    Section.FileOffset = BegOfDebugData + Entries[i].Offset - BaseAddress;
                    Section.Size = Entries[i].Size;
                    Sections.Add(Section.Analyze(BaseAddress,BegOfDebugData + Entries[i].Offset, Entries[i].Size));
                    }
                }
            Names = Sections.OfType<OMFSSectionNames>().FirstOrDefault() ?? EmptyList<String>.Value;
            Sections.Where(i => i.SectionIndex != OMFSSectionIndex.Names).AsParallel().ForAll(i=>{
                i.ResolveReferences(this);
                });
            return;
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return $"{Signature}:{((Status == 1) ? "Ready" : "Pending...")}";
            }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <returns>A service object of type <paramref name="Service"/>.-or- null if there is no service object of type <paramref name="Service"/>.</returns>
        /// <param name="Service">An object that specifies the type of service object to get.</param>
        public Object GetService(Type Service) {
            if (Service == GetType()) { return this; }
            if (Service == typeof(ICodeViewNameTable)) { return this; }
            return null;
            }

        public virtual void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags)
            {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            Writer.WriteLine("{0}Directory:",LinePrefix);
            foreach (var section in Sections) {
                Writer.WriteLine("{0}  ModuleIndex:{1:x4} Offset:{2:x8} FileOffset:{3:x8} Size:{4:x8} Type:sst{5}",
                    LinePrefix,
                    section.ModuleIndex, section.Offset,
                    section.FileOffset, section.Size,
                    section.SectionIndex);
                }
            Writer.WriteLine("{0}Details:",LinePrefix);
            foreach (var section in Sections) {
                Writer.WriteLine("{0}  ModuleIndex:{1:x4} Offset:{2:x8} FileOffset:{3:x8} Size:{4:x8} Type:sst{5}",
                    LinePrefix,
                    section.ModuleIndex, section.Offset,
                    section.FileOffset, section.Size,
                    section.SectionIndex);
                section.WriteTo(Writer,LinePrefix + "    ", Flags);
                Writer.WriteLine();
                }
            }

        protected virtual Boolean TryGetType(OMFSSectionIndex Index, out Type Type)
            {
            return SSectionTypes.TryGetValue(Index, out Type);
            }

        private static readonly IDictionary<OMFSSectionIndex,Type> SSectionTypes = new Dictionary<OMFSSectionIndex,Type>();
        static OMFDirectory()
            {
            foreach (var type in typeof(OMFSSection).Assembly.GetTypes()) {
                var key = type.GetCustomAttributes(false).OfType<OMFSSectionIndexAttribute>().FirstOrDefault();
                if (key != null) {
                    SSectionTypes[key.Index] = type;
                    }
                }
            }

        String ICodeViewNameTable.this[Int32 Index] { get {
            return Names[Index];
            }}
        }
    }