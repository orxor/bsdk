using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableRow))]
    internal class TransferTableRowFactory : TransferTextElementFactory<TableRow>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(TableRow Source, TableRow Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                var SourceCells = Source.Cells;
                var TargetCells = Target.Cells;
                CopyTriggers(Source,Target);
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                foreach (var SourceCell in SourceCells) {
                    var TargetCell = (TableCell)Activator.CreateInstance(SourceCell.GetType());
                    TargetCells.Add(TargetCell);
                    ApplyStyle(TargetCell,Target);
                    GetFactory(SourceCell.GetType()).CopyTo(SourceCell,TargetCell);
                    }
                }
            }

        /// <summary>Transfers data context.</summary>
        /// <param name="Target">Target object.</param>
        /// <param name="DataContext">Data context.</param>
        protected override void TransferDataContext(TableRow Target, Object DataContext) {
            base.TransferDataContext(Target, DataContext);
            foreach (var Cell in Target.Cells.ToArray()) {
                GetFactory(Cell).TransferDataContext(Cell,DataContext);
                }
            }
        }
    }