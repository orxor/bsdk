﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Internal;

namespace BinaryStudio.Security.Cryptography
    {
    public abstract partial class CryptographicContext
        {
        #region M:VerifyAttachedMessageSignature(Stream)
        public virtual void VerifyAttachedMessageSignature(Stream InputStream) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            VerifyAttachedMessageSignature(InputStream,null,out var signers);
            }
        #endregion
        #region M:VerifyAttachedMessageSignature(Stream,Stream,{out}IList<X509Certificate>)
        public virtual void VerifyAttachedMessageSignature(Stream InputStream,Stream OutputStream,out IList<X509Certificate> Signers) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            if (!VerifyAttachedMessageSignature(out var e,InputStream,OutputStream,out Signers,null)) {
                throw e;
                }
            }
        #endregion
        #region M:VerifyAttachedMessageSignature({out}Exception,Stream,Stream,{out}IList<X509Certificate>,X509CertificateResolver)
        public virtual Boolean VerifyAttachedMessageSignature(out Exception e,Stream InputStream,Stream OutputStream,out IList<X509Certificate> Signers, X509CertificateResolver resolver) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            EnsureEntries(out var entries);
            e = null;
            Signers = new List<X509Certificate>();
            try
                {
                using (var message = CryptographicMessage.OpenToDecode((Bytes,Final)=> { OutputStream?.Write(Bytes,0,Bytes.Length); })) {
                    var Block = new Byte[SIGNATURE_BUFFER_SIZE];
                    var count = 0;
                    for (;;) {
                        Yield();
                        var sz = InputStream.Read(Block, 0, Block.Length);
                        if (sz == 0) { break; }
                        message.Update(Block, sz, false);
                        count += sz;
                        }
                    if (count == 0) { throw HResultException.GetExceptionForHR(HRESULT.CRYPT_E_STREAM_INSUFFICIENT_DATA); }
                    message.Update(EmptyArray<Byte>.Value, 0, true);
                    using (var store = new MessageCertificateStorage(message.Handle)) {
                        for (var signerindex = 0;; signerindex++) {
                            if (message.GetParameter(CMSG_PARAM.CMSG_SIGNER_CERT_INFO_PARAM, signerindex, out var hr, out var r)) {
                                unsafe {
                                    fixed (Byte* blob = r) {
                                        var digest    = message.GetParameter(CMSG_PARAM.CMSG_COMPUTED_HASH_PARAM, signerindex);
                                        var encdigest = message.GetParameter(CMSG_PARAM.CMSG_ENCRYPTED_DIGEST,    signerindex);
                                        var certinfo = (CERT_INFO*)blob;
                                        var certificate = store.Find(certinfo);
                                        if (certificate == null) { throw new Exception(); }
                                        if (certificate != null) {
                                            Signers.Add(certificate);
                                            VerifyObject(certificate,CertificateChainPolicy.CERT_CHAIN_POLICY_BASE);
                                            if (!message.Control(
                                                CRYPT_MESSAGE_FLAGS.CRYPT_MESSAGE_NONE,
                                                CMSG_CTRL.CMSG_CTRL_VERIFY_SIGNATURE,
                                                (IntPtr)((CERT_CONTEXT*)certificate.Handle)->CertInfo))
                                                {
                                                throw HResultException.GetExceptionForHR((HRESULT)Entries.GetLastError());
                                                }
                                            }
                                        }
                                    }
                                }
                            else
                                {
                                if (hr == HRESULT.CRYPT_E_INVALID_INDEX) { break; }
                                throw HResultException.GetExceptionForHR(hr);
                                }
                            }
                        }
                    }
                return true;
                }
            catch(Exception x)
                {
                e = x;
                return false;
                }
            finally
                {
                Signers = new ReadOnlyCollection<X509Certificate>(Signers);
                }
            }
        #endregion
        #region M:VerifyDetachedMessageSignature({out}Exception,Stream,Stream,{out}IList<X509Certificate>,X509CertificateResolver)
        public virtual Boolean VerifyDetachedMessageSignature(out Exception e,Stream SignatureStream,Stream DataStream,out IList<X509Certificate> Signers, X509CertificateResolver resolver) {
            if (SignatureStream == null) { throw new ArgumentNullException(nameof(SignatureStream)); }
            if (DataStream == null) { throw new ArgumentNullException(nameof(DataStream)); }
            EnsureEntries(out var entries);
            e = null;
            Signers = new List<X509Certificate>();
            try
                {
                using (var message = CryptographicMessage.OpenToDecode(CRYPT_OPEN_MESSAGE_FLAGS.CMSG_DETACHED_FLAG)) {
                    var Block = new Byte[SIGNATURE_BUFFER_SIZE];
                    #region reading signature
                    for (;;) {
                        Yield();
                        var sz = SignatureStream.Read(Block, 0, Block.Length);
                        if (sz == 0) { break; }
                        message.Update(Block, sz, false);
                        }
                    message.Update(EmptyArray<Byte>.Value, 0, true);
                    #endregion
                    #region reading data
                    for (;;) {
                        Yield();
                        var sz = DataStream.Read(Block, 0, Block.Length);
                        if (sz == 0) { break; }
                        message.Update(Block, sz, false);
                        }
                    message.Update(EmptyArray<Byte>.Value, 0, true);
                    #endregion
                    using (var store = new MessageCertificateStorage(message.Handle)) {
                        for (var signerindex = 0;; signerindex++) {
                            if (message.GetParameter(CMSG_PARAM.CMSG_SIGNER_CERT_INFO_PARAM, signerindex, out var hr, out var r)) {
                                unsafe {
                                    fixed (Byte* blob = r) {
                                        var digest    = message.GetParameter(CMSG_PARAM.CMSG_COMPUTED_HASH_PARAM, signerindex);
                                        var encdigest = message.GetParameter(CMSG_PARAM.CMSG_ENCRYPTED_DIGEST,    signerindex);
                                        var certinfo = (CERT_INFO*)blob;
                                        var certificate = store.Find(certinfo);
                                        if (certificate == null) { throw new Exception(); }
                                        if (certificate != null) {
                                            Signers.Add(certificate);
                                            VerifyObject(certificate,CertificateChainPolicy.CERT_CHAIN_POLICY_BASE);
                                            if (!message.Control(
                                                CRYPT_MESSAGE_FLAGS.CRYPT_MESSAGE_NONE,
                                                CMSG_CTRL.CMSG_CTRL_VERIFY_SIGNATURE,
                                                (IntPtr)((CERT_CONTEXT*)certificate.Handle)->CertInfo))
                                                {
                                                throw HResultException.GetExceptionForHR((HRESULT)Entries.GetLastError());
                                                }
                                            }
                                        }
                                    }
                                }
                            else
                                {
                                if (hr == HRESULT.CRYPT_E_INVALID_INDEX) { break; }
                                throw HResultException.GetExceptionForHR(hr);
                                }
                            }
                        }
                    }
                return true;
                }
            catch(Exception x)
                {
                e = x;
                return false;
                }
            finally
                {
                Signers = new ReadOnlyCollection<X509Certificate>(Signers);
                }
            }
        #endregion
        #region M:VerifySignature(X509Certificate,X509Certificate)
        public void VerifySignature(X509Certificate subject, X509Certificate issuer) {
            if (subject == null) { throw new ArgumentNullException(nameof(subject)); }
            if (issuer == null) { throw new ArgumentNullException(nameof(issuer)); }
            EnsureEntries(out var entries);
            Validate(entries.CryptVerifyCertificateSignature(Handle,
                CRYPT_VERIFY_CERT_SIGN_SUBJECT_CERT,subject.Handle,
                CRYPT_VERIFY_CERT_SIGN_ISSUER_CERT,issuer.Handle,0));
            }
        #endregion
        #region M:VerifySignature({out}Exception,X509Certificate,X509Certificate):Boolean
        public Boolean VerifySignature(out Exception e, X509Certificate subject, X509Certificate issuer) {
            if (subject == null) { throw new ArgumentNullException(nameof(subject)); }
            if (issuer == null) { throw new ArgumentNullException(nameof(issuer)); }
            e = null;
            try
                {
                VerifySignature(subject, issuer);
                return true;
                }
            catch (Exception exception)
                {
                e = exception;
                return false;
                }
            }
        #endregion
        #region M:VerifySignature(X509CertificateRevocationList,X509Certificate)
        public void VerifySignature(X509CertificateRevocationList subject, X509Certificate issuer) {
            if (subject == null) { throw new ArgumentNullException(nameof(subject)); }
            if (issuer == null) { throw new ArgumentNullException(nameof(issuer)); }
            EnsureEntries(out var entries);
            Validate(entries.CryptVerifyCertificateSignature(Handle,
                CRYPT_VERIFY_CERT_SIGN_SUBJECT_CRL,subject.Handle,
                CRYPT_VERIFY_CERT_SIGN_ISSUER_CERT,issuer.Handle,0));
            }
        #endregion
        #region M:VerifySignature({out}Exception,X509CertificateRevocationList,X509Certificate):Boolean
        public Boolean VerifySignature(out Exception e, X509CertificateRevocationList subject, X509Certificate issuer) {
            if (subject == null) { throw new ArgumentNullException(nameof(subject)); }
            if (issuer == null) { throw new ArgumentNullException(nameof(issuer)); }
            e = null;
            try
                {
                VerifySignature(subject, issuer);
                return true;
                }
            catch (Exception exception)
                {
                e = exception;
                return false;
                }
            }
        #endregion

        private const Int32 CRYPT_VERIFY_CERT_SIGN_SUBJECT_BLOB                       = 1;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_SUBJECT_CERT                       = 2;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_SUBJECT_CRL                        = 3;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_SUBJECT_OCSP_BASIC_SIGNED_RESPONSE = 4;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_ISSUER_PUBKEY                      = 1;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_ISSUER_CERT                        = 2;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_ISSUER_CHAIN                       = 3;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_ISSUER_NULL                        = 4;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_DISABLE_MD2_MD4_FLAG          = 0x00000001;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_SET_STRONG_PROPERTIES_FLAG    = 0x00000002;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_RETURN_STRONG_PROPERTIES_FLAG = 0x00000004;
        private const Int32 CRYPT_VERIFY_CERT_SIGN_CHECK_WEAK_HASH_FLAG          = 0x00000008;
        }
    }