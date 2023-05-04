using System;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public class CmsEncryptedContentInfo : CmsContentInfo
        {
        public new Byte[] Content { get; }
        public X509AlgorithmIdentifier ContentEncryptionAlgorithm { get; }
        internal CmsEncryptedContentInfo(Asn1Object o)
            : base(o)
            {
            State |= ObjectState.Failed;
            State &= ~ObjectState.DisposeUnderlyingObject;
            ContentEncryptionAlgorithm = new ContentEncryptionAlgorithmIdentifier((Asn1Sequence)o[1]);
            Content = Context.InnerBody;
            State &= ~ObjectState.Failed;
            State |= ObjectState.DisposeUnderlyingObject;
            }
        }
    }