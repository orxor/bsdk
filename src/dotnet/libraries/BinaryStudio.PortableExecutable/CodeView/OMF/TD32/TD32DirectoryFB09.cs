﻿using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFDirectorySignature(OMFDirectorySignature.FB09)]
    public class TD32DirectoryFB09 : OMFDirectory
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.FB09; }}
        public TD32DirectoryFB09(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }

        protected override Boolean TryGetType(OMFSSectionIndex Index, out Type Type) {
            switch (Index) {
                case OMFSSectionIndex.Module    : { Type = typeof(TD32SSectionModule);    return true; }
                case OMFSSectionIndex.AlignSym  : { Type = typeof(TD32SSectionAlignSym);  return true; }
                case OMFSSectionIndex.SrcModule : { Type = typeof(TD32SSectionSrcModule); return true; }
                }
            return base.TryGetType(Index, out Type);
            }
        }
    }