using System;

namespace BinaryStudio.PortableExecutable
    {
    [OMFDirectorySignature(OMFDirectorySignature.FB0A)]
    public class TD32DirectoryFB0A : TD32DirectoryFB09
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.FB0A; }}
        public TD32DirectoryFB0A(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }