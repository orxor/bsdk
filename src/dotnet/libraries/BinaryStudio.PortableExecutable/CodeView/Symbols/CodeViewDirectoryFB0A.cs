using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [OMFDirectorySignature(OMFDirectorySignature.FB0A)]
    public class CodeViewDirectoryFB0A : CodeViewDirectoryFB09
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.FB0A; }}
        public CodeViewDirectoryFB0A(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }