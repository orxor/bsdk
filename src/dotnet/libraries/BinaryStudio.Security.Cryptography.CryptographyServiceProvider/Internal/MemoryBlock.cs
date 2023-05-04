using System;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    internal class MemoryBlock
        {
        public Int32 Order { get; }
        public Byte[] Block { get; }
        public MemoryBlock(Int32 order, Byte[] block, Int32 size) {
            Block = new Byte[size];
            Array.Copy(block,Block,size);
            Order = order;
            }

        public MemoryBlock(Int32 order, Byte[] block) {
            Block = block;
            Order = order;
            }

        public override String ToString()
            {
            return $"Order:{Order:D3},Size:{Block.Length}";
            }
        }
    }
