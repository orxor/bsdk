using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(BlockUIContainer))]
    internal class TransferBlockUIContainerFactory : TransferBlockFactory<BlockUIContainer>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(BlockUIContainer Source, BlockUIContainer Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                Target.Child = Clone(Source.Child);
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                }
            }
        }
    }