using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableRowGroup))]
    internal class CloneTableRowGroupFactory : CloneTextElementFactory<TableRowGroup>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(TableRowGroup Source, TableRowGroup Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                var SourceRows = Source.Rows;
                var TargetRows = Target.Rows;
                foreach (var SourceRow in SourceRows) {
                    var TargetRow = (TableRow)Activator.CreateInstance(SourceRow.GetType());
                    TargetRows.Add(TargetRow);
                    //ApplyStyle(TargetRow,Host);
                    GetFactory(SourceRow.GetType()).CopyTo(SourceRow,TargetRow);
                    }
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                }
            }
        }
    }