using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Documents;

namespace BinaryStudio.PlatformUI.Extensions
    {
    public class CloneFactory
        {
        private static void CopyTo(ContextMenu Source,ContextMenu Target,FrameworkContentElement Host)
            {
            }

        private static void ApplyBindings(DependencyObject source, DependencyProperty property) {
            if ((source != null) && (property != null)) {
                var B = BindingOperations.GetBindingBase(source,property);
                if (B != null) {
                    var E = BindingOperations.GetBindingExpressionBase(source,property);
                    if (E != null) {
                        E.UpdateTarget();
                        }
                    }
                }
            }

        private static void ApplyBindings(BlockUIContainer Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(List Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(Figure Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(Floater Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(InlineUIContainer Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(LineBreak Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(Run Source) {
            if (Source == null) { return; }
            ApplyBindings(Source,Run.TextProperty);
            }

        private static void ApplyBindings(Span Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(Inline Source) {
            if (Source == null) { return; }
            ApplyBindings(Source as Figure);
            ApplyBindings(Source as Floater);
            ApplyBindings(Source as InlineUIContainer);
            ApplyBindings(Source as LineBreak);
            ApplyBindings(Source as Run);
            ApplyBindings(Source as Span);
            }

        private static void ApplyBindings(Paragraph Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var Inline in Source.Inlines.ToArray()) {
                Inline.DataContext = Inline.DataContext ?? DataContext;
                ApplyBindings(Inline);
                }
            }

        private static void ApplyBindings(Section Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var Block in Source.Blocks.ToArray()) {
                Block.DataContext = Block.DataContext ?? DataContext;
                ApplyBindings(Block);
                }
            }

        private static void ApplyBindings(TableRowGroup Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var SourceRow in Source.Rows.ToArray()) {
                SourceRow.DataContext = SourceRow.DataContext ?? DataContext;
                ApplyBindings(SourceRow);
                }
            }

        private static void ApplyBindings(Table Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var SourceRowGroup in Source.RowGroups.ToArray()) {
                SourceRowGroup.DataContext = SourceRowGroup.DataContext ?? DataContext;
                ApplyBindings(SourceRowGroup);
                }
            }

        private static void ApplyBindings(Block Source) {
            if (Source == null) { return; }
            ApplyBindings(Source as BlockUIContainer);
            ApplyBindings(Source as List);
            ApplyBindings(Source as Paragraph);
            ApplyBindings(Source as Section);
            ApplyBindings(Source as Table);
            }

        private static void ApplyBindings(TableCell Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var Block in Source.Blocks.ToArray()) {
                Block.DataContext = Block.DataContext ?? DataContext;
                ApplyBindings(Block);
                }
            }

        public static void ApplyBindings(TableRow Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var Cell in Source.Cells.ToArray()) {
                Cell.DataContext = Cell.DataContext ?? DataContext;
                ApplyBindings(Cell);
                }
            }

        private static BindingBase Clone(BindingBase Source) {
            //if (Source != null) {
            //    if (Source is Binding Binding) {
            //        var mi = typeof(BindingBase).GetMethod("Clone",BindingFlags.Instance|BindingFlags.NonPublic);
            //        return (BindingBase)mi.Invoke(Source,new Object[]{ Binding.Mode });
            //        }
            //    }
            return Source;
            }

        #region M:CopyTo(DependencyObject,DependencyObject,DependencyProperty)
        public static void CopyTo(DependencyObject Source,DependencyObject Target,DependencyProperty Property)
            {
            if (Source == null) { return; }
            var TargetValue = Target.GetValue(Property);
            var SourceValue = Source.GetValue(Property);
            var TargetMetadata = Property.GetMetadata(Target);
            var e = BindingOperations.GetBindingBase(Source,Property);
            if (e != null) {
                BindingOperations.SetBinding(Target,Property,Clone(e));
                }
            else
                {
                Target.SetValue(Property,SourceValue);
                }
            }
        #endregion
        #region M:CopyTo(Paragraph,Paragraph)
        public static void CopyTo(Paragraph Source,Paragraph Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((Block)Source,(Block)Target,Host);
            CopyTo(Source,Target,Paragraph.KeepTogetherProperty);
            CopyTo(Source,Target,Paragraph.KeepWithNextProperty);
            CopyTo(Source,Target,Paragraph.MinOrphanLinesProperty);
            CopyTo(Source,Target,Paragraph.MinWidowLinesProperty);
            CopyTo(Source,Target,Paragraph.TextDecorationsProperty);
            CopyTo(Source,Target,Paragraph.TextIndentProperty);
            var SourceInlines = Source.Inlines;
            var TargetInlines = Target.Inlines;
            foreach (var SourceInline in SourceInlines) {
                var TargetInline = (Inline)Activator.CreateInstance(SourceInline.GetType());
                TargetInlines.Add(TargetInline);
                ApplyStyle(TargetInline,Host);
                CopyTo(SourceInline as Figure, TargetInline as Figure, Host);
                CopyTo(SourceInline as Floater, TargetInline as Floater, Host);
                CopyTo(SourceInline as InlineUIContainer, TargetInline as InlineUIContainer, Host);
                CopyTo(SourceInline as LineBreak, TargetInline as LineBreak, Host);
                CopyTo(SourceInline as Run, TargetInline as Run, Host);
                CopyTo(SourceInline as Span, TargetInline as Span, Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            CopyTo(Source,Target,ContentControl.ContentProperty);
            }
        #endregion
        #region M:CopyTo(TableCell,TableCell)
        private static void CopyTo(TableCell Source,TableCell Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((TextElement)Source,Target,Host);
            CopyTo(Source,Target,TableCell.BorderBrushProperty);
            CopyTo(Source,Target,TableCell.BorderThicknessProperty);
            CopyTo(Source,Target,TableCell.ColumnSpanProperty);
            CopyTo(Source,Target,TableCell.FlowDirectionProperty);
            CopyTo(Source,Target,TableCell.LineHeightProperty);
            CopyTo(Source,Target,TableCell.LineStackingStrategyProperty);
            CopyTo(Source,Target,TableCell.PaddingProperty);
            CopyTo(Source,Target,TableCell.RowSpanProperty);
            CopyTo(Source,Target,TableCell.TextAlignmentProperty);
            var SourceBlocks = Source.Blocks;
            var TargetBlocks = Target.Blocks;
            foreach (var SourceBlock in SourceBlocks) {
                var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                TargetBlocks.Add(TargetBlock);
                ApplyStyle(TargetBlock,Host);
                CopyTo(SourceBlock as BlockUIContainer, TargetBlock as BlockUIContainer, Host);
                CopyTo(SourceBlock as List, TargetBlock as List, Host);
                CopyTo(SourceBlock as Paragraph, TargetBlock as Paragraph, Host);
                CopyTo(SourceBlock as Section, TargetBlock as Section, Host);
                CopyTo(SourceBlock as Table, TargetBlock as Table, Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(Block,Block)
        private static void CopyTo(Block Source,Block Target,FrameworkContentElement Host) {
            if (Source == null) { return; }
            CopyTo((TextElement)Source,Target,Host);
            CopyTo(Source,Target,Block.BorderBrushProperty);
            CopyTo(Source,Target,Block.BorderThicknessProperty);
            CopyTo(Source,Target,Block.BreakColumnBeforeProperty);
            CopyTo(Source,Target,Block.BreakPageBeforeProperty);
            CopyTo(Source,Target,Block.ClearFloatersProperty);
            CopyTo(Source,Target,Block.FlowDirectionProperty);
            CopyTo(Source,Target,Block.IsHyphenationEnabledProperty);
            CopyTo(Source,Target,Block.LineHeightProperty);
            CopyTo(Source,Target,Block.LineStackingStrategyProperty);
            CopyTo(Source,Target,Block.MarginProperty);
            CopyTo(Source,Target,Block.PaddingProperty);
            CopyTo(Source,Target,Block.TextAlignmentProperty);
            }
        #endregion
        #region M:CopyTo(TextElement,TextElement)
        private static void CopyTo(TextElement Source,TextElement Target,FrameworkContentElement Host) {
            if (Source == null) { return; }
            CopyTo((FrameworkContentElement)Source,Target,Host);
            CopyTo(Source,Target,TextElement.BackgroundProperty);
            CopyTo(Source,Target,TextElement.FontFamilyProperty);
            CopyTo(Source,Target,TextElement.FontSizeProperty);
            CopyTo(Source,Target,TextElement.FontStretchProperty);
            CopyTo(Source,Target,TextElement.FontStyleProperty);
            CopyTo(Source,Target,TextElement.FontWeightProperty);
            CopyTo(Source,Target,TextElement.ForegroundProperty);
            CopyTo(Source,Target,TextElement.TextEffectsProperty);
            }
        #endregion
        #region M:CopyTo(FrameworkContentElement,FrameworkContentElement)
        private static void CopyTo(FrameworkContentElement Source,FrameworkContentElement Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo(Source,Target,FrameworkContentElement.NameProperty);
            CopyTo((ContentElement)Source,Target,Host);
            CopyTo(Source,Target,FrameworkContentElement.StyleProperty);
            CopyTo(Source,Target,FrameworkContentElement.ContextMenuProperty);
            CopyTo(Source,Target,FrameworkContentElement.CursorProperty);
            CopyTo(Source,Target,FrameworkContentElement.FocusVisualStyleProperty);
            CopyTo(Source,Target,FrameworkContentElement.ForceCursorProperty);
            CopyTo(Source,Target,FrameworkContentElement.InputScopeProperty);
            CopyTo(Source,Target,FrameworkContentElement.LanguageProperty);
            CopyTo(Source,Target,FrameworkContentElement.OverridesDefaultStyleProperty);
            CopyTo(Source,Target,FrameworkContentElement.TagProperty);
            CopyTo(Source,Target,FrameworkContentElement.ToolTipProperty);
            Target.Resources = Source.Resources;
            }
        #endregion
        #region M:CopyTo(Inline,Inline)
        private static void CopyTo(Inline Source,Inline Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((TextElement)Source,Target,Host);
            CopyTo(Source,Target,Inline.BaselineAlignmentProperty);
            CopyTo(Source,Target,Inline.FlowDirectionProperty);
            CopyTo(Source,Target,Inline.TextDecorationsProperty);
            }
        #endregion
        #region M:CopyTo(Run,Run)
        private static void CopyTo(Run Source,Run Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            Target.Text = Source.Text;
            CopyTo((Inline)Source,Target,Host);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            CopyTo(Source,Target,Run.TextProperty);
            }
        #endregion
        #region M:CopyTo(Span,Span)
        private static void CopyTo(Span Source,Span Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((Inline)Source,Target,Host);
            var SourceInlines = Source.Inlines;
            var TargetInlines = Target.Inlines;
            foreach (var SourceInline in SourceInlines) {
                var TargetInline = (Inline)Activator.CreateInstance(SourceInline.GetType());
                TargetInlines.Add(TargetInline);
                ApplyStyle(TargetInline,Host);
                CopyTo(SourceInline as Figure, TargetInline as Figure, Host);
                CopyTo(SourceInline as Floater, TargetInline as Floater, Host);
                CopyTo(SourceInline as InlineUIContainer, TargetInline as InlineUIContainer, Host);
                CopyTo(SourceInline as LineBreak, TargetInline as LineBreak, Host);
                CopyTo(SourceInline as Run, TargetInline as Run, Host);
                CopyTo(SourceInline as Span, TargetInline as Span, Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(InlineUIContainer,InlineUIContainer)
        private static void CopyTo(InlineUIContainer Source,InlineUIContainer Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((Inline)Source,Target,Host);
            Target.Child = Clone(Source.Child,Host);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(AnchoredBlock,AnchoredBlock)
        private static void CopyTo(AnchoredBlock Source,AnchoredBlock Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((Inline)Source,Target,Host);
            CopyTo(Source,Target,AnchoredBlock.BorderBrushProperty);
            CopyTo(Source,Target,AnchoredBlock.BorderThicknessProperty);
            CopyTo(Source,Target,AnchoredBlock.LineHeightProperty);
            CopyTo(Source,Target,AnchoredBlock.LineStackingStrategyProperty);
            CopyTo(Source,Target,AnchoredBlock.MarginProperty);
            CopyTo(Source,Target,AnchoredBlock.PaddingProperty);
            CopyTo(Source,Target,AnchoredBlock.TextAlignmentProperty);
            var SourceBlocks = Source.Blocks;
            var TargetBlocks = Target.Blocks;
            foreach (var SourceBlock in SourceBlocks) {
                var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                TargetBlocks.Add(TargetBlock);
                ApplyStyle(TargetBlock,Host);
                CopyTo(SourceBlock as BlockUIContainer, TargetBlock as BlockUIContainer, Host);
                CopyTo(SourceBlock as List, TargetBlock as List, Host);
                CopyTo(SourceBlock as Paragraph, TargetBlock as Paragraph, Host);
                CopyTo(SourceBlock as Section, TargetBlock as Section, Host);
                CopyTo(SourceBlock as Table, TargetBlock as Table, Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(BlockUIContainer,BlockUIContainer)
        private static void CopyTo(BlockUIContainer Source,BlockUIContainer Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((Block)Source,Target,Host);
            Target.Child = Clone(Source.Child,Host);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(List,List)
        private static void CopyTo(List Source,List Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((Block)Source,Target,Host);
            CopyTo(Source,Target,List.MarkerOffsetProperty);
            CopyTo(Source,Target,List.MarkerStyleProperty);
            CopyTo(Source,Target,List.StartIndexProperty);
            var SourceListItems = Source.ListItems;
            var TargetListItems = Target.ListItems;
            foreach (var SourceListItem in SourceListItems) {
                var TargetListItem = (ListItem)Activator.CreateInstance(SourceListItem.GetType());
                TargetListItems.Add(TargetListItem);
                ApplyStyle(TargetListItem,Host);
                CopyTo(SourceListItem,TargetListItem,Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(ListItem,ListItem)
        private static void CopyTo(ListItem Source,ListItem Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((TextElement)Source,Target,Host);
            CopyTo(Source,Target,ListItem.BorderBrushProperty);
            CopyTo(Source,Target,ListItem.BorderThicknessProperty);
            CopyTo(Source,Target,ListItem.FlowDirectionProperty);
            CopyTo(Source,Target,ListItem.LineHeightProperty);
            CopyTo(Source,Target,ListItem.LineStackingStrategyProperty);
            CopyTo(Source,Target,ListItem.MarginProperty);
            CopyTo(Source,Target,ListItem.PaddingProperty);
            CopyTo(Source,Target,ListItem.TextAlignmentProperty);
            var SourceBlocks = Source.Blocks;
            var TargetBlocks = Target.Blocks;
            foreach (var SourceBlock in SourceBlocks) {
                var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                TargetBlocks.Add(TargetBlock);
                ApplyStyle(TargetBlock,Host);
                CopyTo(SourceBlock as BlockUIContainer, TargetBlock as BlockUIContainer, Host);
                CopyTo(SourceBlock as List, TargetBlock as List, Host);
                CopyTo(SourceBlock as Paragraph, TargetBlock as Paragraph, Host);
                CopyTo(SourceBlock as Section, TargetBlock as Section, Host);
                CopyTo(SourceBlock as Table, TargetBlock as Table, Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(ContentElement,ContentElement)
        private static void CopyTo(ContentElement Source,ContentElement Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo(Source,Target,ContentElement.IsEnabledProperty);
            CopyTo(Source,Target,ContentElement.FocusableProperty);
            CopyTo(Source,Target,DocumentProperties.IsAutoSizeProperty);
            CopyTo(Source,Target,DocumentProperties.IsSharedSizeScopeProperty);
            CopyTo(Source,Target,DocumentProperties.SharedGroupObjectProperty);
            CopyTo(Source,Target,DocumentProperties.WidthProperty);
            CopyTo(Source,Target,DocumentProperties.SharedSizeGroupProperty);
            CopyTo(Source,Target,DocumentProperties.DesiredSizeProperty);
            }
        #endregion
        #region M:CopyTo(Section,Section)
        public static void CopyTo(Section Source,Section Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((Block)Source,Target,Host);
            Target.HasTrailingParagraphBreakOnPaste = Source.HasTrailingParagraphBreakOnPaste;
            var SourceBlocks = Source.Blocks;
            var TargetBlocks = Target.Blocks;
            foreach (var SourceBlock in SourceBlocks) {
                var TargetBlock = (Block)Activator.CreateInstance(SourceBlock.GetType());
                TargetBlocks.Add(TargetBlock);
                ApplyStyle(TargetBlock,Host);
                CopyTo(SourceBlock as BlockUIContainer, TargetBlock as BlockUIContainer, Host);
                CopyTo(SourceBlock as List, TargetBlock as List, Host);
                CopyTo(SourceBlock as Paragraph, TargetBlock as Paragraph, Host);
                CopyTo(SourceBlock as Section, TargetBlock as Section, Host);
                CopyTo(SourceBlock as Table, TargetBlock as Table, Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            CopyTo(Source,Target,ContentControl.ContentProperty);
            }
        #endregion
        #region M:CopyTo(TableColumn,TableColumn)
        private static void CopyTo(TableColumn Source,TableColumn Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((FrameworkContentElement)Source,Target,Host);
            CopyTo(Source,Target,TableColumn.BackgroundProperty);
            CopyTo(Source,Target,TableColumn.WidthProperty);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(Table,Table)
        private static void CopyTo(Table Source,Table Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((Block)Source,Target,Host);
            CopyTo(Source,Target,Table.CellSpacingProperty);
            var SourceColumns = Source.Columns;
            var TargetColumns = Target.Columns;
            foreach (var SourceColumn in SourceColumns) {
                var TargetColumn = (TableColumn)Activator.CreateInstance(SourceColumn.GetType());
                TargetColumns.Add(TargetColumn);
                ApplyStyle(TargetColumn,Host);
                CopyTo(SourceColumn,TargetColumn,Host);
                }
            var SourceRowGroups = Source.RowGroups;
            var TargetRowGroups = Target.RowGroups;
            foreach (var SourceRowGroup in SourceRowGroups) {
                var TargetRowGroup = (TableRowGroup)Activator.CreateInstance(SourceRowGroup.GetType());
                TargetRowGroups.Add(TargetRowGroup);
                ApplyStyle(TargetRowGroup,Host);
                CopyTo(SourceRowGroup,TargetRowGroup,Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            if (DocumentProperties.GetIsAutoSize(Target)) {
                DocumentProperties.DoAutoSize(Target);
                }
            }
        #endregion
        #region M:CopyTo(TableRowGroup,TableRowGroup)
        private static void CopyTo(TableRowGroup Source,TableRowGroup Target,FrameworkContentElement Host) {
            if (Source == null) { return; }
            CopyTo((TextElement)Source,Target,Host);
            var SourceRows = Source.Rows;
            var TargetRows = Target.Rows;
            foreach (var SourceRow in SourceRows) {
                var TargetRow = (TableRow)Activator.CreateInstance(SourceRow.GetType());
                TargetRows.Add(TargetRow);
                ApplyStyle(TargetRow,Host);
                CopyTo(SourceRow,TargetRow,Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(TableRow,TableRow)
        public static void CopyTo(TableRow Source,TableRow Target,FrameworkContentElement Host) {
            if (Source == null) { return; }
            CopyTo((TextElement)Source,Target,Host);
            var SourceCells = Source.Cells;
            var TargetCells = Target.Cells;
            foreach (var SourceCell in SourceCells) {
                var TargetCell = (TableCell)Activator.CreateInstance(SourceCell.GetType());
                TargetCells.Add(TargetCell);
                ApplyStyle(TargetCell,Host);
                CopyTo(SourceCell,TargetCell,Host);
                }
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(Figure,Figure)
        private static void CopyTo(Figure Source,Figure Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((AnchoredBlock)Source,Target,Host);
            CopyTo(Source,Target,Figure.CanDelayPlacementProperty);
            CopyTo(Source,Target,Figure.HeightProperty);
            CopyTo(Source,Target,Figure.HorizontalAnchorProperty);
            CopyTo(Source,Target,Figure.HorizontalOffsetProperty);
            CopyTo(Source,Target,Figure.VerticalAnchorProperty);
            CopyTo(Source,Target,Figure.VerticalOffsetProperty);
            CopyTo(Source,Target,Figure.WidthProperty);
            CopyTo(Source,Target,Figure.WrapDirectionProperty);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion
        #region M:CopyTo(Floater,Floater)
        private static void CopyTo(Floater Source,Floater Target,FrameworkContentElement Host)
            {
            if (Source == null) { return; }
            CopyTo((AnchoredBlock)Source,Target,Host);
            CopyTo(Source,Target,Floater.HorizontalAlignmentProperty);
            CopyTo(Source,Target,Floater.WidthProperty);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        #endregion

        private static UIElement Clone(UIElement Source, FrameworkContentElement Host)
            {
            throw new NotImplementedException();
            }

        private static T Load<T>(FrameworkContentElement Source, T Target)
            where T: FrameworkContentElement
            {
            if (Target != null) {
                if (!Target.IsLoaded) {
                    CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                    Target.RaiseEvent(new RoutedEventArgs(FrameworkContentElement.LoadedEvent));
                    }
                }
            return Target;
            }

        private static void ApplyStyle<T>(T Target,FrameworkContentElement Host)
            where T: FrameworkContentElement
            {
            if (Host != null) {
                if (Host.TryFindResource(typeof(T)) is Style Style) {
                    Target.Style = Style;
                    return;
                    }
                if (Host.TryFindResource(Target.GetType()) is Style Style2) {
                    Target.Style = Style2;
                    return;
                    }
                }
            }
        }
    }