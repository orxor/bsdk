using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(ListItem))]
    internal class TransferListItemFactory : TransferTextElementFactory<ListItem>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(ListItem Source, ListItem Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,ListItem.BorderBrushProperty);
                Transfer(Source,Target,ListItem.BorderThicknessProperty);
                Transfer(Source,Target,ListItem.FlowDirectionProperty);
                Transfer(Source,Target,ListItem.LineHeightProperty);
                Transfer(Source,Target,ListItem.LineStackingStrategyProperty);
                Transfer(Source,Target,ListItem.MarginProperty);
                Transfer(Source,Target,ListItem.PaddingProperty);
                Transfer(Source,Target,ListItem.TextAlignmentProperty);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
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
        }
    }