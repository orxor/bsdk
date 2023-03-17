using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    /// <summary>
    /// Represents an X.509 certificate.
    /// </summary>
    public sealed class X509Certificate : X509Object,IExceptionSerializable
        {
        private IntPtr Context;
        internal Asn1Certificate Source;

        public override IntPtr Handle { get { return Context; }}
        public Int32 Version       { get { return Source.Version;      }}
        public String SerialNumber { get { return Source.SerialNumber; }}
        public String Thumbprint   { get { return Source.Thumbprint;   }}
        public DateTime NotBefore  { get { return Source.NotBefore;    }}
        public DateTime NotAfter   { get { return Source.NotAfter;     }}
        public String Issuer       { get { return Source.Issuer.ToString();  }}
        public String Subject      { get { return Source.Subject.ToString(); }}
        public String Country      { get { return Source.Country; }}
        public Oid SignatureAlgorithm { get; }
        public Oid HashAlgorithm { get; }
        public String Container {get;internal set; }
        public KEY_SPEC_TYPE KeySpec { get;internal set; }
        internal Boolean IsMachineKeySet { get;set; }

        public unsafe Byte[] Bytes { get{
            var context = (CERT_CONTEXT*)Context;
            var r = new Byte[context->CertEncodedSize];
            for (var i = 0; i < context->CertEncodedSize; ++i) {
                r[i] = context->CertEncoded[i];
                }
            return r;
            }}

        #region ctor{IntPtr}
        public X509Certificate(IntPtr context) {
            if (context == IntPtr.Zero) { throw new ArgumentOutOfRangeException(nameof(context)); }
            Context = Entries.CertDuplicateCertificateContext(context);
            Source = BuildSource(Context);
            SignatureAlgorithm = Source.SignatureAlgorithm.SignatureAlgorithm;
            HashAlgorithm = Source.SignatureAlgorithm.HashAlgorithm;
            }
        #endregion
        #region ctor{Asn1Certificate}
        public X509Certificate(Asn1Certificate source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Source = source;
            Context = CertCreateCertificateContext(source.UnderlyingObject.Body);
            if (Context == IntPtr.Zero) {
                var hr = GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR((Int32)hr);
                }
            SignatureAlgorithm = Source.SignatureAlgorithm.SignatureAlgorithm;
            HashAlgorithm = Source.SignatureAlgorithm.HashAlgorithm;
            }
        #endregion
        #region ctor{Byte[]}
        public X509Certificate(Byte[] source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Source = BuildSource(source);
            Context = CertCreateCertificateContext(source);
            if (Context == IntPtr.Zero) {
                var hr = GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR((Int32)hr);
                }
            SignatureAlgorithm = Source.SignatureAlgorithm.SignatureAlgorithm;
            HashAlgorithm = Source.SignatureAlgorithm.HashAlgorithm;
            }
        #endregion
        #region ctor{Byte[],CryptKey}
        internal unsafe X509Certificate(Byte[] source,CryptKey key) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (key == null) { throw new ArgumentOutOfRangeException(nameof(key)); }
            Source = BuildSource(source);
            Context = Validate(Entries.CertCreateCertificateContext(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,source,source.Length),NotZero);
            SignatureAlgorithm = Source.SignatureAlgorithm.SignatureAlgorithm;
            HashAlgorithm = Source.SignatureAlgorithm.HashAlgorithm;
            KeySpec = key.KeySpec;
            using (var manager = new LocalMemoryManager()) {
                var pi = new CRYPT_KEY_PROV_INFO {
                    ContainerName = 
                        (Environment.OSVersion.Platform <= PlatformID.WinCE)
                         ? (IntPtr)manager.StringToMem(key.Context.FullQualifiedContainerName, Entries.UnicodeEncoding)
                         : (IntPtr)manager.StringToMem(key.Context.Container, Entries.UnicodeEncoding),
                    ProviderName = (IntPtr)manager.StringToMem(key.Context.ProviderName, Entries.UnicodeEncoding),
                    ProviderFlags = key.Context.ProviderFlags & (~CryptographicContextFlags.CRYPT_SILENT),
                    ProviderType = key.Context.ProviderType,
                    KeySpec = key.KeySpec
                    };
                //SetProperty(CERT_PROP_ID.CERT_KEY_PROV_INFO_PROP_ID, 0, ref pi);
                }
            }
        #endregion

        #region M:BuildSource(CERT_CONTEXT*):Asn1Certificate
        private static unsafe Asn1Certificate BuildSource(CERT_CONTEXT* Context) {
            if (Context == null) { throw new ArgumentNullException(nameof(Context)); }
            var Size  = Context->CertEncodedSize;
            var Bytes = Context->CertEncoded;
            var Buffer = new Byte[Size];
            for (var i = 0U; i < Size; ++i) {
                Buffer[i] = Bytes[i];
                }
            return BuildSource(Buffer);
            }
        #endregion
        #region M:BuildSource(IntPtr):Asn1Certificate
        private static unsafe Asn1Certificate BuildSource(IntPtr Context) {
            if (Context == IntPtr.Zero) { throw new ArgumentException(nameof(Context)); }
            return BuildSource((CERT_CONTEXT*)Context);;
            }
        #endregion
        #region M:BuildSource(Byte[]):Asn1Certificate
        private static Asn1Certificate BuildSource(Byte[] Context) {
            if (Context == null) { throw new ArgumentException(nameof(Context)); }
            var r = new Asn1Certificate(Asn1Object.Load(new ReadOnlyMemoryMappingStream(Context)).FirstOrDefault());
            if (r.IsFailed) {
                throw new InvalidDataException();
                }
            return r;
            }
        #endregion
        #region M:SetProperty(CERT_PROP_ID,{ref}CRYPT_KEY_PROV_INFO)
        private void SetProperty(CERT_PROP_ID index, Int32 flags, ref CRYPT_KEY_PROV_INFO value) {
            Validate(Entries,Entries.CertSetCertificateContextProperty(Handle, index, flags, ref value));
            }
        #endregion
        #region M:GetProperty(CERT_PROP_ID,{out}CRYPT_KEY_PROV_INFO):HRESULT
        internal unsafe HRESULT GetProperty(CERT_PROP_ID index, out CRYPT_KEY_PROV_INFO value) {
            value = new CRYPT_KEY_PROV_INFO();
            var r = GetProperty(index, out Byte[] o);
            if (r != HRESULT.S_OK) { return r; }
            fixed (Byte* B = o) {
                value = *(CRYPT_KEY_PROV_INFO*)B;
                }
            return HRESULT.S_OK;
            }
        #endregion
        #region M:GetProperty(CERT_PROP_ID,{out}Byte[]):HRESULT
        private HRESULT GetProperty(CERT_PROP_ID index,out Byte[] r) {
            r = EmptyArray<Byte>.Value;
            var c = 0;
            if (!Entries.CertGetCertificateContextProperty(Handle, index, null, ref c)) { return (HRESULT)Entries.GetLastError(); } r = new Byte[c];
            if (!Entries.CertGetCertificateContextProperty(Handle, index, r,    ref c)) { return (HRESULT)Entries.GetLastError(); }
            return HRESULT.S_OK;
            }
        #endregion
        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the instance and optionally releases the managed resources.</summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(Boolean disposing) {
            lock(this) {
                if (!Disposed) {
                    base.Dispose(disposing);
                    Dispose(ref Source);
                    if (Context != IntPtr.Zero) {
                        Entries.CertFreeCertificateContext(Context);
                        Context = IntPtr.Zero;
                        }
                    }
                }
            }
        #endregion
        #region M:CertCreateCertificateContext(Byte[]):IntPtr
        private static IntPtr CertCreateCertificateContext(Byte[] source)
            {
            return Entries.CertCreateCertificateContext(X509_ASN_ENCODING|PKCS_7_ASN_ENCODING,source,source.Length);
            }
        #endregion

        #region M:Verify(CertificateChainPolicy)
        public void Verify(CertificateChainPolicy policy) {
            CryptographicContext.DefaultContext.VerifyObject(this, policy);
            }
        #endregion

        #region M:IExceptionSerializable.WriteTo(TextWriter)
        void IExceptionSerializable.WriteTo(TextWriter target) {
            using (var writer = new DefaultJsonWriter(new JsonTextWriter(target){
                    Formatting = Formatting.Indented,
                    Indentation = 2,
                    IndentChar = ' '
                    })) {
                ((IExceptionSerializable)this).WriteTo(writer);
                }
            }
        #endregion
        #region M:IExceptionSerializable.WriteTo(IJsonWriter)
        void IExceptionSerializable.WriteTo(IJsonWriter writer) {
            using (writer.Object()) {
                writer.WriteValue(nameof(NotBefore),NotBefore);
                writer.WriteValue(nameof(NotAfter),NotAfter);
                writer.WriteValue(nameof(SerialNumber),SerialNumber);
                writer.WriteValue(nameof(Subject),Subject);
                writer.WriteValue(nameof(Issuer),Issuer);
                writer.WriteValue(nameof(Thumbprint),Thumbprint);
                }
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="IJsonWriter"/> to write to.</param>
        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValue(nameof(Version),Version);
                writer.WriteValue(nameof(NotBefore),NotBefore);
                writer.WriteValue(nameof(NotAfter),NotAfter);
                writer.WriteValue(nameof(SerialNumber),SerialNumber);
                writer.WriteValue(nameof(Subject),Subject);
                writer.WriteValue(nameof(Issuer),Issuer);
                writer.WriteValue(nameof(Thumbprint),Thumbprint);
                writer.WriteValue(nameof(Source),Source);
                }
            }
        #endregion

        public override Object GetService(Type service) {
            if (service == typeof(Asn1Certificate)) { return Source; }
            return base.GetService(service);
            }
        }
    }