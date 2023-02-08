using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.CryptographicMessageSyntax
    {
    /**
     * [RFC5652]
     *
     * Attribute ::= SEQUENCE
     * {
     *   attrType   OBJECT IDENTIFIER,
     *   attrValues SET OF AttributeValue
     * }
     *
     * AttributeValue ::= ANY
     */
    [DefaultProperty(nameof(Value))]
    public class CmsAttribute : CmsObject
        {
        public Oid Type { get; }
        public virtual Object Value { get; }

        protected CmsAttribute(CmsAttribute o)
            : base(o)
            {
            Type  = o.Type;
            Value = o.Value;
            }

        internal CmsAttribute(Asn1Object o)
            : base(o)
            {
            Value = o[1];
            if (o is Asn1Sequence u) {
                Type  = (Asn1ObjectIdentifier)u[0];
                Value = new HashSet<Asn1Object>(u[1]);
                }
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         */
        public override String ToString()
            {
            return Type.ToString();
            }

        private static readonly IDictionary<String, Type> types = new ConcurrentDictionary<String, Type>();
        private static readonly ReaderWriterLockSlim syncobject = new ReaderWriterLockSlim();

        public static CmsAttribute From(CmsAttribute source) {
            if (ReferenceEquals(source, null)) { throw new ArgumentNullException(nameof(source)); }
            EnsureFactory();
            using (ReadLock(syncobject)) {
                if (types.TryGetValue(source.Type.ToString(), out var type)) {
                    if (type.IsSubclassOf(typeof(CmsAttribute))) {
                        return (CmsAttribute)Activator.CreateInstance(
                            type,
                            BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public,
                            null,
                            new Object[]{ source },
                            null);
                        }
                    }
                }
            return source;
            }

        #region M:EnsureFactory
        private static void EnsureFactory() {
            using (UpgradeableReadLock(syncobject)) {
                if (types.Count == 0) {
                    using (WriteLock(syncobject)) {
                        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(CmsAttribute)))) {
                            foreach (var attribute in type.GetCustomAttributes(typeof(CmsSpecificAttribute), false).OfType<CmsSpecificAttribute>())
                                {
                                types.Add(attribute.Key, type);
                                }
                            }
                        }
                    }
                }
            }
        #endregion
        }
    }