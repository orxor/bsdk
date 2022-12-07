using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    internal abstract class TransferTextElementFactory<T> : TransferFrameworkContentElementFactory<T>
        where T : TextElement
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(T Source, T Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,TextElement.BackgroundProperty);
                CopyTo(Source,Target,TextElement.FontFamilyProperty);
                CopyTo(Source,Target,TextElement.FontSizeProperty);
                CopyTo(Source,Target,TextElement.FontStretchProperty);
                CopyTo(Source,Target,TextElement.FontStyleProperty);
                CopyTo(Source,Target,TextElement.FontWeightProperty);
                CopyTo(Source,Target,TextElement.ForegroundProperty);
                CopyTo(Source,Target,TextElement.TextEffectsProperty);
                }
            }
        }
    }