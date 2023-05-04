using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableRowGroup))]
    internal class TransferTableRowGroupFactory : TransferTextElementFactory<TableRowGroup>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(TableRowGroup Source, TableRowGroup Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,ItemsControl.ItemsSourceProperty);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                var SourceRows = Source.Rows;
                var TargetRows = Target.Rows;
                foreach (var SourceRow in SourceRows) {
                    var TargetRow = (TableRow)Activator.CreateInstance(SourceRow.GetType());
                    TargetRows.Add(TargetRow);
                    ApplyStyle(TargetRow,Target);
                    GetFactory(SourceRow).Transfer(SourceRow,TargetRow);
                    }
                }
            }
        }
    }