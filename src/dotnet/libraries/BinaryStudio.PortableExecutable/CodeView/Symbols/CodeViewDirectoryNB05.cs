using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewDirectorySignature(CodeViewDirectorySignature.NB05)]
    public class CodeViewDirectoryNB05 : CodeViewDirectory
        {
        public override CodeViewDirectorySignature Signature { get { return CodeViewDirectorySignature.NB05; }}
        public CodeViewDirectoryNB05(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }