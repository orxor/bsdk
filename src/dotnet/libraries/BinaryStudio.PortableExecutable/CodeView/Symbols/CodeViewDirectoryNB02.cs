using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [OMFDirectorySignature(OMFDirectorySignature.NB02)]
    public class CodeViewDirectoryNB02 : OMFDirectory
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.NB02; }}
        public CodeViewDirectoryNB02(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }