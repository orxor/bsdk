using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    internal abstract class TransferAnchoredBlockFactory<T> : TransferInlineFactory<T>
        where T : AnchoredBlock
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(T Source, T Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,AnchoredBlock.BorderBrushProperty);
                CopyTo(Source,Target,AnchoredBlock.BorderThicknessProperty);
                CopyTo(Source,Target,AnchoredBlock.LineHeightProperty);
                CopyTo(Source,Target,AnchoredBlock.LineStackingStrategyProperty);
                CopyTo(Source,Target,AnchoredBlock.MarginProperty);
                CopyTo(Source,Target,AnchoredBlock.PaddingProperty);
                CopyTo(Source,Target,AnchoredBlock.TextAlignmentProperty);
                var SourceBlocks = Source.Blocks;
                var TargetBlocks = Target.Blocks;
                var SourceBlock = SourceBlocks.FirstBlock;
                while (SourceBlock != null) {
                    var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                    TargetBlocks.Add(TargetBlock);
                    ApplyStyle(TargetBlock,Target);
                    GetFactory(SourceBlock).CopyTo(SourceBlock,TargetBlock);
                    SourceBlock = SourceBlock.NextBlock;
                    }
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                }
            }
        }
    }