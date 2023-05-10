using System;
using System.IO;
using System.Text;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider.Properties;
using BinaryStudio.Services;

namespace BinaryStudio.Security.Cryptography.Specific.Fintech
    {
    public class FintechMessageProvider : CryptographicObject
        {
        public override IntPtr Handle { get { return Context.Handle; }}
        public CryptographicContext Context { get; }
        public FintechMessageProvider(CryptographicContext context)
            {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            Context = context;
            }

        public void CreateMessageSignature(Stream InputStream,Stream OutputStream,X509Certificate Signer,CryptographicMessageFlags Flags, RequestSecureString RequestSecureString) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            if (Signer == null) { throw new ArgumentNullException(nameof(Signer)); }
            try
                {
                if (Flags.HasFlag(CryptographicMessageFlags.Attached) && Flags.HasFlag(CryptographicMessageFlags.Detached)) {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(Flags),
                        message: String.Format(
                            PlatformContext.DefaultCulture,
                            Resources.InvalidFlagCombination,
                                nameof(CryptographicMessageFlags.Attached) + "," +
                                nameof(CryptographicMessageFlags.Detached)));
                    }
                if (!Flags.HasFlag(CryptographicMessageFlags.SkipCertificateValidation)) { Context.VerifyObject(Signer,CertificateChainPolicy.CERT_CHAIN_POLICY_BASE); }
                var entries = (CryptographicFunctions)Context.GetService(typeof(CryptographicFunctions));
                if (Flags.HasFlag(CryptographicMessageFlags.Attached))
                    {
                    using (var context = Context.RequestSigningSecureString(Signer,RequestSecureString)) {
                        using (var hash = new CryptHashAlgorithm(context,Signer.HashAlgorithm)) {
                            Write(OutputStream,FT_SIGNATURE_CONTAINER_HEADER_MAGIC);
                            Write(OutputStream,Signer.HashAlgorithm.Value);
                            Write(OutputStream,Signer.Bytes);
                            Write(OutputStream,FT_SIGNATURE_CONTAINER_BARRIER_MAGIC);
                            hash.Compute(InputStream, (block, size) => {
                                Write(OutputStream,block,size);
                                });
                            hash.SignHash(Signer.KeySpec,out var digest,out var signature);
                            Write(OutputStream,signature);
                            Write(OutputStream,signature.Length);
                            }
                        }
                    }
                else if (Flags.HasFlag(CryptographicMessageFlags.Detached))
                    {
                    using (var context = Context.RequestSigningSecureString(Signer,RequestSecureString)) {
                        using (var hash = new CryptHashAlgorithm(context,Signer.HashAlgorithm)) {
                            hash.Compute(InputStream);
                            hash.SignHash(Signer.KeySpec,out var digest,out var signature);
                            Write(OutputStream,signature);
                            }
                        }
                    }
                else
                    {
                    throw new ArgumentOutOfRangeException(nameof(Flags));
                    }
                }
            catch (Exception e)
                {
                e.Add("Flags",Flags);
                }
            }

        #region M:Write(Stream,UInt32)
        private static unsafe void Write(Stream stream, UInt32 r) {
            var buffer = new Byte[sizeof(UInt32)];
            fixed (Byte* i = buffer) {
                *(UInt32*)i = r;
                }
            stream.Write(buffer,0,buffer.Length);
            }
        #endregion
        #region M:Write(Stream,Int32)
        private static unsafe void Write(Stream stream, Int32 r) {
            var buffer = new Byte[sizeof(Int32)];
            fixed (Byte* i = buffer) {
                *(Int32*)i = r;
                }
            stream.Write(buffer,0,buffer.Length);
            }
        #endregion
        #region M:Write(Stream,Byte[])
        private static unsafe void Write(Stream stream, Byte[] value) {
            stream.Write(value,0,value.Length);
            }
        #endregion
        #region M:Write(Stream,Byte[],Int32)
        private static unsafe void Write(Stream stream, Byte[] value,Int32 size) {
            stream.Write(value,0,size);
            }
        #endregion
        #region M:Write(Stream,Encoding,String)
        private static unsafe void Write(Stream stream, Encoding encoding, String value) {
            var r = encoding.GetBytes(value);
            stream.WriteByte((Byte)r.Length);
            stream.Write(r,0,r.Length);
            }
        #endregion
        #region M:Write(Stream,String)
        private static unsafe void Write(Stream stream, String value) {
            Write(stream,Encoding.UTF8,value);
            }
        #endregion

        private const UInt32 FT_SIGNATURE_CONTAINER_HEADER_MAGIC  = 0x00035446;
        private const UInt32 FT_SIGNATURE_CONTAINER_BARRIER_MAGIC = 0xFF00EE10;
        }
    }
