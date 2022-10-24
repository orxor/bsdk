using System;

namespace BinaryStudio.DiagnosticServices
    {
    public sealed class ColorScope : IDisposable
        {
        private readonly ConsoleColor color;
        public ColorScope()
            {
            color = Console.ForegroundColor;
            }

        public ColorScope(ConsoleColor color)
            {
            this.color = Console.ForegroundColor;
            Console.ForegroundColor = color;
            }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
            {
            Console.ForegroundColor = color;
            }
        }
    }