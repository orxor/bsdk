using System;
using System.Windows;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    internal abstract class TransferFrameworkContentElementFactory<T> : TransferContentElementFactory<T>
        where T : FrameworkContentElement
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(T Source, T Target) {
            if (Source == null) { return; }
            Transfer(Source,Target,FrameworkContentElement.NameProperty);
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,FrameworkContentElement.StyleProperty);
                Transfer(Source,Target,FrameworkContentElement.ContextMenuProperty);
                Transfer(Source,Target,FrameworkContentElement.CursorProperty);
                Transfer(Source,Target,FrameworkContentElement.FocusVisualStyleProperty);
                Transfer(Source,Target,FrameworkContentElement.ForceCursorProperty);
                Transfer(Source,Target,FrameworkContentElement.InputScopeProperty);
                Transfer(Source,Target,FrameworkContentElement.LanguageProperty);
                Transfer(Source,Target,FrameworkContentElement.OverridesDefaultStyleProperty);
                Transfer(Source,Target,FrameworkContentElement.TagProperty);
                Transfer(Source,Target,FrameworkContentElement.ToolTipProperty);
                Target.Resources = Source.Resources;
                }
            }

        /// <summary>Transfers data context.</summary>
        /// <param name="Target">Target object.</param>
        /// <param name="DataContext">Data context.</param>
        protected override void TransferDataContext(T Target, Object DataContext)
            {
            Target.DataContext = DataContext;
            }
        }
    }