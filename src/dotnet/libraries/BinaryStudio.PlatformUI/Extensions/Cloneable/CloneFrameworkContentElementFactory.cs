using System.Windows;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    internal abstract class CloneFrameworkContentElementFactory<T> : CloneContentElementFactory<T>
        where T : FrameworkContentElement
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(T Source, T Target) {
            if (Source == null) { return; }
            CopyTo(Source,Target,FrameworkContentElement.NameProperty);
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,FrameworkContentElement.StyleProperty);
                CopyTo(Source,Target,FrameworkContentElement.ContextMenuProperty);
                CopyTo(Source,Target,FrameworkContentElement.CursorProperty);
                CopyTo(Source,Target,FrameworkContentElement.FocusVisualStyleProperty);
                CopyTo(Source,Target,FrameworkContentElement.ForceCursorProperty);
                CopyTo(Source,Target,FrameworkContentElement.InputScopeProperty);
                CopyTo(Source,Target,FrameworkContentElement.LanguageProperty);
                CopyTo(Source,Target,FrameworkContentElement.OverridesDefaultStyleProperty);
                CopyTo(Source,Target,FrameworkContentElement.TagProperty);
                CopyTo(Source,Target,FrameworkContentElement.ToolTipProperty);
                Target.Resources = Source.Resources;
                }
            }
        }
    }