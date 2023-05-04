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
    [CloneFactory(typeof(Paragraph))]
    internal class TransferParagraphFactory : TransferBlockFactory<Paragraph>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(Paragraph Source, Paragraph Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,Paragraph.KeepTogetherProperty);
                Transfer(Source,Target,Paragraph.KeepWithNextProperty);
                Transfer(Source,Target,Paragraph.MinOrphanLinesProperty);
                Transfer(Source,Target,Paragraph.MinWidowLinesProperty);
                Transfer(Source,Target,Paragraph.TextDecorationsProperty);
                Transfer(Source,Target,Paragraph.TextIndentProperty);
                if (ContentControl.ContentProperty.IsOwnedBy(Source.GetType())) {
                    Transfer(Source,Target,ContentControl.ContentProperty);
                    Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                    Update(Target,ContentControl.ContentProperty);
                    }
                else
                    {
                    Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                    }
                var SourceInlines = Source.Inlines;
                var TargetInlines = Target.Inlines;
                var SourceInline = SourceInlines.FirstInline;
                while (SourceInline != null) {
                    var TargetInline = (Inline)Activator.CreateInstance(SourceInline.GetType());
                    TargetInlines.Add(TargetInline);
                    ApplyStyle(TargetInline,Target);
                    GetFactory(SourceInline).Transfer(SourceInline,TargetInline);
                    SourceInline = SourceInline.NextInline;
                    }
                }
            }

        /// <summary>Transfers data context.</summary>
        /// <param name="Target">Target object.</param>
        /// <param name="DataContext">Data context.</param>
        protected override void TransferDataContext(Paragraph Target, Object DataContext) {
            base.TransferDataContext(Target, DataContext);
            var Inline = Target.Inlines.FirstInline;
            while (Inline != null) {
                GetFactory(Inline).TransferDataContext(Inline,DataContext);
                Inline = Inline.NextInline;
                }
            }
        }
    }