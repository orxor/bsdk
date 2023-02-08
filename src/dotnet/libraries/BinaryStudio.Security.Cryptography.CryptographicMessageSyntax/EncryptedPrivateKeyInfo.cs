using System;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public class EncryptedPrivateKeyInfo : Asn1LinkObject
        {
        public X509AlgorithmIdentifier EncryptionAlgorithm { get; }
        public Byte[] EncryptedData { get; }
        internal EncryptedPrivateKeyInfo(Asn1Object o)
            : base(o)
            {
            State |= ObjectState.Failed;
            State &= ~ObjectState.DisposeUnderlyingObject;
            EncryptionAlgorithm = new EncryptionAlgorithmIdentifier((Asn1Sequence)o[0]);
            EncryptedData = EmptyArray<Byte>.Value;
            if (o[1] != null) {
                EncryptedData = o[1].Body;
                }
            State &= ~ObjectState.Failed;
            State |= ObjectState.DisposeUnderlyingObject;
            }
        }
    }