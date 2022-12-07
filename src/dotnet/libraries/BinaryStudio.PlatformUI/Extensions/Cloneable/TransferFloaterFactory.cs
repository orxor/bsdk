using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Floater))]
    internal class TransferFloaterFactory : TransferAnchoredBlockFactory<Floater>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(Floater Source, Floater Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,Floater.HorizontalAlignmentProperty);
                CopyTo(Source,Target,Floater.WidthProperty);
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                }
            }
        }
    }