using System;

namespace UnitTests.BinaryStudio.Common
    {
    public class ColorScope : IDisposable
        {
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