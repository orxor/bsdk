using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Controls;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Section))]
    internal class TransferSectionFactory : TransferBlockFactory<Section>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(Section Source, Section Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Target.HasTrailingParagraphBreakOnPaste = Source.HasTrailingParagraphBreakOnPaste;
                if (ContentControl.ContentProperty.IsOwnedBy(Source.GetType())) {
                    Transfer(Source,Target,ContentControl.ContentProperty);
                    Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                    Update(Target,ContentControl.ContentProperty);
                    }
                else
                    {
                    Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                    }
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
        protected override void TransferDataContext(Section Target, Object DataContext) {
            base.TransferDataContext(Target, DataContext);
            var e = BindingOperations.GetBindingExpressionBase(Target,ContentControl.ContentProperty);
            if (e != null) {
                e.UpdateTarget();
                }
            else
                {
                var Block = Target.Blocks.FirstBlock;
                while (Block != null) {
                    GetFactory(Block).TransferDataContext(Block,DataContext);
                    Block = Block.NextBlock;
                    }
                }
            }
        }
    }