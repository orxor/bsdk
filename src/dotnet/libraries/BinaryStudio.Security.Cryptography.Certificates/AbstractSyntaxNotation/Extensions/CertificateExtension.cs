using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {   
    /// <summary>
    /// Represents an X509 extension.
    /// <p>The extensions defined for X.509 v3 certificates provide methods for associating additional attributes with
    /// users or public keys and for managing relationships between CAs. The X.509 v3 certificate format also allows
    /// communities to define private extensions to carry information unique to those communities. Each extension in
    /// a certificate is designated as either critical or non-critical. A certificate-using system MUST reject the
    /// certificate if it encounters a critical extension it does not recognize or a critical extension that contains
    /// information that it cannot process. A non-critical extension MAY be ignored if it is not recognized, but MUST
    /// be processed if it is recognized. Communities may elect to use additional extensions; however, caution ought
    /// to be exercised in adopting any critical extensions in certificates that might prevent use in a general context.</p>
    /// <p>Each extension includes an OID and an ASN.1 structure. When an extension appears in a certificate, the OID
    /// appears as the field <see cref="Identifier"/> and the corresponding ASN.1 DER encoded structure is the value
    /// of the octet string <see cref="Body"/>. A certificate MUST NOT include more than one instance of a particular
    /// extension. An extension includes the boolean <see cref="IsCritical"/>, with a default value of
    /// <see langword="false"/>.</p>
    /// </summary>
    [DebuggerDisplay(@"\{{" + nameof(ToString) + @"(),nq}\}")]
    public class CertificateExtension : Asn1LinkObject
        {
        public Oid Identifier { get;private set; }
        public Boolean IsCritical { get; }

        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Boolean IsExplicitConstructed { get { return base.IsExplicitConstructed; }}
        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Boolean IsImplicitConstructed { get { return base.IsImplicitConstructed; }}
        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Boolean IsIndefiniteLength { get { return base.IsIndefiniteLength; }}
        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Asn1Object UnderlyingObject { get { return base.UnderlyingObject; }}
        public new Asn1OctetString Body { get;protected set; }

        #region ctor{Asn1Object}
        protected internal CertificateExtension(Asn1Object source)
            : base(source)
            {
            var c = source.Count;
            var i = 1;
            Identifier = (Asn1ObjectIdentifier)source[0];
            if (c > i) {
                if ((source[i].Class == Asn1ObjectClass.Universal) && (((Asn1UniversalObject)source[i]).Type == Asn1ObjectType.Boolean)) {
                    IsCritical = (Asn1Boolean)source[i];
                    i++;
                    }
                }
            if (c > i) {
                Body = (Asn1OctetString)source[i];
                }
            }
        #endregion
        #region ctor{CertificateExtension}
        protected internal CertificateExtension(CertificateExtension source)
            : base(source)
            {
            Identifier = source.Identifier;
            IsCritical = source.IsCritical;
            Body = source.Body;
            }
        #endregion
        #region ctor{Oid,Boolean}
        protected CertificateExtension(Oid identifier, Boolean critial)
            : base(new Asn1PrivateObject(0))
            {
            Identifier = identifier;
            IsCritical = critial;
            Body = null;
            }
        #endregion
        #region ctor{String,Boolean}
        protected CertificateExtension(String identifier, Boolean critial)
            : base(new Asn1PrivateObject(0))
            {
            Identifier = new Oid(identifier);
            IsCritical = critial;
            Body = null;
            }
        #endregion

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return Identifier.Value;
            }

        private static readonly IDictionary<String, Type> types = new ConcurrentDictionary<String, Type>();
        private static readonly ReaderWriterLockSlim syncobject = new ReaderWriterLockSlim();

        /// <summary>
        /// Attempts to construct strong-typed extension instance from base extensions source.
        /// If library cannot recognize it then original source returns.
        /// </summary>
        /// <param name="source">Source containing unstructured extension data.</param>
        /// <returns>Returns suggested extension instance from source.</returns>
        internal static CertificateExtension From(CertificateExtension source) {
            if (ReferenceEquals(source, null)) { throw new ArgumentNullException(nameof(source)); }
            try
                {
                EnsureFactory();
                using (ReadLock(syncobject)) {
                    if (types.TryGetValue(source.Identifier.Value, out var type)) {
                        if (type.IsSubclassOf(typeof(CertificateExtension))) {
                            var r = (CertificateExtension)Activator.CreateInstance(type,
                                BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance,
                                null,new Object[]{ source },null);
                            return r;
                            }
                        }
                    }
                return source;
                }
            catch (Exception e)
                {
                e.Data["Identifier"] = source.Identifier.Value;
                e.Data["IsCritical"] = source.IsCritical;
                throw;
                }
            }

        #region M:EnsureFactory
        private static void EnsureFactory() {
            using (UpgradeableReadLock(syncobject)) {
                if (types.Count == 0) {
                    using (WriteLock(syncobject)) {
                        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(CertificateExtension)))) {
                            foreach (var attribute in type.GetCustomAttributes(typeof(Asn1CertificateExtensionAttribute), false).OfType<Asn1CertificateExtensionAttribute>())
                                {
                                types.Add(attribute.Key, type);
                                }
                            }
                        }
                    }
                }
            }
        #endregion

        /// <summary>
        /// Releases the unmanaged resources used by the instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            if (!State.HasFlag(ObjectState.Disposed)) {
                Identifier = null;
                Body = null;
                base.Dispose(disposing);
                State |= ObjectState.Disposed;
                }
            }

        public override void WriteTo(Stream target, Boolean force = false) {
            var o = Body;
            BuildBody(ref o);
            if (o == null) { throw new InvalidOperationException(); }
            Body = o;
            var r = new Asn1Sequence {
                IsExplicitConstructed = true
                };
            r.Add(new Asn1ObjectIdentifier(Identifier));
            if (IsCritical)
                {
                r.Add(new Asn1Boolean(true));
                }
            r.Add(Body);
            r.WriteTo(target, true);
            }

        protected virtual void BuildBody(ref Asn1OctetString o)
            {
            }

        protected void BuildBody()
            {
            var r = Body;
            BuildBody(ref r);
            Body = r;
            }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            throw new NotImplementedException()
                .Add("Type",GetType().FullName)
                .Add("Identifier",$"{Identifier} {{{OID.ResourceManager.GetString(Identifier.ToString(), CultureInfo.InvariantCulture)}}}");
            base.WriteTo(writer);
            }
        }
    }