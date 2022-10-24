using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public class CmsObject : Asn1LinkObject
        {
        public CmsObject(Asn1Object o)
            : base(o)
            {
            State |= ObjectState.Failed;
            }
        }
    }