using System;
using System.Windows;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Span))]
    internal class CloneSpanFactory : CloneInlineFactory<Span>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(Span Source, Span Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            var SourceInlines = Source.Inlines;
            var TargetInlines = Target.Inlines;
            foreach (var SourceInline in SourceInlines) {
                var TargetInline = (Inline)Activator.CreateInstance(SourceInline.GetType());
                TargetInlines.Add(TargetInline);
                //ApplyStyle(TargetInline,Host);
                GetFactory(SourceInline.GetType()).CopyTo(SourceInline,TargetInline);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        }
    }