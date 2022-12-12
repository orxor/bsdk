using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    internal abstract class TransferBlockFactory<T> : TransferTextElementFactory<T>
        where T : Block
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(T Source, T Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,Block.BorderBrushProperty);
                Transfer(Source,Target,Block.BorderThicknessProperty);
                Transfer(Source,Target,Block.BreakColumnBeforeProperty);
                Transfer(Source,Target,Block.BreakPageBeforeProperty);
                Transfer(Source,Target,Block.ClearFloatersProperty);
                Transfer(Source,Target,Block.FlowDirectionProperty);
                Transfer(Source,Target,Block.IsHyphenationEnabledProperty);
                Transfer(Source,Target,Block.LineHeightProperty);
                Transfer(Source,Target,Block.LineStackingStrategyProperty);
                Transfer(Source,Target,Block.MarginProperty);
                Transfer(Source,Target,Block.PaddingProperty);
                Transfer(Source,Target,Block.TextAlignmentProperty);
                }
            }
        }
    }