using System;
using System.Diagnostics;
using System.IO;

// ReSharper disable LocalVariableHidesMember
// ReSharper disable ParameterHidesMember

namespace BinaryStudio.PortableExecutable
    {
    public abstract class OMFDirectory
        {
        protected readonly unsafe Byte* BaseAddress;
        protected readonly unsafe Byte* BegOfDebugData;
        protected readonly unsafe Byte* EndOfDebugData;
        protected Int32 Status;

        public abstract OMFDirectorySignature Signature { get; }
        protected unsafe OMFDirectory(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            {
            this.BaseAddress = (Byte*)BaseAddress;
            this.BegOfDebugData = (Byte*)BegOfDebugData;
            this.EndOfDebugData = (Byte*)EndOfDebugData;
            Status = 0;
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
            for (var i = 0; i < Header->DirEntryCount; i++) {

                }
            return;
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return $"{Signature}:{((Status == 1) ? "Ready" : "Pending...")}";
            }
        }
    }