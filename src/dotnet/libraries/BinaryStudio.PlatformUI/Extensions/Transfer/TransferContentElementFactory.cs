using System.Windows;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Documents;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    internal abstract class TransferContentElementFactory<T> : TransferFactory<T>
        where T : ContentElement
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(T Source, T Target) {
            if (Source == null) { return; }
            using (new DebugScope()) {
                Transfer(Source,Target,ContentElement.IsEnabledProperty);
                Transfer(Source,Target,ContentElement.FocusableProperty);
                Transfer(Source,Target,TextProperties.IsSharedSizeScopeProperty);
                Transfer(Source,Target,TextProperties.SharedGroupObjectProperty);
                Transfer(Source,Target,TextProperties.WidthProperty);
                Transfer(Source,Target,TextProperties.SharedSizeGroupProperty);
                Transfer(Source,Target,TextProperties.DesiredSizeProperty);
                }
            }
        }
    }