using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformComponents;
using BinaryStudio.PlatformComponents.Exceptions;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.CryptographyServiceProvider;
using BinaryStudio.Security.Cryptography.Internal;

namespace BinaryStudio.Security.Cryptography
    {
    using CERT_BLOB = CRYPT_BLOB;
    public abstract partial class CryptographicContext
        {
        private static readonly Int32 EncryptTaskCount;
        #region M:EncryptMessage(Stream,Stream,IList<X509Certificate>,Oid,CryptographicMessageFlags)
        public unsafe void EncryptMessage(Stream InputStream,Stream OutputStream,IList<X509Certificate> Recipients,Oid AlgId,CryptographicMessageFlags Flags) {
            if (AlgId == null) { throw new ArgumentNullException(nameof(AlgId)); }
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            if (Recipients == null) { throw new ArgumentNullException(nameof(Recipients)); }
            if (IsNullOrEmpty(Recipients)) { throw new ArgumentOutOfRangeException(nameof(Recipients)); }
            if (Recipients.Any(i => i == null)) { throw new ArgumentOutOfRangeException(nameof(Recipients)); }
            if (Flags.HasFlag(CryptographicMessageFlags.Attached)) { throw new ArgumentOutOfRangeException(nameof(Flags)); }
            if (Flags.HasFlag(CryptographicMessageFlags.Detached)) { throw new ArgumentOutOfRangeException(nameof(Flags)); }
            EnsureEntries(out var entries);
            try
                {
                //Recipients.ForAll(i => {
                //    VerifyObject(i,CertificateChainPolicy.CERT_CHAIN_POLICY_BASE);
                //    });
                using (var manager = new LocalMemoryManager()) {
                    var EncodeInfo = new CMSG_ENVELOPED_ENCODE_INFO {
                        Size = sizeof(CMSG_ENVELOPED_ENCODE_INFO),
                        ContentEncryptionAlgorithm = new CRYPT_ALGORITHM_IDENTIFIER {
                            ObjectId = (IntPtr)manager.StringToMem(AlgId.Value,Encoding.ASCII)
                            },
                        RecipientsCount = Recipients.Count,
                        Recipients = (CERT_INFO**)manager.Alloc(Recipients.Count*IntPtr.Size)
                        };
                    for (var i = 0; i < EncodeInfo.RecipientsCount; i++) {
                        EncodeInfo.Recipients[i] = ((CERT_CONTEXT*)Recipients[i].Handle)->CertInfo;
                        }
                    #if CMSG_ENVELOPED_ENCODE_INFO_HAS_CMS_FIELDS
                    if (Flags.HasFlag(CryptographicMessageFlags.IncludeSigningCertificate)) {
                        EncodeInfo.cCertEncoded = Recipients.Count;
                        EncodeInfo.rgCertEncoded = (CERT_BLOB*)manager.Alloc(sizeof(CERT_BLOB)*Recipients.Count);
                        for (var i = 0; i < EncodeInfo.RecipientsCount; i++) {
                            var certinfo = ((CERT_CONTEXT*)Recipients[i].Handle);
                            EncodeInfo.rgCertEncoded[i].Size = certinfo->CertEncodedSize;
                            EncodeInfo.rgCertEncoded[i].Data = certinfo->CertEncoded;
                            }
                        }
                    #endif
                    EncryptMessage(InputStream,OutputStream, ref EncodeInfo,Flags);
                    }
                }
            catch (Exception e)
                {
                e.Add("Flags",Flags);
                e.Add("Recipients",Recipients);
                e.Add("AlgId",AlgId);
                throw;
                }
            }
        #endregion
        #region M:EncryptMessage(Stream,Stream,{ref}CMSG_ENVELOPED_ENCODE_INFO,CryptographicMessageFlags)
        private unsafe void EncryptMessage(Stream InputStream,Stream OutputStream,ref CMSG_ENVELOPED_ENCODE_INFO EncodeInfo,CryptographicMessageFlags Flags) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            try
                {
                if (Flags.HasFlag(CryptographicMessageFlags.Split)) {
                    EncryptMessage(Split(InputStream),OutputStream, ref EncodeInfo, Flags,EncryptTaskCount);
                    }
                else
                    {
                    using (var message = CryptographicMessage.OpenToEncode(
                        (Bytes,Final)=> {
                            OutputStream?.Write(Bytes,0,Bytes.Length);
                            },
                        Flags.HasFlag(CryptographicMessageFlags.IndefiniteLength) ? CMSG_INDEFINITE_LENGTH : (UInt32)InputStream.Length,
                        0,CMSG_TYPE.CMSG_ENVELOPED,ref EncodeInfo))
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
            catch (Exception e)
                {
                e.Add("Flags",Flags);
                throw;
                }
            }
        #endregion
        #region M:EncryptMessage(IEnumerable<MemoryBlock>,Stream,{ref}CMSG_ENVELOPED_ENCODE_INFO,CryptographicMessageFlags,Int32)
        private void EncryptMessage(IEnumerable<MemoryBlock> InputSequence,Stream OutputStream,ref CMSG_ENVELOPED_ENCODE_INFO EncodeInfo,CryptographicMessageFlags Flags, Int32 TaskCount) {
            if (InputSequence == null) { throw new ArgumentNullException(nameof(InputSequence)); }
            if (OutputStream  == null) { throw new ArgumentNullException(nameof(OutputStream)); }
            try
                {
                using (ReaderWriterLockSlim
                    rdL = new ReaderWriterLockSlim(),
                    wrL = new ReaderWriterLockSlim())
                    {
                    var LocalEncodeInfo = EncodeInfo;
                    Write(OutputStream,FT_MAGIC_1);
                    var InputSequenceEmpty = 1;
                    var EncryptInputSequence  = new Queue<MemoryBlock>();
                    var EncryptOutputSequence = new LinkedList<MemoryBlock>();
                    #region broker:encryption{TaskCount}
                    var tasks = new List<Task>(Enumerable.Range(0,TaskCount).Select(i=>Task.Factory.StartNew(()=>{
                        while (true) {
                            Yield();
                            MemoryBlock SourceBlock = null;
                            #region dequeue input block 
                            using (UpgradeableReadLock(rdL)) {
                                if (EncryptInputSequence.Count > 0) {
                                    using (WriteLock(rdL)) {
                                        SourceBlock = EncryptInputSequence.Dequeue();
                                        }
                                    }
                                }
                            #endregion
                            #region encrypt and enqueue output block
                            if (SourceBlock != null) {
                                using (var TargetBlock = new MemoryStream()) {
                                    if (SourceBlock.Order == 0)
                                        {
                                        EncryptBlock(SourceBlock.Block, TargetBlock, ref LocalEncodeInfo,Flags);
                                        }
                                    else
                                        {
                                        EncryptBlock(SourceBlock.Block, TargetBlock, ref LocalEncodeInfo,Flags & ~CryptographicMessageFlags.IncludeSigningCertificate);
                                        }
                                    using (WriteLock(wrL)) {
                                        EncryptOutputSequence.AddLast(new MemoryBlock(SourceBlock.Order,TargetBlock.ToArray()));
                                        continue;
                                        }
                                    }
                                }
                            if (Interlocked.CompareExchange(ref InputSequenceEmpty,0,0) == 0) {
                                break;
                                }
                            #endregion
                            }
                        Interlocked.Decrement(ref TaskCount);
                        })));
                    #endregion
                    #region broker:output
                    var CurrentBlockIndex = 0;
                    tasks.Add(Task.Factory.StartNew(()=>{
                        while (true) {
                            Yield();
                            MemoryBlock SourceBlock = null;
                            #region dequeue input block 
                            using (UpgradeableReadLock(wrL)) {
                                if (EncryptOutputSequence.Count > 0) {
                                    for (var i = EncryptOutputSequence.First; i != null; i = i.Next) {
                                        if (i.Value.Order == CurrentBlockIndex) {
                                            using (WriteLock(wrL)) {
                                                SourceBlock = i.Value;
                                                EncryptOutputSequence.Remove(i);
                                                CurrentBlockIndex++;
                                                break;
                                                }
                                            }
                                        }
                                    }
                                else if ((Interlocked.CompareExchange(ref InputSequenceEmpty,0,0) == 0) &&
                                         (Interlocked.CompareExchange(ref TaskCount,0,0) == 0))
                                    {
                                    break;
                                    }
                                }
                            #endregion
                            #region write desired block to output stream
                            if (SourceBlock != null) {
                                lock(OutputStream) {
                                    Write(OutputStream,SourceBlock.Block.Length);
                                    Write(OutputStream,SourceBlock.Block);
                                    }
                                }
                            #endregion
                            }
                        }));
                    #endregion
                    var BlockIndex = 0;
                    foreach (var InputBlock in InputSequence) {
                        using (WriteLock(rdL)) {
                            EncryptInputSequence.Enqueue(InputBlock);
                            }
                        BlockIndex++;
                        }
                    Interlocked.Decrement(ref InputSequenceEmpty);
                    Task.WaitAll(tasks.ToArray());
                    }
                }
            catch (AggregateException e) {
                if (e.InnerExceptions.Count == 1) {
                    #if NET35
                    throw HResultException.GetExceptionForHR(
                        (HRESULT)Marshal.GetHRForException(e.InnerExceptions[0]),
                        e.InnerExceptions[0]).
                        Add("Flags",Flags);
                    #else
                    #endif
                    }
                e.Add("Flags",Flags);
                throw;
                }
            catch (Exception e)
                {
                e.Add("Flags",Flags);
                throw;
                }
            }
        #endregion
        #region M:EncryptBlock(Byte[],Stream,{ref}CMSG_ENVELOPED_ENCODE_INFO,Flags)
        private void EncryptBlock(Byte[] InputBlock,Stream OutputStream,ref CMSG_ENVELOPED_ENCODE_INFO EncodeInfo,CryptographicMessageFlags Flags) {
            Flags &= ~CryptographicMessageFlags.Split;
            Flags |= CryptographicMessageFlags.SkipCertificateValidation;
            using (var InputStream = new MemoryStream(InputBlock)) {
                EncryptMessage(InputStream, OutputStream, ref EncodeInfo, Flags);
                }
            }
        #endregion
        #region M:Split(Stream):IEnumerable<MemoryBlock>
        private IEnumerable<MemoryBlock> Split(Stream InputStream) {
            if (InputStream == null) { throw new ArgumentNullException(nameof(InputStream)); }
            var block = new Byte[ENCRYPT_BUFFER_SIZE];
            var offset = sizeof(UInt32);
            var size = InputStream.Read(block, 0, offset);
            var order = 0;
            if ((size == 4) && 
                ((block[0] == 0x46)) && 
                ((block[1] == 0x54)) && 
                ((block[2] == 0x05)) &&
                ((block[3] == 0x00) || (block[3] == 0x01)))
                {
                /* Proprietary FINTECH format */
                for (;;) {
                    ReadInt32(InputStream,out size);
                    if (size == 0) { break; }
                    Validate(ReadBlock(InputStream, size, out block));
                    yield return new MemoryBlock(order,block);
                    order++;
                    }
                }
            else
                {
                if (size == 0) { throw HResultException.GetExceptionForHR(HRESULT.CRYPT_E_INVALID_MSG_TYPE); }
                offset = size;
                for (;;) {
                    size = InputStream.Read(block, offset, block.Length - offset);
                    if (size == 0) {
                        if (offset > 0) {
                            yield return new MemoryBlock(order,block, offset);
                            }
                        break;
                        }
                    yield return new MemoryBlock(order,block, size);
                    offset = 0;
                    order++;
                    }
                }
            }
        #endregion

        private const UInt32 FT_MAGIC_0 = 0x00055446;
        private const UInt32 FT_MAGIC_1 = 0x01055446;
        private const Int32 ENCRYPT_BUFFER_SIZE = 5*1024*1024;
        }
    }
