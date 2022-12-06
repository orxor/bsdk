using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableColumn))]
    internal class CloneTableColumnFactory : CloneFrameworkContentElementFactory<TableColumn>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(TableColumn Source, TableColumn Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,TableColumn.BackgroundProperty);
                CopyTo(Source,Target,TableColumn.WidthProperty);
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                CopyTo(Source,Target,TextProperties.IsAutoSizeProperty);
                }
            }
        }
    }