using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableCell))]
    internal class TransferTableCellFactory : TransferTextElementFactory<TableCell>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(TableCell Source, TableCell Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,TableCell.BorderBrushProperty);
                Transfer(Source,Target,TableCell.BorderThicknessProperty);
                Transfer(Source,Target,TableCell.ColumnSpanProperty);
                Transfer(Source,Target,TableCell.FlowDirectionProperty);
                Transfer(Source,Target,TableCell.LineHeightProperty);
                Transfer(Source,Target,TableCell.LineStackingStrategyProperty);
                Transfer(Source,Target,TableCell.PaddingProperty);
                Transfer(Source,Target,TableCell.RowSpanProperty);
                Transfer(Source,Target,TableCell.TextAlignmentProperty);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                var SourceBlocks = Source.Blocks;
                var TargetBlocks = Target.Blocks;
                var SourceBlock = SourceBlocks.FirstBlock;
                while (SourceBlock != null) {
                    var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                    TargetBlocks.Add(TargetBlock);
                    ApplyStyle(TargetBlock,Target);
                    GetFactory(SourceBlock).Transfer(SourceBlock,TargetBlock);
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