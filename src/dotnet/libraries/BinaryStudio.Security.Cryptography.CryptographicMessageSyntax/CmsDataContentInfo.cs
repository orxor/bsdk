using System.Collections.Generic;
using System.Collections.ObjectModel;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using JetBrains.Annotations;

// ReSharper disable LocalVariableHidesMember

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    [CmsSpecific(ObjectIdentifiers.szOID_PKCS_7_DATA)]
    [UsedImplicitly]
    public class CmsDataContentInfo : CmsContentInfo
        {
        public new IList<CmsContentInfo> Content { get; }
        #region ctor{Asn1Object}
        public CmsDataContentInfo(Asn1Object source)
            : base(source)
            {
            Content = EmptyArray<CmsContentInfo>.Value;
            State |= ObjectState.Failed;
            State &= ~ObjectState.DisposeUnderlyingObject;
            if (source[1][0] is Asn1OctetString OctetString) {
                if (OctetString.Count > 0) {
                    var content = new List<CmsContentInfo>();
                    try
                        {
                        if (OctetString[0] is Asn1Sequence Sequence) {
                            foreach (var o in Sequence) {
                                content.Add(Create(o));
                                }
                            State &= ~ObjectState.Failed;
                            State |= ObjectState.DisposeUnderlyingObject;
                            }
                        }
                    finally
                        {
                        Content = new ReadOnlyCollection<CmsContentInfo>(content);
                        }
                    }
                }
            }
        #endregion
        }
    }