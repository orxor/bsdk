using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Properties;
using BinaryStudio.Serialization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Extensions
    {   
    [DebuggerDisplay(@"\{{" + nameof(ToString) + @"(),nq}\}")]
    public class Asn1CertificateExtension : Asn1LinkObject
        {
        public Oid Identifier { get;private set; }
        public Boolean IsCritical { get; }

        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Boolean IsExplicitConstructed { get { return base.IsExplicitConstructed; }}
        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Boolean IsImplicitConstructed { get { return base.IsImplicitConstructed; }}
        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Boolean IsIndefiniteLength { get { return base.IsIndefiniteLength; }}
        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Asn1Object UnderlyingObject { get { return base.UnderlyingObject; }}
        [Browsable(false)][DebuggerBrowsable(DebuggerBrowsableState.Never)] public Asn1OctetString Body { get;protected set; }

        protected internal Asn1CertificateExtension(Asn1Object source)
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

        protected internal Asn1CertificateExtension(Asn1CertificateExtension source)
            : base(source)
            {
            Identifier = source.Identifier;
            IsCritical = source.IsCritical;
            Body = source.Body;
            }

        protected Asn1CertificateExtension(Oid identifier, Boolean critial)
            : base(new Asn1PrivateObject(0))
            {
            Identifier = identifier;
            IsCritical = critial;
            Body = null;
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <filterpriority>2</filterpriority>
         * */
        public override String ToString()
            {
            return Identifier.ToString();
            }

        private static readonly IDictionary<String, Type> types = new ConcurrentDictionary<String, Type>();
        private static readonly ReaderWriterLockSlim syncobject = new ReaderWriterLockSlim();

        public static Asn1CertificateExtension From(Asn1CertificateExtension source) {
            if (ReferenceEquals(source, null)) { throw new ArgumentNullException(nameof(source)); }
            try
                {
                EnsureFactory();
                using (ReadLock(syncobject)) {
                    if (types.TryGetValue(source.Identifier.ToString(), out var type)) {
                        if (type.IsSubclassOf(typeof(Asn1CertificateExtension))) {
                            var r = (Asn1CertificateExtension)Activator.CreateInstance(type, source);
                            return r;
                            }
                        }
                    }
                return source;
                }
            catch (Exception e)
                {
                e.Data["Identifier"] = source.Identifier.ToString();
                e.Data["IsCritical"] = source.IsCritical;
                throw;
                }
            }

        #region M:EnsureFactory
        private static void EnsureFactory() {
            using (UpgradeableReadLock(syncobject)) {
                if (types.Count == 0) {
                    using (WriteLock(syncobject)) {
                        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(Asn1CertificateExtension)))) {
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