using System.Windows;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Documents;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    internal abstract class CloneContentElementFactory<T> : CloneFactory<T>
        where T : ContentElement
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(T Source, T Target) {
            if (Source == null) { return; }
            using (new DebugScope()) {
                CopyTo(Source,Target,ContentElement.IsEnabledProperty);
                CopyTo(Source,Target,ContentElement.FocusableProperty);
                CopyTo(Source,Target,TextProperties.IsSharedSizeScopeProperty);
                CopyTo(Source,Target,TextProperties.SharedGroupObjectProperty);
                CopyTo(Source,Target,TextProperties.WidthProperty);
                CopyTo(Source,Target,TextProperties.SharedSizeGroupProperty);
                CopyTo(Source,Target,TextProperties.DesiredSizeProperty);
                }
            }
        }
    }