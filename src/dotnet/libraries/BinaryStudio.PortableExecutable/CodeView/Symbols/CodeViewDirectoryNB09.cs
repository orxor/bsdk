using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [CodeViewDirectorySignature(CodeViewDirectorySignature.NB09)]
    public class CodeViewDirectoryNB09 : CodeViewDirectory
        {
        public override CodeViewDirectorySignature Signature { get { return CodeViewDirectorySignature.NB09; }}
        public CodeViewDirectoryNB09(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }