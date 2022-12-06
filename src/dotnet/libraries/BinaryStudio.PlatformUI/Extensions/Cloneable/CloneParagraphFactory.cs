using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Paragraph))]
    internal class CloneParagraphFactory : CloneBlockFactory<Paragraph>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(Paragraph Source, Paragraph Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            CopyTo(Source,Target,Paragraph.KeepTogetherProperty);
            CopyTo(Source,Target,Paragraph.KeepWithNextProperty);
            CopyTo(Source,Target,Paragraph.MinOrphanLinesProperty);
            CopyTo(Source,Target,Paragraph.MinWidowLinesProperty);
            CopyTo(Source,Target,Paragraph.TextDecorationsProperty);
            CopyTo(Source,Target,Paragraph.TextIndentProperty);
            var SourceInlines = Source.Inlines;
            var TargetInlines = Target.Inlines;
            foreach (var SourceInline in SourceInlines) {
                var TargetInline = (Inline)Activator.CreateInstance(SourceInline.GetType());
                TargetInlines.Add(TargetInline);
                //ApplyStyle(TargetInline,Host);
                GetFactory(SourceInline.GetType()).CopyTo(SourceInline,TargetInline);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            CopyTo(Source,Target,ContentControl.ContentProperty);
            }
        }
    }