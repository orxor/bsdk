using System;
using System.Windows;
using System.Windows.Documents;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    internal abstract class CloneAnchoredBlockFactory<T> : CloneInlineFactory<T>
        where T : AnchoredBlock
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(T Source, T Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            CopyTo(Source,Target,AnchoredBlock.BorderBrushProperty);
            CopyTo(Source,Target,AnchoredBlock.BorderThicknessProperty);
            CopyTo(Source,Target,AnchoredBlock.LineHeightProperty);
            CopyTo(Source,Target,AnchoredBlock.LineStackingStrategyProperty);
            CopyTo(Source,Target,AnchoredBlock.MarginProperty);
            CopyTo(Source,Target,AnchoredBlock.PaddingProperty);
            CopyTo(Source,Target,AnchoredBlock.TextAlignmentProperty);
            var SourceBlocks = Source.Blocks;
            var TargetBlocks = Target.Blocks;
            foreach (var SourceBlock in SourceBlocks) {
                var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                TargetBlocks.Add(TargetBlock);
                //ApplyStyle(TargetBlock,Host);
                GetFactory(SourceBlock.GetType()).CopyTo(SourceBlock,TargetBlock);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        }
    }