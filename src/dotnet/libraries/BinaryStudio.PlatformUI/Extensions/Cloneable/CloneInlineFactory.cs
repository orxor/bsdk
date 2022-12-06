using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    internal abstract class CloneInlineFactory<T> : CloneTextElementFactory<T>
        where T : Inline
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(T Source, T Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,Inline.BaselineAlignmentProperty);
                CopyTo(Source,Target,Inline.FlowDirectionProperty);
                CopyTo(Source,Target,Inline.TextDecorationsProperty);
                }
            }
        }
    }