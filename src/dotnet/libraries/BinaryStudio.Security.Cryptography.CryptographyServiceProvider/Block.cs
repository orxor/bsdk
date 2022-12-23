using System;

namespace BinaryStudio.Security.Cryptography.CryptographyServiceProvider
    {
    internal class Block<T>
        {
        public Int32 Index { get; }
        public T Value { get; }
        public Block(Int32 index, T value) {
            Index = index;
            Value = value;
            }
        }
    }
