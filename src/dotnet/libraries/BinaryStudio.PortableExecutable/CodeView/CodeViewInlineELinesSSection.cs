using BinaryStudio.PortableExecutable.Win32;
using System;

namespace BinaryStudio.PortableExecutable.CodeView
    {
    public class CodeViewInlineELinesSSection : CodeViewPrimarySSection
        {
        public override DEBUG_S Type { get { return DEBUG_S.DEBUG_S_INLINEELINES; }}
        public CV_INLINEE_SOURCE_LINE Signature { get; }
        internal unsafe CodeViewInlineELinesSSection(CodeViewSection section, Int32 offset, Byte* content, Int32 length)
            : base(section, offset, content, length)
            {
            Console.Error.WriteLine($"{Type}");
            var r = content;
            Signature = (CV_INLINEE_SOURCE_LINE)ReadUInt32(ref r);
            }
        }
    }