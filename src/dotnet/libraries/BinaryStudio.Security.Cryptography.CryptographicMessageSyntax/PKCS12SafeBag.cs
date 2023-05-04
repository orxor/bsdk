using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    /// <summary>
    /// SafeBag ::= SEQUENCE {
    ///   bagId         BAG-TYPE.&amp;id ({PKCS12BagSet}),
    ///   bagValue      [0] EXPLICIT BAG-TYPE.&amp;Type({PKCS12BagSet}{@bagId}),
    ///   bagAttributes SET OF PKCS12Attribute OPTIONAL
    ///   }
    /// </summary>
    public abstract class PKCS12SafeBag : CmsContentInfo
        {
        public IList<CmsAttribute> Attributes { get; }
        protected PKCS12SafeBag(Asn1Object o)
            : base(o)
            {
            Attributes = EmptyList<CmsAttribute>.Value;
            if (o.Count > 2) {
                Attributes = new Asn1ReadOnlyCollection<CmsAttribute>(o[2].
                    OfType<Asn1Sequence>().
                    Select(i => CmsAttribute.From(new CmsAttribute(i))).
                    ToList());
                }
            }
        }
    }