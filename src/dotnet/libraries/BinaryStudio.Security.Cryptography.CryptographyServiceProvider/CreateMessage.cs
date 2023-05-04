using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider.Properties;

namespace BinaryStudio.Security.Cryptography
    {
    using CERT_BLOB = CRYPT_BLOB;
    public abstract partial class CryptographicContext
        {
        #region M:CreateMessageSignature(Stream,Stream,IList<X509Certificate>,CryptographicMessageFlags,IRequestSecureString)
        public unsafe void CreateMessageSignature(Stream InputStream,Stream OutputStream,IList<X509Certificate> Signers,CryptographicMessageFlags Flags, RequestSecureString RequestSecureString) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            if (Signers == null) { throw new ArgumentNullException(nameof(Signers)); }
            if (IsNullOrEmpty(Signers)) { throw new ArgumentOutOfRangeException(nameof(Signers)); }
            if (Signers.Any(i => i == null)) { throw new ArgumentOutOfRangeException(nameof(Signers)); }
            if (Flags.HasFlag(CryptographicMessageFlags.Attached) && Flags.HasFlag(CryptographicMessageFlags.Detached)) {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(Flags),
                    message: String.Format(
                        PlatformContext.DefaultCulture,
                        Resources.InvalidFlagCombination,
                            nameof(CryptographicMessageFlags.Attached) + "," +
                            nameof(CryptographicMessageFlags.Detached)));
                }
            try
                {
                if (!Flags.HasFlag(CryptographicMessageFlags.SkipCertificateValidation)) {
                    Signers.ForAll(i => {
                        VerifyObject(i,CertificateChainPolicy.CERT_CHAIN_POLICY_BASE);
                        });
                    }
                var contextes = new CryptographicContext[Signers.Count];
                using (var manager = new LocalMemoryManager()) {
                    var si = Environment.Is64BitProcess
                        ? (CMSG_SIGNED_ENCODE_INFO)new CMSG_SIGNED_ENCODE_INFO64()
                        : (CMSG_SIGNED_ENCODE_INFO)new CMSG_SIGNED_ENCODE_INFO32();
                    si.Setup(Signers.Count, manager.Alloc(Signers.Count*(Environment.Is64BitProcess
                        ? sizeof(CMSG_SIGNER_ENCODE_INFO64)
                        : sizeof(CMSG_SIGNER_ENCODE_INFO32))));
                    if (Flags.HasFlag(CryptographicMessageFlags.IncludeSigningCertificate)) {
                        si.Certificates = (CERT_BLOB*)manager.Alloc(sizeof(CERT_BLOB)*si.SignerCount);
                        si.CertificateCount = si.SignerCount;
                        }
                    for (var i = 0; i < si.SignerCount; i++) {
                        var certificate = Signers[i];
                        var certinfo = (CERT_CONTEXT*)certificate.Handle;
                        contextes[i] = RequestSigningSecureString(certificate,RequestSecureString);
                        #if CMSG_SIGNER_ENCODE_INFO_HAS_CMS_FIELDS
                        si.Setup(i,contextes[i].KeySpec,contextes[i].Handle,certinfo->CertInfo,
                            (IntPtr)manager.StringToMem(certificate.HashAlgorithm.Value, Encoding.ASCII),
                            (IntPtr)manager.StringToMem(certificate.SignatureAlgorithm.Value, Encoding.ASCII));
                        #else
                        si.Setup(i,contextes[i].KeySpec,contextes[i].Handle,certinfo->CertInfo,
                            (IntPtr)manager.StringToMem(certificate.HashAlgorithm.Value, Encoding.ASCII));
                        #endif
                        if (Flags.HasFlag(CryptographicMessageFlags.IncludeSigningCertificate)) {
                            si.Certificates[i].Size = certinfo->CertEncodedSize;
                            si.Certificates[i].Data = certinfo->CertEncoded;
                            }
                        }
                    using (var message = CryptographicMessage.OpenToEncode(
                        (Bytes,Final)=> {
                            OutputStream?.Write(Bytes,0,Bytes.Length);
                            },
                        Flags.HasFlag(CryptographicMessageFlags.IndefiniteLength) ? CMSG_INDEFINITE_LENGTH : (UInt32)InputStream.Length,
                        Flags.HasFlag(CryptographicMessageFlags.Detached) ? CRYPT_OPEN_MESSAGE_FLAGS.CMSG_DETACHED_FLAG : CRYPT_OPEN_MESSAGE_FLAGS.CMSG_NONE,
                        CMSG_TYPE.CMSG_SIGNED, si))
                        {
                        var Block = new Byte[SIGNATURE_BUFFER_SIZE];
                        for (;;) {
                            Yield();
                            var sz = InputStream.Read(Block, 0, Block.Length);
                            if (sz == 0) { break; }
                            message.Update(Block, sz, false);
                            }
                        Block[0] = 0;
                        message.Update(Block, 0, true);
                        }
                    }
                }
            catch(Exception e)
                {
                e.Add("Flags",Flags);
                e.Add("Signers",Signers);
                throw;
                }
            }
        #endregion
        }
    }