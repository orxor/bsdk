using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Controls;
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
        protected override void CopyTo(FlowDocument Source, FlowDocument Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,FlowDocument.BackgroundProperty);
                CopyTo(Source,Target,FlowDocument.ColumnGapProperty);
                CopyTo(Source,Target,FlowDocument.ColumnRuleBrushProperty);
                CopyTo(Source,Target,FlowDocument.ColumnRuleWidthProperty);
                CopyTo(Source,Target,FlowDocument.ColumnWidthProperty);
                CopyTo(Source,Target,FlowDocument.FlowDirectionProperty);
                CopyTo(Source,Target,FlowDocument.FontFamilyProperty);
                CopyTo(Source,Target,FlowDocument.FontSizeProperty);
                CopyTo(Source,Target,FlowDocument.FontStretchProperty);
                CopyTo(Source,Target,FlowDocument.FontStyleProperty);
                CopyTo(Source,Target,FlowDocument.FontWeightProperty);
                CopyTo(Source,Target,FlowDocument.ForegroundProperty);
                CopyTo(Source,Target,FlowDocument.IsColumnWidthFlexibleProperty);
                CopyTo(Source,Target,FlowDocument.IsHyphenationEnabledProperty);
                CopyTo(Source,Target,FlowDocument.IsOptimalParagraphEnabledProperty);
                CopyTo(Source,Target,FlowDocument.LineHeightProperty);
                CopyTo(Source,Target,FlowDocument.LineStackingStrategyProperty);
                CopyTo(Source,Target,FlowDocument.MaxPageHeightProperty);
                CopyTo(Source,Target,FlowDocument.MaxPageWidthProperty);
                CopyTo(Source,Target,FlowDocument.MinPageHeightProperty);
                CopyTo(Source,Target,FlowDocument.MinPageWidthProperty);
                CopyTo(Source,Target,FlowDocument.PageHeightProperty);
                CopyTo(Source,Target,FlowDocument.PagePaddingProperty);
                CopyTo(Source,Target,FlowDocument.PageWidthProperty);
                CopyTo(Source,Target,FlowDocument.TextAlignmentProperty);
                CopyTo(Source,Target,FlowDocument.TextEffectsProperty);
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                //if (Target.Parent is Control Control) {
                //    if (Target.IsDefaultValue(FlowDocument.FontFamilyProperty)) { CopyTo(Control,Target,Control.FontFamilyProperty); }
                //    }
                var SourceBlocks = Source.Blocks;
                var TargetBlocks = Target.Blocks;
                foreach (var SourceBlock in SourceBlocks) {
                    var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                    TargetBlocks.Add(TargetBlock);
                    GetFactory(SourceBlock.GetType()).CopyTo(SourceBlock,TargetBlock);
                    }
                ApplyStyle(Target,Target.Parent as FrameworkElement);
                }
            }
        }
    }