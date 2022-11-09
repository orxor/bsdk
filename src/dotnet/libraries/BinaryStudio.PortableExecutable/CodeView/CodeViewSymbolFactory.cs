using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable StaticMemberInGenericType

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public abstract class CodeViewSymbolFactory<T,I>
        where T: ICodeViewSymbolAttribute
        {
        private static readonly IDictionary<UInt16,Type> Types = new Dictionary<UInt16,Type>();
        static CodeViewSymbolFactory() {
            foreach (var type in typeof(CodeViewSymbol<T,I>).Assembly.GetTypes()) {
                var key = type.GetCustomAttributes(false).OfType<T>().FirstOrDefault();
                if (key != null) {
                    Types.Add(key.Key, type);
                    }
                }
            }

        protected abstract unsafe ICodeViewSymbol CreateDefault(CodeViewSymbolsSSection Section, Int32 Offset, I Index, Byte* Content, Int32 Length);

        public unsafe ICodeViewSymbol Create(CodeViewSymbolsSSection Section, Int32 Offset, I Index, Byte* Content, Int32 Length) {
            if (Types.TryGetValue((UInt16)(Object)Index,out var type)) {
                return (CodeViewSymbol<T,I>)Activator.CreateInstance(type,
                    Section,
                    Offset,
                    (IntPtr)Content,
                    Length);
                }
            return CreateDefault(Section,Offset,Index,Content,Length);
            }
        }
    }