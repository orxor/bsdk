using System;
using System.Windows;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(List))]
    internal class CloneListFactory : CloneBlockFactory<List>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(List Source, List Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            CopyTo(Source,Target,List.MarkerOffsetProperty);
            CopyTo(Source,Target,List.MarkerStyleProperty);
            CopyTo(Source,Target,List.StartIndexProperty);
            var SourceListItems = Source.ListItems;
            var TargetListItems = Target.ListItems;
            foreach (var SourceListItem in SourceListItems) {
                var TargetListItem = (ListItem)Activator.CreateInstance(SourceListItem.GetType());
                TargetListItems.Add(TargetListItem);
                //ApplyStyle(TargetListItem,Host);
                GetFactory(SourceListItem.GetType()).CopyTo(SourceListItem,TargetListItem);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        }
    }