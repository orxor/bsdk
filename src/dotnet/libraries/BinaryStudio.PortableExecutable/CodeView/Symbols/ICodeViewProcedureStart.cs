using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public interface ICodeViewProcedureStart : ICodeViewBlockStart
        {
        Int16 SegmentIndex { get; }
        Int32 ProcedureOffset { get; }
        Int32 ProcedureLength { get; }
        }
    }