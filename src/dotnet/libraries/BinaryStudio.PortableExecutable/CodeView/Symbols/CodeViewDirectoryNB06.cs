using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [OMFDirectorySignature(OMFDirectorySignature.NB06)]
    public class CodeViewDirectoryNB06 : OMFDirectory
        {
        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.NB06; }}
        public CodeViewDirectoryNB06(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }
        }
    }