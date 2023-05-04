using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    internal abstract class TransferInlineFactory<T> : TransferTextElementFactory<T>
        where T : Inline
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(T Source, T Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,Inline.BaselineAlignmentProperty);
                Transfer(Source,Target,Inline.FlowDirectionProperty);
                Transfer(Source,Target,Inline.TextDecorationsProperty);
                }
            }
        }
    }