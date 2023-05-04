using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Floater))]
    internal class TransferFloaterFactory : TransferAnchoredBlockFactory<Floater>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(Floater Source, Floater Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,Floater.HorizontalAlignmentProperty);
                Transfer(Source,Target,Floater.WidthProperty);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                }
            }
        }
    }