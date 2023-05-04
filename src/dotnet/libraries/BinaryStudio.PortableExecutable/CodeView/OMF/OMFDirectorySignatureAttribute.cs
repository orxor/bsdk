using System;

namespace BinaryStudio.PortableExecutable
    {
    public class OMFDirectorySignatureAttribute : Attribute
        {
        public OMFDirectorySignature Signature { get; }
        public OMFDirectorySignatureAttribute(OMFDirectorySignature Signature)
            {
            this.Signature = Signature;
            }
        }
    }