using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewDirectorySignature(CodeViewDirectorySignature.NB00)]
    public class CodeViewDirectoryNB00 : CodeViewDirectory
        {
        public override CodeViewDirectorySignature Signature { get { return CodeViewDirectorySignature.NB00; }}
        public CodeViewDirectoryNB00(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }