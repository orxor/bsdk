using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [OMFDirectorySignature(OMFDirectorySignature.NB09)]
    public class CodeViewDirectoryNB09 : OMFDirectory
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.NB09; }}
        public CodeViewDirectoryNB09(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }