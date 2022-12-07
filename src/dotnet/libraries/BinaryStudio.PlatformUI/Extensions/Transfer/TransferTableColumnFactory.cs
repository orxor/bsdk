using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableColumn))]
    internal class TransferTableColumnFactory : TransferFrameworkContentElementFactory<TableColumn>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(TableColumn Source, TableColumn Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,TableColumn.BackgroundProperty);
                Transfer(Source,Target,TableColumn.WidthProperty);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                Transfer(Source,Target,TextProperties.IsAutoSizeProperty);
                }
            }
        }
    }