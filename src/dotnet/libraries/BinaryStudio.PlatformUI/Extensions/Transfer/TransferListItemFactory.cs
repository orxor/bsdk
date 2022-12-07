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
                CopyTo(Source,Target,ListItem.BorderBrushProperty);
                CopyTo(Source,Target,ListItem.BorderThicknessProperty);
                CopyTo(Source,Target,ListItem.FlowDirectionProperty);
                CopyTo(Source,Target,ListItem.LineHeightProperty);
                CopyTo(Source,Target,ListItem.LineStackingStrategyProperty);
                CopyTo(Source,Target,ListItem.MarginProperty);
                CopyTo(Source,Target,ListItem.PaddingProperty);
                CopyTo(Source,Target,ListItem.TextAlignmentProperty);
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                var SourceBlocks = Source.Blocks;
                var TargetBlocks = Target.Blocks;
                foreach (var SourceBlock in SourceBlocks) {
                    var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                    TargetBlocks.Add(TargetBlock);
                    //ApplyStyle(TargetBlock,Host);
                    GetFactory(SourceBlock.GetType()).CopyTo(SourceBlock,TargetBlock);
                    }
                }
            }
        }
    }