using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(TableRow))]
    internal class CloneTableRowFactory : CloneTextElementFactory<TableRow>
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
                foreach (var SourceCell in SourceCells) {
                    var TargetCell = (TableCell)Activator.CreateInstance(SourceCell.GetType());
                    TargetCells.Add(TargetCell);
                    //ApplyStyle(TargetCell,Host);
                    GetFactory(SourceCell.GetType()).CopyTo(SourceCell,TargetCell);
                    }
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                CopyTriggers(Source,Target);
                }
            }
        }
    }