using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [OMFDirectorySignature(OMFDirectorySignature.FB09)]
    public class CodeViewDirectoryFB09 : OMFDirectory
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.FB09; }}
        public CodeViewDirectoryFB09(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }