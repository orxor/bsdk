using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(FlowDocument))]
    internal class TransferFlowDocumentFactory : TransferFrameworkContentElementFactory<FlowDocument>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(FlowDocument Source, FlowDocument Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,FlowDocument.BackgroundProperty);
                Transfer(Source,Target,FlowDocument.ColumnGapProperty);
                Transfer(Source,Target,FlowDocument.ColumnRuleBrushProperty);
                Transfer(Source,Target,FlowDocument.ColumnRuleWidthProperty);
                Transfer(Source,Target,FlowDocument.ColumnWidthProperty);
                Transfer(Source,Target,FlowDocument.FlowDirectionProperty);
                Transfer(Source,Target,FlowDocument.FontFamilyProperty);
                Transfer(Source,Target,FlowDocument.FontSizeProperty);
                Transfer(Source,Target,FlowDocument.FontStretchProperty);
                Transfer(Source,Target,FlowDocument.FontStyleProperty);
                Transfer(Source,Target,FlowDocument.FontWeightProperty);
                Transfer(Source,Target,FlowDocument.ForegroundProperty);
                Transfer(Source,Target,FlowDocument.IsColumnWidthFlexibleProperty);
                Transfer(Source,Target,FlowDocument.IsHyphenationEnabledProperty);
                Transfer(Source,Target,FlowDocument.IsOptimalParagraphEnabledProperty);
                Transfer(Source,Target,FlowDocument.LineHeightProperty);
                Transfer(Source,Target,FlowDocument.LineStackingStrategyProperty);
                Transfer(Source,Target,FlowDocument.MaxPageHeightProperty);
                Transfer(Source,Target,FlowDocument.MaxPageWidthProperty);
                Transfer(Source,Target,FlowDocument.MinPageHeightProperty);
                Transfer(Source,Target,FlowDocument.MinPageWidthProperty);
                Transfer(Source,Target,FlowDocument.PageHeightProperty);
                Transfer(Source,Target,FlowDocument.PagePaddingProperty);
                Transfer(Source,Target,FlowDocument.PageWidthProperty);
                Transfer(Source,Target,FlowDocument.TextAlignmentProperty);
                Transfer(Source,Target,FlowDocument.TextEffectsProperty);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                var SourceBlocks = Source.Blocks;
                var TargetBlocks = Target.Blocks;
                var SourceBlock = SourceBlocks.FirstBlock;
                while (SourceBlock != null) {
                    var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                    TargetBlocks.Add(TargetBlock);
                    GetFactory(SourceBlock).Transfer(SourceBlock,TargetBlock);
                    SourceBlock = SourceBlock.NextBlock;
                    }
                ApplyStyle(Target,Target.Parent as FrameworkElement);
                }
            }
        }
    }