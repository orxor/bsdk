using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    /// <summary>
    /// The <see cref="CmsContentInfo"/> class represents the CMS ContentInfo
    /// data structure as defined in the CMS standards document.<br/>This data
    /// structure is the basis for all CMS messages.
    /// </summary>
    public class CmsContentInfo : CmsObject
        {
        public Oid ContentType { get; }
        protected Asn1ContextSpecificObject Context { get; }

        #region ctor{Asn1Object}
        /// <summary>
        /// Creates an instance of the <see cref="CmsContentInfo"/> class by using an ASN1 object as the data.
        /// </summary>
        /// <param name="o">An ASN1 object that represents the data from which to create the <see cref="CmsContentInfo"/> object.</param>
        protected CmsContentInfo(Asn1Object o)
            : base(o)
            {
            if (o is null) { throw new ArgumentNullException(nameof(o)); }
            ContentType = (Asn1ObjectIdentifier)o[0];
            Context = o.OfType<Asn1ContextSpecificObject>().FirstOrDefault();
            if (Context == null) { throw new ArgumentOutOfRangeException(nameof(o)); }
            if (Context.Type != 0) { throw new ArgumentOutOfRangeException(nameof(o)); }
            }
        #endregion

        #region M:Create(Asn1Object):CmsContentInfo
        public static CmsContentInfo Create(Asn1Object o) {
            if (o is null) { throw new ArgumentNullException(nameof(o)); }
            var key = ((Asn1ObjectIdentifier)o[0]).ToString();
            if (Types.TryGetValue(key, out var type)) {
                //var ctor = type.GetConstructor(BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public, null, new[]{ typeof(Asn1Object)}, null);
                return (CmsContentInfo)Activator.CreateInstance(
                    type,BindingFlags.NonPublic | BindingFlags.Instance,
                    null,new Object[]{ o },null);
                }
            throw new ArgumentOutOfRangeException(nameof(o),key,"Specified argument was out of the range of valid values.");
            }
        #endregion
        #region M:ToString:String
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return ContentType.FriendlyName;
            }
        #endregion

        private readonly static IDictionary<String,Type> Types = new Dictionary<String,Type>();
        static CmsContentInfo()
            {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(CmsContentInfo)))) {
                foreach (var attribute in type.GetCustomAttributes(typeof(CmsSpecificAttribute), false).OfType<CmsSpecificAttribute>()) {
                    Types.Add(attribute.Key, type);
                    }
                }
            }
        }
    }