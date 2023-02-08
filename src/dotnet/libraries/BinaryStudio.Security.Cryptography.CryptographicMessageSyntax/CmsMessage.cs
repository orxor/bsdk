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
        public Oid ContentType { get { return ContentInfo.ContentType; }}

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
                ContentInfo = CmsContentInfo.Create(o);
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
        }
    }