using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BinaryStudio.IO;
using BinaryStudio.PlatformComponents;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider;

namespace BinaryStudio.Security.Cryptography
    {
    public class CryptographicContext : CryptographicObject
        {
        private const Int32 DefaultBufferSize = 1024;
        private const UInt32 FintechEncMagic0 = 0x00055446;
        private const UInt32 FintechEncMagic1 = 0x01055446;

        #region M:VerifyAttachedMessage(Stream,Stream,{out}IList<X509Certificate>)
        public void VerifyAttachedMessage(Stream InputStream,Stream OutputStream,out IList<X509Certificate> Certificates) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            if (!InputStream.CanSeek) { throw new ArgumentOutOfRangeException(nameof(InputStream)); }
            if (OutputStream == null) { throw new ArgumentNullException(nameof(OutputStream)); }
            Certificates = EmptyArray<X509Certificate>.Value;
            using (InputStream.PositionScope()) {
                if (ReadUInt32(InputStream) == 0x00035446) {
                    var HashAlgorithmIdentifier = ReadPascalString(InputStream,Encoding.ASCII);
                    var CertificateSize = ReadInt32(InputStream);
                    if (CertificateSize != null) {
                        var CertificateBuffer = new Byte[CertificateSize.Value];
                        if (InputStream.Read(CertificateBuffer,0,CertificateBuffer.Length) == CertificateBuffer.Length) {
                            var certificate = new X509Certificate(CertificateBuffer);
                            if (ReadUInt32(InputStream) == 0xff00ee10) {
                                var B = InputStream.Position;
                                InputStream.Seek(-sizeof(Int32),SeekOrigin.End);
                                var SignatureSize = ReadInt32(InputStream);
                                var E = InputStream.Seek(-sizeof(Int32)-SignatureSize.Value,SeekOrigin.End);
                                InputStream.Position = B;
                                CopyTo(InputStream,OutputStream,(Int32)(E-B));
                                return;
                                }
                            }
                        }
                    }
                }
            }
        #endregion
        #region M:DecryptMessage(Stream)
        public void DecryptMessage(Stream InputStream) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            if (!InputStream.CanSeek) { throw new ArgumentOutOfRangeException(nameof(InputStream)); }
            using (InputStream.PositionScope()) {
                switch (ReadUInt32(InputStream)) {
                    case 0x00055446:
                    case 0x01055446:
                        {
                        DecryptMessage(FetchMemoryBlocks(InputStream));
                        }
                        return;
                    #region {verifies message and then decrypt content}
                    case 0x00035446:
                        {
                        InputStream.Seek(-sizeof(UInt32),SeekOrigin.Current);
                        var TargetFileName = Path.GetTempFileName();
                        try
                            {
                            using (var OutputStream = File.OpenWrite(TargetFileName)) { VerifyAttachedMessage(InputStream,OutputStream,out var certificates); }
                            using (var OtherStream = File.OpenRead(TargetFileName)) {
                                DecryptMessage(OtherStream);
                                }
                            }
                        finally
                            {
                            File.Delete(TargetFileName);
                            }
                        }
                        return;
                    #endregion
                    }
                }
            }
        #endregion
        #region M:DecryptMessage(IEnumerable<MemoryBlock>)
        private void DecryptMessage(IEnumerable<MemoryBlock> InputBlocks) {
            if (InputBlocks == null) { throw new ArgumentNullException(nameof(InputBlocks)); }
            using (ReaderWriterLockSlim
                readL = new ReaderWriterLockSlim(),
                wrteL = new ReaderWriterLockSlim(),
                rsltL = new ReaderWriterLockSlim())
                {
                X509Certificate SigningCertificate = null;
                var rsltT = new Thread(()=>{
                    });
                var readT = Task.Factory.StartNew(()=>{
                    var e = InputBlocks.GetEnumerator();
                    if (e.MoveNext()) {
                        DecryptBlock(e.Current,out SigningCertificate);
                        AsEnumerable(e).AsParallel().ForAll(block=>{
                            });
                        }
                    });
                Task.WaitAll(readT);
                }
            }
        #endregion
        #region M:DecryptBlock(MemoryBlock,{out}X509Certificate)
        private void DecryptBlock(MemoryBlock InputBlock,out X509Certificate SigningCertificate) {
            SigningCertificate = null;
            }
        #endregion

        #region M:ReadUInt32(Stream):UInt32?
        private static unsafe UInt32? ReadUInt32(Stream source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var buffer = new Byte[sizeof(UInt32)];
            var count = source.Read(buffer, 0, sizeof(UInt32));
            if (count == sizeof(UInt32)) {
                fixed (Byte* r = buffer) {
                    return *(UInt32*)r;
                    }
                }
            return null;
            }
        #endregion
        #region M:ReadInt32(Stream):Int32?
        private static unsafe Int32? ReadInt32(Stream source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var buffer = new Byte[sizeof(Int32)];
            var count = source.Read(buffer, 0, sizeof(Int32));
            if (count == sizeof(Int32)) {
                fixed (Byte* r = buffer) {
                    return *(Int32*)r;
                    }
                }
            return null;
            }
        #endregion
        #region M:ReadPascalString(Stream,Encoding):String
        private static String ReadPascalString(Stream source, Encoding encoding) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (encoding == null) { throw new ArgumentNullException(nameof(encoding)); }
            var c = source.ReadByte();
            var buffer = new Byte[c];
            var r = source.Read(buffer,0,c);
            if (r != c) { return null; }
            return encoding.GetString(buffer);
            }
        #endregion
        #region M:CopyTo(Stream,Strean,Int32)
        private static void CopyTo(Stream Source,Stream Target,Int32 Size) {
            var buffer = new Byte[DefaultBufferSize];
            while (Size > 0) {
                var c = Source.Read(buffer,0,Math.Min(Size,DefaultBufferSize));
                if (c > 0) {
                    Target.Write(buffer,0,c);
                    Size -= c;
                    }
                }
            }
        #endregion

        private class MemoryBlock : Block<Byte[]>
            {
            public MemoryBlock(int index, byte[] value)
                : base(index, value)
                {
                }
            }

        #region M:FetchMemoryBlocks(Stream):IEnumerable<MemoryBlock>
        private static IEnumerable<MemoryBlock> FetchMemoryBlocks(Stream source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var i = 0;
            for (;;) {
                Thread.Yield();
                var size = ReadInt32(source);
                if (size == null) { break; }
                var block = new Byte[(Int32)size];
                source.Read(block,0,block.Length);
                yield return new MemoryBlock(i,block);
                i++;
                }
            }
        #endregion
        #region M:AsEnumerable<T>(IEnumerator<T>):IEnumerable<T>
        private static IEnumerable<T> AsEnumerable<T>(IEnumerator<T> e) {
            while (e.MoveNext()) {
                yield return e.Current;
                }
            }
        #endregion
        }
    }