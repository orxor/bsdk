using System;
using System.Linq;
using System.Security.Cryptography;
using BinaryStudio.IO;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.CryptographicMessageSyntax;

namespace BinaryStudio.Security.Cryptography.PersonalInformationExchangeSyntax
    {
    public class PfxFile : Asn1LinkObject
        {
        public Int32 Version { get; }
        public CmsContentInfo ContentInfo { get; }
        public Oid ContentType { get { return ContentInfo.ContentType; }}

        #region ctor{Byte[]}
        public PfxFile(Byte[] o)
            : this(Load(new ReadOnlyMemoryMappingStream(o)).FirstOrDefault())
            {
            }
        #endregion
        #region ctor{Asn1Object}
        public PfxFile(Asn1Object o)
            : base(o)
            {
            State |= ObjectState.Failed;
            State &= ~ObjectState.DisposeUnderlyingObject;
            if (o == null) { throw new ArgumentNullException(nameof(o)); }
            if ((o.Count >= 2) && (o.Count <= 3)) {
                if ((o[0] is Asn1Integer) &&
                    (o[1] is Asn1Sequence))
                    {
                    Version = (Asn1Integer)o[0];
                    ContentInfo = CmsContentInfo.Create(o[1]);
                    State &= ~ObjectState.Failed;
                    State |= ObjectState.DisposeUnderlyingObject;
                    }
                }
            }
        #endregion
        }
    }
