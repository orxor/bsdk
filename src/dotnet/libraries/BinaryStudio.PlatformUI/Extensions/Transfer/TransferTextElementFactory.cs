using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    internal abstract class TransferTextElementFactory<T> : TransferFrameworkContentElementFactory<T>
        where T : TextElement
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(T Source, T Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,TextElement.BackgroundProperty);
                Transfer(Source,Target,TextElement.FontFamilyProperty);
                Transfer(Source,Target,TextElement.FontSizeProperty);
                Transfer(Source,Target,TextElement.FontStretchProperty);
                Transfer(Source,Target,TextElement.FontStyleProperty);
                Transfer(Source,Target,TextElement.FontWeightProperty);
                Transfer(Source,Target,TextElement.ForegroundProperty);
                Transfer(Source,Target,TextElement.TextEffectsProperty);
                }
            }
        }
    }