using System;
using System.IO;
using System.Security.Cryptography;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    public class CmsMessage : CmsObject
        {
        private String thumbprint;
        public String Thumbprint { get {
            if (thumbprint == null) {
                using (var engine = SHA1.Create())
                using(var output = new MemoryStream()) {
                    UnderlyingObject.WriteTo(output);
                    output.Seek(0, SeekOrigin.Begin);
                    thumbprint = engine.ComputeHash(output).ToString("x");
                    }
                }
            return thumbprint;
            }}

        public CmsContentInfo ContentInfo { get; }
        public Oid ContentType { get; }

        public CmsMessage(Asn1Object o)
            : base(o)
            {
            State |= ObjectState.Failed;
            State &= ~ObjectState.DisposeUnderlyingObject;
            if (o == null) { throw new ArgumentNullException(nameof(o)); }
            if ((o.Class == Asn1ObjectClass.Universal) && (o is Asn1Sequence)) {
                if (o.Count <= 1) { throw new ArgumentOutOfRangeException(nameof(o)); }
                if (!(o[0] is Asn1ObjectIdentifier))       { return; }
                if (!(o[1] is Asn1ContextSpecificObject))  { throw new ArgumentOutOfRangeException(nameof(o)); }
                if (((Asn1ContextSpecificObject)o[1]).Type != 0) { throw new ArgumentOutOfRangeException(nameof(o)); }
                ContentType = new Oid(((Asn1ObjectIdentifier)o[0]).ToString());
                switch (((Asn1ObjectIdentifier)o[0]).ToString()) {
                    case OID_Data:          { ContentInfo = new CmsDataContentInfo(o[1]);          } break;
                    case OID_SignedData:    { ContentInfo = new CmsSignedDataContentInfo(o[1]);    } break;
                    case OID_EnvelopedData: { ContentInfo = new CmsEnvelopedDataContentInfo(o[1]); } break;
                    case OID_SignEnvData:   { ContentInfo = new CmsSignEnvDataContentInfo(o[1]);   } break;
                    case OID_DigestedData:  { ContentInfo = new CmsDigestedDataContentInfo(o[1]);  } break;
                    case OID_EncryptedData: { ContentInfo = new CmsEncryptedDataContentInfo(o[1]); } break;
                    default : { throw new ArgumentOutOfRangeException(nameof(o)); }
                    }
                State &= ~ObjectState.Failed;
                State |= ObjectState.DisposeUnderlyingObject;
                }
            }

        /**
         * <summary>Gets the service object of the specified type.</summary>
         * <param name="service">An object that specifies the type of service object to get.</param>
         * <returns>A service object of type <paramref name="service"/>.-or- null if there is no service object of type <paramref name="service"/>.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override Object GetService(Type service) {
            if (service == typeof(CmsSignedDataContentInfo)) { return ContentInfo as CmsSignedDataContentInfo; }
            return base.GetService(service);
            }

        private const String OID_Data          = "1.2.840.113549.1.7.1";
        private const String OID_SignedData    = "1.2.840.113549.1.7.2";
        private const String OID_EnvelopedData = "1.2.840.113549.1.7.3";
        private const String OID_SignEnvData   = "1.2.840.113549.1.7.4";
        private const String OID_DigestedData  = "1.2.840.113549.1.7.5";
        private const String OID_EncryptedData = "1.2.840.113549.1.7.6";
        }
    }