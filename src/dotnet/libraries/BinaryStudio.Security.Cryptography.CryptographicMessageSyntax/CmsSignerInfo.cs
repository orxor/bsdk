using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    /**
     * [RFC5652]
     * SignerInfo ::= SEQUENCE
     * {
     *   version            CMSVersion,
     *   sid                SignerIdentifier,
     *   digestAlgorithm    DigestAlgorithmIdentifier,
     *   signedAttrs        [0] IMPLICIT SignedAttributes OPTIONAL,
     *   signatureAlgorithm SignatureAlgorithmIdentifier,
     *   signature          SignatureValue,
     *   unsignedAttrs      [1] IMPLICIT UnsignedAttributes OPTIONAL
     * }
     *
     * SignerIdentifier ::= CHOICE
     * {
     *   issuerAndSerialNumber IssuerAndSerialNumber,
     *   subjectKeyIdentifier [0] SubjectKeyIdentifier
     * }
     *
     * SignedAttributes   ::= SET SIZE (1..MAX) OF Attribute
     * UnsignedAttributes ::= SET SIZE (1..MAX) OF Attribute
     *
     * Attribute ::= SEQUENCE
     * {
     *   attrType   OBJECT IDENTIFIER,
     *   attrValues SET OF AttributeValue
     * }
     *
     * AttributeValue ::= ANY
     * SignatureValue ::= OCTET STRING
     */
    public class CmsSignerInfo : CmsObject
        {
        private const Int32 ORDER_VERSION           = 0;
        private const Int32 ORDER_SIGNER_IDENTIFIER = 1;
        private const Int32 ORDER_DIGEST_ALGORITHM  = 2;
        private const Int32 ORDER_SIGNED_ATTRIBUTES = 3;

        public Int32 Version { get; }
        public CmsSignerIdentifier SignerIdentifier { get; }
        public X509AlgorithmIdentifier DigestAlgorithm { get; }
        public X509AlgorithmIdentifier SignatureAlgorithm { get; }
        public ISet<CmsAttribute> SignedAttributes   { get; }
        ISet<CmsAttribute> UnsignedAttributes { get; }
        public Asn1OctetString SignatureValue { get; }

        public CmsSignerInfo(Asn1Object o)
            :base(o)
            {
            SignedAttributes   = new HashSet<CmsAttribute>();
            UnsignedAttributes = new HashSet<CmsAttribute>();
            if (o is Asn1Sequence u) {
                var c = u.Count;
                Version = (Asn1Integer)u[ORDER_VERSION];
                SignerIdentifier = CmsSignerIdentifier.Choice(u[ORDER_SIGNER_IDENTIFIER]);
                DigestAlgorithm = new X509AlgorithmIdentifier((Asn1Sequence)u[ORDER_DIGEST_ALGORITHM]);
                var i = ORDER_SIGNED_ATTRIBUTES;
                if (u[i] is Asn1ContextSpecificObject contextspecific) {
                    SignedAttributesContainer = contextspecific;
                    SignedAttributes.UnionWith(u[i].Select(j => CmsAttribute.From(new CmsAttribute(j))));
                    i++;
                    }
                var SignatureAlgorithmSource = u[i++] as Asn1Sequence;
                if (SignatureAlgorithmSource != null) {
                    SignatureAlgorithm = new X509AlgorithmIdentifier(SignatureAlgorithmSource);
                    SignatureValue = (Asn1OctetString)u[i++];
                    if (i < c) {
                        SignedAttributes.UnionWith(u[i].Select(j => CmsAttribute.From(new CmsAttribute(j))));
                        }
                    }
                }
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         */
        public override String ToString()
            {
            return SignerIdentifier.ToString();
            }

        private readonly Asn1ContextSpecificObject SignedAttributesContainer;
        }
    }