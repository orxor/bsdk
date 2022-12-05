using System;
using BinaryStudio.PlatformUI.Models;
using BinaryStudio.PortableExecutable.CodeView;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(S_REGISTER16))]
    internal class ModelS_REGISTER16 : ModelCodeViewSymbol<S_REGISTER16>
        {
        public Int32 Offset { get; }
        public DEBUG_SYMBOL_INDEX Type { get; }
        public String RegisterName { get; }

        public ModelS_REGISTER16(S_REGISTER16 source)
            : base(source)
            {
            Offset = source.Offset;
            Type = source.Type;
            RegisterName = source.DecodeRegister(source.RegisterIndex).ToString();
            }
        }
    }