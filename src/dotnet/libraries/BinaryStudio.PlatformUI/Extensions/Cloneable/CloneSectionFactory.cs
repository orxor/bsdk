using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Section))]
    internal class CloneSectionFactory : CloneBlockFactory<Section>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(Section Source, Section Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                Target.HasTrailingParagraphBreakOnPaste = Source.HasTrailingParagraphBreakOnPaste;
                var SourceBlocks = Source.Blocks;
                var TargetBlocks = Target.Blocks;
                foreach (var SourceBlock in SourceBlocks) {
                    var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                    TargetBlocks.Add(TargetBlock);
                    //ApplyStyle(TargetBlock,Host);
                    GetFactory(SourceBlock.GetType()).CopyTo(SourceBlock,TargetBlock);
                    }
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                CopyTo(Source,Target,ContentControl.ContentProperty);
                }
            }
        }
    }