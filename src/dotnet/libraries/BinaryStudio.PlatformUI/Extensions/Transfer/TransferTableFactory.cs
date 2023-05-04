using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Table))]
    internal class TransferTableFactory : TransferBlockFactory<Table>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(Table Source, Table Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,Table.CellSpacingProperty);
                var SourceColumns = Source.Columns;
                var TargetColumns = Target.Columns;
                foreach (var SourceColumn in SourceColumns) {
                    var TargetColumn = (TableColumn)Activator.CreateInstance(SourceColumn.GetType());
                    TargetColumns.Add(TargetColumn);
                    ApplyStyle(TargetColumn,Target);
                    GetFactory(SourceColumn).Transfer(SourceColumn,TargetColumn);
                    }
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                var SourceRowGroups = Source.RowGroups;
                var TargetRowGroups = Target.RowGroups;
                foreach (var SourceRowGroup in SourceRowGroups) {
                    var TargetRowGroup = (TableRowGroup)Activator.CreateInstance(SourceRowGroup.GetType());
                    TargetRowGroups.Add(TargetRowGroup);
                    ApplyStyle(TargetRowGroup,Target);
                    GetFactory(SourceRowGroup).Transfer(SourceRowGroup,TargetRowGroup);
                    }
                Transfer(Source,Target,TextProperties.IsAutoSizeProperty);
                }
            }
        }
    }