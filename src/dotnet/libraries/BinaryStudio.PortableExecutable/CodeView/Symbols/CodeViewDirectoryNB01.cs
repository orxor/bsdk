using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewDirectorySignature(CodeViewDirectorySignature.NB01)]
    public class CodeViewDirectoryNB01 : CodeViewDirectory
        {
        public override CodeViewDirectorySignature Signature { get { return CodeViewDirectorySignature.NB01; }}
        public CodeViewDirectoryNB01(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }