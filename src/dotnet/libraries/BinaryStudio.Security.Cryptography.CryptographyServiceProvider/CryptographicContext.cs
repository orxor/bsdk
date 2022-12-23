using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider.Internal;

namespace BinaryStudio.Security.Cryptography
{
    //public class CryptographicContext : CryptographicObject
    //    {
    //    private const Int32 DefaultBufferSize = 1024;
    //    private const UInt32 CMSG_INDEFINITE_LENGTH = 0xffffffff;
    //    private CryptographicContext Context;

    //    public override IntPtr Handle => throw new NotImplementedException();

    //    #region M:VerifyAttachedMessage(Stream,Stream,{out}IList<X509Certificate>)
    //    public void VerifyAttachedMessage(Stream InputStream,Stream OutputStream,out IList<X509Certificate> Certificates) {
    //        if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
    //        if (!InputStream.CanSeek) { throw new ArgumentOutOfRangeException(nameof(InputStream)); }
    //        if (OutputStream == null) { throw new ArgumentNullException(nameof(OutputStream)); }
    //        Certificates = EmptyArray<X509Certificate>.Value;
    //        using (InputStream.PositionScope()) {
    //            if (ReadUInt32(InputStream) == 0x00035446) {
    //                var HashAlgorithmIdentifier = ReadPascalString(InputStream,Encoding.ASCII);
    //                var CertificateSize = ReadInt32(InputStream);
    //                if (CertificateSize != null) {
    //                    var CertificateBuffer = new Byte[CertificateSize.Value];
    //                    if (InputStream.Read(CertificateBuffer,0,CertificateBuffer.Length) == CertificateBuffer.Length) {
    //                        var certificate = new X509Certificate(CertificateBuffer);
    //                        if (ReadUInt32(InputStream) == 0xff00ee10) {
    //                            var B = InputStream.Position;
    //                            InputStream.Seek(-sizeof(Int32),SeekOrigin.End);
    //                            var SignatureSize = ReadInt32(InputStream);
    //                            var E = InputStream.Seek(-sizeof(Int32)-SignatureSize.Value,SeekOrigin.End);
    //                            InputStream.Position = B;
    //                            CopyTo(InputStream,OutputStream,(Int32)(E-B));
    //                            return;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    #endregion
    //    #region M:DecryptMessage(Stream)
    //    public unsafe void DecryptMessage(Stream InputStream, Stream OutputStream, out X509Certificate RecipientCertificate) {
    //        if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
    //        if (!InputStream.CanSeek) { throw new ArgumentOutOfRangeException(nameof(InputStream)); }
    //        RecipientCertificate = null;
    //        using (InputStream.PositionScope()) {
    //            switch (ReadUInt32(InputStream)) {
    //                case 0x00055446:
    //                case 0x01055446:
    //                    {
    //                    DecryptMessage(FetchMemoryBlocks(InputStream),out RecipientCertificate);
    //                    }
    //                    return;
    //                #region {verifies message and then decrypt content}
    //                case 0x00035446:
    //                    {
    //                    InputStream.Seek(-sizeof(UInt32),SeekOrigin.Current);
    //                    var TargetFileName = Path.GetTempFileName();
    //                    try
    //                        {
    //                        using (var EncryptedOutputStream = File.OpenWrite(TargetFileName)) { VerifyAttachedMessage(InputStream,EncryptedOutputStream,out var certificates); }
    //                        using (var EncryptedOutputStream = File.OpenRead(TargetFileName)) {
    //                            DecryptMessage(EncryptedOutputStream,OutputStream,out RecipientCertificate);
    //                            }
    //                        }
    //                    finally
    //                        {
    //                        File.Delete(TargetFileName);
    //                        }
    //                    }
    //                    return;
    //                #endregion
    //                }
    //            }
    //        var si = new CMSG_STREAM_INFO(CMSG_INDEFINITE_LENGTH,delegate(IntPtr arg, Byte* data, UInt32 size, Boolean final) {
    //            var bytes = new Byte[size];
    //            for (var i = 0; i < size; i++) {
    //                bytes[i] = data[i];
    //                }
    //            OutputStream.Write(bytes, 0, bytes.Length);
    //            return true;
    //            }, IntPtr.Zero);
    //        var msg = CryptMsgOpenToDecode(CRYPT_MSG_TYPE.PKCS_7_ASN_ENCODING,
    //            CRYPT_OPEN_MESSAGE_FLAGS.CMSG_NONE,
    //            CMSG_TYPE.CMSG_NONE, IntPtr.Zero, IntPtr.Zero,
    //            ref si);
    //        if (msg == IntPtr.Zero) { Validate(GetHRForLastWin32Error()); }
    //        var decrypting = false;
    //        using (var m = new CryptographicMessage(msg)) {
    //            var block = new Byte[DefaultBufferSize];
    //            for (;;) {
    //                Thread.Yield();
    //                var sz = InputStream.Read(block, 0, block.Length);
    //                if (sz == 0) { break; }
    //                m.Update(block, sz, false);
    //                if (!decrypting) {
    //                    var r = m.GetParameter(CMSG_PARAM.CMSG_ENVELOPE_ALGORITHM_PARAM, 0, out var hr);
    //                    if (hr == 0) {
    //                        fixed (Byte* p = r) { Debug.Print($"algid:{Marshal.PtrToStringAnsi(((CRYPT_ALGORITHM_IDENTIFIER*)p)->ObjectId)}"); }
    //                        var a = new List<Exception>();
    //                        for (var index = 0;;index++) {
    //                            r = m.GetParameter(CMSG_PARAM.CMSG_CMS_RECIPIENT_INFO_PARAM, index);
    //                            fixed (Byte* p = r) {
    //                                var KeyTrans = (IntPtr.Size == 4)
    //                                    ? (CMSG_KEY_TRANS_RECIPIENT_INFO)(*((CMSG_CMS_RECIPIENT_INFO32*)p)->KeyTrans)
    //                                    : (CMSG_KEY_TRANS_RECIPIENT_INFO)(*((CMSG_CMS_RECIPIENT_INFO64*)p)->KeyTrans);
    //                                var CertId = KeyTrans.RecipientId;
    //                                var IssuerSerialNumber = CertId.IssuerSerialNumber;
    //                                var SerialNumber = DecodeSerialNumberString(ref IssuerSerialNumber.SerialNumber);
    //                                var Issuer       = DecodeNameString(ref IssuerSerialNumber.Issuer);
    //                                //if (IntPtr.Size == 4) {
    //                                //    var keytrans = ((CMSG_CMS_RECIPIENT_INFO32*)p)->KeyTrans;
    //                                //    var certid = keytrans->RecipientId;
    //                                //    }
    //                                //else
    //                                //    {
    //                                //    var keytrans = ((CMSG_CMS_RECIPIENT_INFO64*)p)->KeyTrans;
    //                                //    var certid = keytrans->RecipientId;
    //                                //    serialnumber = DecodeSerialNumberString(ref certid.IssuerSerialNumber.SerialNumber);
    //                                //    issuer       = DecodeNameString(ref certid.IssuerSerialNumber.Issuer);
    //                                //    }
    //                                //certificate  = certificates.FirstOrDefault(i => i.SerialNumber == serialnumber);
    //                                //if (certificate == null)
    //                                //    {
    //                                //    a.Add(new Exception($"Certificate [{serialnumber}][{issuer}] not found in local system."));
    //                                //    continue;
    //                                //    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    #endregion
    //    #region M:DecryptMessage(IEnumerable<MemoryBlock>)
    //    private void DecryptMessage(IEnumerable<MemoryBlock> InputBlocks,out X509Certificate RecipientCertificate) {
    //        if (InputBlocks == null) { throw new ArgumentNullException(nameof(InputBlocks)); }
    //        RecipientCertificate = null;
    //        using (ReaderWriterLockSlim
    //            readL = new ReaderWriterLockSlim(),
    //            wrteL = new ReaderWriterLockSlim(),
    //            rsltL = new ReaderWriterLockSlim())
    //            {
    //            X509Certificate Certificate = null;
    //            var rsltT = new Thread(()=>{
    //                });
    //            var readT = Task.Factory.StartNew(()=>{
    //                var e = InputBlocks.GetEnumerator();
    //                if (e.MoveNext()) {
    //                    DecryptBlock(e.Current,out Certificate);
    //                    AsEnumerable(e).AsParallel().ForAll(block=>{
    //                        });
    //                    }
    //                });
    //            Task.WaitAll(readT);
    //            RecipientCertificate = Certificate;
    //            }
    //        }
    //    #endregion
    //    #region M:DecryptBlock(MemoryBlock,{out}X509Certificate)
    //    private void DecryptBlock(MemoryBlock InputBlock,out X509Certificate RecipientCertificate) {
    //        RecipientCertificate = null;
    //        using (MemoryStream
    //            InputStream  = new MemoryStream(InputBlock.Value),
    //            OutputStream = new MemoryStream())
    //            {
    //            InputStream.Seek(0,SeekOrigin.Begin);
    //            DecryptMessage(InputStream,OutputStream,out RecipientCertificate);
    //            }
    //        }
    //    #endregion

    //    #region M:ReadUInt32(Stream):UInt32?
    //    private static unsafe UInt32? ReadUInt32(Stream source) {
    //        if (source == null) { throw new ArgumentNullException(nameof(source)); }
    //        var buffer = new Byte[sizeof(UInt32)];
    //        var count = source.Read(buffer, 0, sizeof(UInt32));
    //        if (count == sizeof(UInt32)) {
    //            fixed (Byte* r = buffer) {
    //                return *(UInt32*)r;
    //                }
    //            }
    //        return null;
    //        }
    //    #endregion
    //    #region M:ReadInt32(Stream):Int32?
    //    private static unsafe Int32? ReadInt32(Stream source) {
    //        if (source == null) { throw new ArgumentNullException(nameof(source)); }
    //        var buffer = new Byte[sizeof(Int32)];
    //        var count = source.Read(buffer, 0, sizeof(Int32));
    //        if (count == sizeof(Int32)) {
    //            fixed (Byte* r = buffer) {
    //                return *(Int32*)r;
    //                }
    //            }
    //        return null;
    //        }
    //    #endregion
    //    #region M:ReadPascalString(Stream,Encoding):String
    //    private static String ReadPascalString(Stream source, Encoding encoding) {
    //        if (source == null) { throw new ArgumentNullException(nameof(source)); }
    //        if (encoding == null) { throw new ArgumentNullException(nameof(encoding)); }
    //        var c = source.ReadByte();
    //        var buffer = new Byte[c];
    //        var r = source.Read(buffer,0,c);
    //        if (r != c) { return null; }
    //        return encoding.GetString(buffer);
    //        }
    //    #endregion
    //    #region M:CopyTo(Stream,Strean,Int32)
    //    private static void CopyTo(Stream Source,Stream Target,Int32 Size) {
    //        var buffer = new Byte[DefaultBufferSize];
    //        while (Size > 0) {
    //            var c = Source.Read(buffer,0,Math.Min(Size,DefaultBufferSize));
    //            if (c > 0) {
    //                Target.Write(buffer,0,c);
    //                Size -= c;
    //                }
    //            }
    //        }
    //    #endregion

    //    private class MemoryBlock : Block<Byte[]>
    //        {
    //        public MemoryBlock(int index, byte[] value)
    //            : base(index, value)
    //            {
    //            }
    //        }

    //    #region M:FetchMemoryBlocks(Stream):IEnumerable<MemoryBlock>
    //    private static IEnumerable<MemoryBlock> FetchMemoryBlocks(Stream source) {
    //        if (source == null) { throw new ArgumentNullException(nameof(source)); }
    //        var i = 0;
    //        for (;;) {
    //            Thread.Yield();
    //            var size = ReadInt32(source);
    //            if (size == null) { break; }
    //            var block = new Byte[(Int32)size];
    //            source.Read(block,0,block.Length);
    //            yield return new MemoryBlock(i,block);
    //            i++;
    //            }
    //        }
    //    #endregion
    //    #region M:AsEnumerable<T>(IEnumerator<T>):IEnumerable<T>
    //    private static IEnumerable<T> AsEnumerable<T>(IEnumerator<T> e) {
    //        while (e.MoveNext()) {
    //            yield return e.Current;
    //            }
    //        }
    //    #endregion

    //    protected CryptographicContext(CRYPT_PROVIDER_TYPE ProviderType) {
    //        if (ProviderType == CRYPT_PROVIDER_TYPE.AUTO) {
    //            #if LINUX
    //            #else
    //            Context = new SCryptographicContext();
    //            #endif
    //            }
    //        }

    //    public CryptographicContext()
    //        :this(CRYPT_PROVIDER_TYPE.AUTO)
    //        {
    //        }

    //    #region M:CryptMsgOpenToDecode(CRYPT_MSG_TYPE,CRYPT_OPEN_MESSAGE_FLAGS,CMSG_TYPE,IntPtr,IntPtr,{ref}CMSG_STREAM_INFO):IntPtr
    //    protected internal virtual IntPtr CryptMsgOpenToDecode(CRYPT_MSG_TYPE dwMsgEncodingType, CRYPT_OPEN_MESSAGE_FLAGS flags, CMSG_TYPE type, IntPtr hCryptProv, IntPtr pRecipientInfo, ref CMSG_STREAM_INFO si) {
    //        return Context.CryptMsgOpenToDecode(dwMsgEncodingType,flags,type,hCryptProv,pRecipientInfo,ref si);
    //        }
    //    #endregion
    //    }
    }