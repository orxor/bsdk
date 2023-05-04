using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Span))]
    internal class TransferSpanFactory : TransferInlineFactory<Span>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(Span Source, Span Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
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
        }
    }