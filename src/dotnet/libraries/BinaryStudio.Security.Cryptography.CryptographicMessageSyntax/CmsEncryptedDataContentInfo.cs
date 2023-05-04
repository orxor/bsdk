using System;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [UsedImplicitly]
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_ENCRYPTED)]
    public class CmsEncryptedDataContentInfo : CmsContentInfo
        {
        public Int32 Version { get; }
        public CmsEncryptedContentInfo EncryptedContentInfo { get; }
        internal CmsEncryptedDataContentInfo(Asn1Object o)
            : base(o)
            {
            State |= ObjectState.Failed;
            State &= ~ObjectState.DisposeUnderlyingObject;
            if (o[1][0].Count == 2) {
                if ((o[1][0][0] is Asn1Integer) &&
                    (o[1][0][1] is Asn1Sequence)) {
                    Version = (Asn1Integer)o[1][0][0];
                    EncryptedContentInfo = new CmsEncryptedContentInfo(o[1][0][1]);
                    if (!EncryptedContentInfo.IsFailed) {
                        State &= ~ObjectState.Failed;
                        State |= ObjectState.DisposeUnderlyingObject;
                        }
                    }
                }
            }
        }
    }