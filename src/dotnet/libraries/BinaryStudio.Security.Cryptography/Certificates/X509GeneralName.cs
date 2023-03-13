using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public static class X509GeneralName
        {
        #region M:From(Asn1ContextSpecificObject):IX509GeneralName
        public static IX509GeneralName From(Asn1ContextSpecificObject source)
            {
            try
                {
                switch ((X509GeneralNameType)source.Type)
                    {
                    case X509GeneralNameType.Other:            { return new Asn1OtherName(source);       }
                    case X509GeneralNameType.RFC822:           { return new RFC822Name(source);          }
                    case X509GeneralNameType.DNS:              { return new Asn1DnsName(source);         }
                    case X509GeneralNameType.X400Address:      { return new X400Address(source);         }
                    case X509GeneralNameType.Directory:        { return X509RelativeDistinguishedNameSequence.Build(source[0]); }
                    case X509GeneralNameType.EDIParty:         { return new EdiPartyName(source);        }
                    case X509GeneralNameType.IA5String:        { return new Asn1Uri(source);             }
                    case X509GeneralNameType.OctetString:      { return new Asn1IpAddress(source);       }
                    case X509GeneralNameType.ObjectIdentifier: { return new Asn1RegisteredId(source);    }
                    default: throw new ArgumentOutOfRangeException(nameof(source));
                    }
                }
            catch (Exception e)
                {
                e.Data["Type"] = (X509GeneralNameType)source.Type;
                throw;
                }
            }
        #endregion
        #region M:BuildContextSpecificObject(SByte,IX509GeneralName):Asn1ContextSpecificObject
        internal static Asn1ContextSpecificObject BuildContextSpecificObject(SByte type, IX509GeneralName source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            switch(source.Type) {
                case X509GeneralNameType.Directory:
                    {
                    return new Asn1ContextSpecificObject(type, new Asn1ContextSpecificObject((SByte)(Int32)source.Type,
                        X509RelativeDistinguishedNameSequence.BuildSequence(((IEnumerable<KeyValuePair<Asn1ObjectIdentifier,String>>)source))));
                    }
                case X509GeneralNameType.Other:
                case X509GeneralNameType.RFC822:
                case X509GeneralNameType.DNS:
                case X509GeneralNameType.X400Address:
                case X509GeneralNameType.EDIParty:
                case X509GeneralNameType.IA5String:
                case X509GeneralNameType.OctetString:
                case X509GeneralNameType.ObjectIdentifier:
                default: throw new NotSupportedException();
                }
            return null;
            }
        #endregion

        public static String ToString(X509GeneralNameType type) {
            try
                {
                switch(type) {
                    case X509GeneralNameType.Other:            return "other";
                    case X509GeneralNameType.RFC822:           return "rfc822-name";
                    case X509GeneralNameType.DNS:              return "dns-name";
                    case X509GeneralNameType.X400Address:      return "x400-address";
                    case X509GeneralNameType.Directory:        return "dir-name";
                    case X509GeneralNameType.EDIParty:         return "edi-party";
                    case X509GeneralNameType.IA5String:        return "uri";
                    case X509GeneralNameType.OctetString:      return "ip-address";
                    case X509GeneralNameType.ObjectIdentifier: return "registered-id";
                    default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
                    }
                }
            catch (Exception e)
                {
                e.Data["Type"] = type;
                throw;
                }
            }
        }
    }
