using System.Windows.Documents;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    internal abstract class CloneBlockFactory<T> : CloneTextElementFactory<T>
        where T : Block
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(T Source, T Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            CopyTo(Source,Target,Block.BorderBrushProperty);
            CopyTo(Source,Target,Block.BorderThicknessProperty);
            CopyTo(Source,Target,Block.BreakColumnBeforeProperty);
            CopyTo(Source,Target,Block.BreakPageBeforeProperty);
            CopyTo(Source,Target,Block.ClearFloatersProperty);
            CopyTo(Source,Target,Block.FlowDirectionProperty);
            CopyTo(Source,Target,Block.IsHyphenationEnabledProperty);
            CopyTo(Source,Target,Block.LineHeightProperty);
            CopyTo(Source,Target,Block.LineStackingStrategyProperty);
            CopyTo(Source,Target,Block.MarginProperty);
            CopyTo(Source,Target,Block.PaddingProperty);
            CopyTo(Source,Target,Block.TextAlignmentProperty);
            }
        }
    }