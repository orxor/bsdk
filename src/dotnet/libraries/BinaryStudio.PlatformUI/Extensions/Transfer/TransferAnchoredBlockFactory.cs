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
                Transfer(Source,Target,AnchoredBlock.BorderBrushProperty);
                Transfer(Source,Target,AnchoredBlock.BorderThicknessProperty);
                Transfer(Source,Target,AnchoredBlock.LineHeightProperty);
                Transfer(Source,Target,AnchoredBlock.LineStackingStrategyProperty);
                Transfer(Source,Target,AnchoredBlock.MarginProperty);
                Transfer(Source,Target,AnchoredBlock.PaddingProperty);
                Transfer(Source,Target,AnchoredBlock.TextAlignmentProperty);
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
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                }
            }
        }
    }