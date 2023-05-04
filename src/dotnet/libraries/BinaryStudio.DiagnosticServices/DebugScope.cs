using System;
using System.Diagnostics;

namespace BinaryStudio.DiagnosticServices
    {
    public class DebugScope : IDisposable
        {
        private readonly Int32 IndentLevel;
        private readonly Int32 IndentSize;
        public DebugScope()
            {
            IndentLevel = Debug.IndentLevel;
            IndentSize  = Debug.IndentSize;
            Debug.IndentLevel++;
            Debug.IndentSize = 2;
            }


        public void Dispose()
            {
            Debug.IndentLevel = IndentLevel;
            Debug.IndentSize  = IndentSize;
            }
        }
    }