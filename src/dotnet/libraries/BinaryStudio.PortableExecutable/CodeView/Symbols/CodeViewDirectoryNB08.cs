using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [OMFDirectorySignature(OMFDirectorySignature.NB08)]
    public class CodeViewDirectoryNB08 : OMFDirectory
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.NB08; }}
        public CodeViewDirectoryNB08(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }