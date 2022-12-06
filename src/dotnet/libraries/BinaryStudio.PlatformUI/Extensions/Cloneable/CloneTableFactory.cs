﻿using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Table))]
    internal class CloneTableFactory : CloneBlockFactory<Table>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(Table Source, Table Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,Table.CellSpacingProperty);
                var SourceColumns = Source.Columns;
                var TargetColumns = Target.Columns;
                foreach (var SourceColumn in SourceColumns) {
                    var TargetColumn = (TableColumn)Activator.CreateInstance(SourceColumn.GetType());
                    TargetColumns.Add(TargetColumn);
                    //ApplyStyle(TargetColumn,Host);
                    GetFactory(SourceColumn.GetType()).CopyTo(SourceColumn,TargetColumn);
                    }
                var SourceRowGroups = Source.RowGroups;
                var TargetRowGroups = Target.RowGroups;
                foreach (var SourceRowGroup in SourceRowGroups) {
                    var TargetRowGroup = (TableRowGroup)Activator.CreateInstance(SourceRowGroup.GetType());
                    TargetRowGroups.Add(TargetRowGroup);
                    //ApplyStyle(TargetRowGroup,Host);
                    GetFactory(SourceRowGroup.GetType()).CopyTo(SourceRowGroup,TargetRowGroup);
                    }
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                CopyTo(Source,Target,TextProperties.IsAutoSizeProperty);
                }
            }
        }
    }