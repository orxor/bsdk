using System;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(List))]
    internal class TransferListFactory : TransferBlockFactory<List>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(List Source, List Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,List.MarkerOffsetProperty);
                Transfer(Source,Target,List.MarkerStyleProperty);
                Transfer(Source,Target,List.StartIndexProperty);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                var SourceListItems = Source.ListItems;
                var TargetListItems = Target.ListItems;
                var SourceListItem = SourceListItems.FirstListItem;
                while (SourceListItem != null) {
                    var TargetListItem = (ListItem)Activator.CreateInstance(SourceListItem.GetType());
                    TargetListItems.Add(TargetListItem);
                    ApplyStyle(TargetListItem,Target);
                    GetFactory(SourceListItem).Transfer(SourceListItem,TargetListItem);
                    SourceListItem = SourceListItem.NextListItem;
                    }
                }
            }
        }
    }