using System;
using System.Collections.Generic;
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
    /**
     * <summary>
     * Represents an X.509 certificate.
     * </summary>
     * <translate lang="ru-RU">
     *   <summary>
     *   Представляет сертификат X.509.
     *   </summary>
     * </translate>
     * */
    public sealed class X509Certificate : X509Object,IExceptionSerializable,DigestSource
        {
        private IntPtr Context;
        internal Asn1Certificate Source;

        /**
         * <summary>
         * Gets a handle to a certificate context described by
         * an unmanaged <see cref="CERT_CONTEXT"/> structure.
         * </summary>
         */
        public override IntPtr Handle { get { return Context; }}

        /**
         * <summary>
         * The X.509 version number.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Получает версию формата сертификата X.509.
         *   </summary>
         * </translate>
         */
        public Int32 Version { get { return Source.Version; }}

        /**
         * <summary>
         * The unique serial number that the issuing certification authority (CA) assigns to the certificate.
         * The serial number is unique for all certificates issued by a given CA.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Уникальный серийный номер, присвоенный сертификату центром сертификации.
         *   Серийный номер уникален для всех сертификатов, выданных определенным центром сертификации.
         *   </summary>
         * </translate>
         */
        public String SerialNumber { get { return Source.SerialNumber; }}

        /**
         * <summary>
         * The digest (or thumbprint) of the certificate data.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Сводка данных (или отпечаток) сертификата.
         *   </summary>
         * </translate>
         */
        public String Thumbprint { get { return Source.Thumbprint; }}

        /**
         * <summary>
         * The beginning date for the period in which the certificate is valid.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Начальная дата периода действия сертификата.
         *   </summary>
         * </translate>
         */
        public DateTime NotBefore { get { return Source.NotBefore; }}

        /**
         * <summary>
         * The final date for the period in which the certificate is valid.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Конечная дата периода действия сертификата.
         *   </summary>
         * </translate>
         */
        public DateTime NotAfter   { get { return Source.NotAfter; }}

        /**
         * <summary>
         * Information regarding the CA that issued the certificate.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Информация о центре сертификации, выдавшем сертификат.
         *   </summary>
         * </translate>
         */
        public String Issuer { get { return Source.Issuer.ToString(); }}

        /**
         * <summary>
         * The name of the individual, computer, device, or CA to whom the certificate is issued.
         * If the issuing CA exists on a domain member server in your enterprise, this will be a distinguished name
         * within the enterprise. Otherwise, this may be a full name and e-mail name or other personal identifier.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Имя лица, компьютера, устройства или центра сертификации, которому выдан сертификат.
         *   Если выдающий сертификаты центр сертификации находится на рядовом сервере домена предприятия,
         *   это будет различающееся имя внутри предприятия. Иначе это может быть полное имя и адрес
         *   электронной почты или другой персональный идентификатор.
         *   </summary>
         * </translate>
         */
        public String Subject { get { return Source.Subject.ToString(); }}

        /**
         * <summary>
         * The country code associated with the certificate.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Код страны, ассоциированной с сертификатом.
         *   </summary>
         * </translate>
         */
        public String Country { get { return Source.Country; }}

        /**
         * <summary>
         * Gets the algorithm used to create the signature of a certificate.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Получает алгоритм, используемый для создания подписи сертификата.
         *   </summary>
         * </translate>
         */
        public Oid SignatureAlgorithm { get; }

        /**
         * <summary>
         * Gets the hash algorithm used to create the signature of a certificate.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Получает алгоритм хэширования, используемый для создания подписи сертификата.
         *   </summary>
         * </translate>
         */
        public Oid HashAlgorithm { get; }

        /**
         * <summary>
         * The name of the container associated with the certificate.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Имя контейнера, ассоциированного с сертификатом.
         *   </summary>
         * </translate>
         */
        public String Container {get;internal set; }
        public KEY_SPEC_TYPE KeySpec { get;internal set; }
        internal Boolean IsMachineKeySet { get;set; }

        /**
         * <summary>
         * Gets the raw data of a certificate.
         * </summary>
         * <translate lang="ru-RU">
         *   <summary>
         *   Бинарный образ сертификата.
         *   </summary>
         * </translate>
         */
        public unsafe Byte[] Bytes { get{
            var context = (CERT_CONTEXT*)Context;
            var r = new Byte[context->CertEncodedSize];
            for (var i = 0; i < context->CertEncodedSize; ++i) {
                r[i] = context->CertEncoded[i];
                }
            return r;
            }}

        IEnumerable<Byte[]> DigestSource.DigestSource { get { return Source.DigestSource; }}

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
        /** <inheritdoc/> */
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
        /// <inheritdoc />
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

        /**
         * <summary>Serves as the default hash function.</summary>
         * <returns>A hash code for the current object.</returns>
         */
        public override Int32 GetHashCode() {
            return (Handle != IntPtr.Zero)
                ? Thumbprint.GetHashCode()
                : 0;
            }

        /**
         * <summary>Returns a string that represents the current object.</summary>
         * <returns>A string that represents the current object.</returns>
         * <translate lang="ru-RU">
         *   <summary>Возвращает строку, представляющую текущий объект.</summary>
         *   <returns>Строка, представляющая текущий объект.</returns>
         * </translate>
         */
        public override String ToString()
            {
            return Source.FriendlyName;
            }

        /** <inheritdoc/> */
        public override Object GetService(Type service) {
            if (service == typeof(Asn1Certificate)) { return Source; }
            return base.GetService(service);
            }
        }
    }