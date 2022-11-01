using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewDirectorySignature(CodeViewDirectorySignature.NB02)]
    public class CodeViewDirectoryNB02 : CodeViewDirectory
        {
        public override CodeViewDirectorySignature Signature { get { return CodeViewDirectorySignature.NB02; }}
        public CodeViewDirectoryNB02(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }