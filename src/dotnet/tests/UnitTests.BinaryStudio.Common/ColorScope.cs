using System;

namespace UnitTests.BinaryStudio.Common
    {
    public class ColorScope : IDisposable
        {
        public static readonly Object SyncRoot = new Object();
        private readonly ConsoleColor color;
        public ColorScope(ConsoleColor color)
            {
            this.color = Console.ForegroundColor;
            Console.ForegroundColor = color;
            }

        void IDisposable.Dispose()
            {
            Console.ForegroundColor = color;
            }
        }
    }