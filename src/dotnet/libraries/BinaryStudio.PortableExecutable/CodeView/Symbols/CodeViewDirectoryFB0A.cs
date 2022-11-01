using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewDirectorySignature(CodeViewDirectorySignature.FB0A)]
    public class CodeViewDirectoryFB0A : CodeViewDirectoryFB09
        {
        public override CodeViewDirectorySignature Signature { get { return CodeViewDirectorySignature.FB0A; }}
        public CodeViewDirectoryFB0A(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }