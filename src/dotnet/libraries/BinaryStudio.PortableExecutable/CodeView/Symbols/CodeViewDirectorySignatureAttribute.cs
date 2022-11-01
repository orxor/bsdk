using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewDirectorySignatureAttribute : Attribute
        {
        public CodeViewDirectorySignature Signature { get; }
        public CodeViewDirectorySignatureAttribute(CodeViewDirectorySignature Signature)
            {
            this.Signature = Signature;
            }
        }
    }