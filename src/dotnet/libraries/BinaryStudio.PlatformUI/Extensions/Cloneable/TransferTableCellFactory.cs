using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableCell))]
    internal class TransferTableCellFactory : TransferTextElementFactory<TableCell>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(TableCell Source, TableCell Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,TableCell.BorderBrushProperty);
                CopyTo(Source,Target,TableCell.BorderThicknessProperty);
                CopyTo(Source,Target,TableCell.ColumnSpanProperty);
                CopyTo(Source,Target,TableCell.FlowDirectionProperty);
                CopyTo(Source,Target,TableCell.LineHeightProperty);
                CopyTo(Source,Target,TableCell.LineStackingStrategyProperty);
                CopyTo(Source,Target,TableCell.PaddingProperty);
                CopyTo(Source,Target,TableCell.RowSpanProperty);
                CopyTo(Source,Target,TableCell.TextAlignmentProperty);
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
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
                }
            }

        /// <summary>Transfers data context.</summary>
        /// <param name="Target">Target object.</param>
        /// <param name="DataContext">Data context.</param>
        protected override void TransferDataContext(TableCell Target, Object DataContext) {
            base.TransferDataContext(Target, DataContext);
            var Block = Target.Blocks.FirstBlock;
            while (Block != null) {
                GetFactory(Block).TransferDataContext(Block,DataContext);
                Block = Block.NextBlock;
                }
            }
        }
    }