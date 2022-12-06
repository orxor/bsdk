using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableCell))]
    internal class CloneTableCellFactory : CloneTextElementFactory<TableCell>
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
    }