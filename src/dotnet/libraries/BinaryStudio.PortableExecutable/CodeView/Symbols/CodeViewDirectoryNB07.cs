using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [OMFDirectorySignature(OMFDirectorySignature.NB07)]
    public class CodeViewDirectoryNB07 : OMFDirectory
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.NB07; }}
        public CodeViewDirectoryNB07(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }