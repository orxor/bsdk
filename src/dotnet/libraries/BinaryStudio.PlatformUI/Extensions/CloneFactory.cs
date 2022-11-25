using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace BinaryStudio.PlatformUI.Extensions
    {
    public class CloneFactory
        {
        private static void CopyTo(ContextMenu Source,ContextMenu Target,FrameworkContentElement Host)
            {
            }

        #region M:CopyTo(DependencyObject,DependencyObject,DependencyProperty)
        private static void CopyTo(DependencyObject Source,DependencyObject Target,DependencyProperty Property)
            {
            var TargetValue = Target.GetValue(Property);
            var SourceValue = Source.GetValue(Property);
            var TargetMetadata = Property.GetMetadata(Target);
            if (Equals(TargetValue,TargetMetadata.DefaultValue)) {
                Target.SetValue(Property,SourceValue);
                }
            //var e = BindingOperations.GetBindingBase(Source,Property);
            //if (e != null) {
            //    BindingOperations.SetBinding(Target,Property,e);
            //    }
            //else
                {
                
                }
            }
        #endregion
        #region M:CopyTo(Paragraph,Paragraph)
        private static void CopyTo(Paragraph Source,Paragraph Target,FrameworkContentElement Host)
            {
            CopyTo((Block)Source,(Block)Target,Host);
            CopyTo(Source,Target,Paragraph.KeepTogetherProperty);
            CopyTo(Source,Target,Paragraph.KeepWithNextProperty);
            CopyTo(Source,Target,Paragraph.MinOrphanLinesProperty);
            CopyTo(Source,Target,Paragraph.MinWidowLinesProperty);
            CopyTo(Source,Target,Paragraph.TextDecorationsProperty);
            CopyTo(Source,Target,Paragraph.TextIndentProperty);
            var SourceInlines = Source.Inlines;
            var TargetInlines = Target.Inlines;
            foreach (var Inline in SourceInlines) {
                TargetInlines.Add(Clone(Inline,Host));
                }
            }
        #endregion
        #region M:CopyTo(Block,Block)
        private static void CopyTo(Block Source,Block Target,FrameworkContentElement Host)
            {
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
        #region M:CopyTo(TableCell,TableCell)
        private static void CopyTo(TableCell Source,TableCell Target,FrameworkContentElement Host)
            {
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
            foreach (var Block in SourceBlocks) {
                TargetBlocks.Add(Clone(Block,Host));
                }
            }
        #endregion
        #region M:CopyTo(TextElement,TextElement)
        private static void CopyTo(TextElement Source,TextElement Target,FrameworkContentElement Host)
            {
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
            CopyTo((ContentElement)Source,Target,Host);
            CopyTo(Source,Target,FrameworkContentElement.StyleProperty);
            CopyTo(Source,Target,FrameworkContentElement.ContextMenuProperty);
            CopyTo(Source,Target,FrameworkContentElement.CursorProperty);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            CopyTo(Source,Target,FrameworkContentElement.FocusVisualStyleProperty);
            CopyTo(Source,Target,FrameworkContentElement.ForceCursorProperty);
            CopyTo(Source,Target,FrameworkContentElement.InputScopeProperty);
            CopyTo(Source,Target,FrameworkContentElement.LanguageProperty);
            CopyTo(Source,Target,FrameworkContentElement.NameProperty);
            CopyTo(Source,Target,FrameworkContentElement.OverridesDefaultStyleProperty);
            CopyTo(Source,Target,FrameworkContentElement.TagProperty);
            CopyTo(Source,Target,FrameworkContentElement.ToolTipProperty);
            Target.Resources = Source.Resources;
            }
        #endregion
        #region M:CopyTo(Inline,Inline)
        private static void CopyTo(Inline Source,Inline Target,FrameworkContentElement Host)
            {
            CopyTo((TextElement)Source,Target,Host);
            CopyTo(Source,Target,Inline.BaselineAlignmentProperty);
            CopyTo(Source,Target,Inline.FlowDirectionProperty);
            CopyTo(Source,Target,Inline.TextDecorationsProperty);
            }
        #endregion
        #region M:CopyTo(Run,Run)
        private static void CopyTo(Run Source,Run Target,FrameworkContentElement Host)
            {
            CopyTo((Inline)Source,Target,Host);
            Target.Text = Source.Text;
            CopyTo(Source,Target,Run.TextProperty);
            }
        #endregion
        #region M:CopyTo(Span,Span)
        private static void CopyTo(Span Source,Span Target,FrameworkContentElement Host)
            {
            CopyTo((Inline)Source,Target,Host);
            var SourceInlines = Source.Inlines;
            var TargetInlines = Target.Inlines;
            foreach (var Inline in SourceInlines) {
                TargetInlines.Add(Clone(Inline,Host));
                }
            }
        #endregion
        #region M:CopyTo(InlineUIContainer,InlineUIContainer)
        private static void CopyTo(InlineUIContainer Source,InlineUIContainer Target,FrameworkContentElement Host)
            {
            CopyTo((Inline)Source,Target,Host);
            Target.Child = Clone(Source.Child,Host);
            }
        #endregion
        #region M:CopyTo(AnchoredBlock,AnchoredBlock)
        private static void CopyTo(AnchoredBlock Source,AnchoredBlock Target,FrameworkContentElement Host)
            {
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
            foreach (var Block in SourceBlocks) {
                TargetBlocks.Add(Clone(Block,Host));
                }
            }
        #endregion
        #region M:CopyTo(BlockUIContainer,BlockUIContainer)
        private static void CopyTo(BlockUIContainer Source,BlockUIContainer Target,FrameworkContentElement Host)
            {
            CopyTo((Block)Source,Target,Host);
            Target.Child = Clone(Source.Child,Host);
            }
        #endregion
        #region M:CopyTo(List,List)
        private static void CopyTo(List Source,List Target,FrameworkContentElement Host)
            {
            CopyTo((Block)Source,Target,Host);
            CopyTo(Source,Target,List.MarkerOffsetProperty);
            CopyTo(Source,Target,List.MarkerStyleProperty);
            CopyTo(Source,Target,List.StartIndexProperty);
            var SourceListItems = Source.ListItems;
            var TargetListItems = Target.ListItems;
            foreach (var ListItem in SourceListItems) {
                TargetListItems.Add(Clone(ListItem,Host));
                }
            }
        #endregion
        #region M:CopyTo(ListItem,ListItem)
        private static void CopyTo(ListItem Source,ListItem Target,FrameworkContentElement Host)
            {
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
            foreach (var Block in SourceBlocks) {
                TargetBlocks.Add(Clone(Block,Host));
                }
            }
        #endregion
        #region M:CopyTo(Figure,Figure)
        private static void CopyTo(Figure Source,Figure Target,FrameworkContentElement Host)
            {
            CopyTo((AnchoredBlock)Source,Target,Host);
            CopyTo(Source,Target,Figure.CanDelayPlacementProperty);
            CopyTo(Source,Target,Figure.HeightProperty);
            CopyTo(Source,Target,Figure.HorizontalAnchorProperty);
            CopyTo(Source,Target,Figure.HorizontalOffsetProperty);
            CopyTo(Source,Target,Figure.VerticalAnchorProperty);
            CopyTo(Source,Target,Figure.VerticalOffsetProperty);
            CopyTo(Source,Target,Figure.WidthProperty);
            CopyTo(Source,Target,Figure.WrapDirectionProperty);
            }
        #endregion
        #region M:CopyTo(Floater,Floater)
        private static void CopyTo(Floater Source,Floater Target,FrameworkContentElement Host)
            {
            CopyTo((AnchoredBlock)Source,Target,Host);
            CopyTo(Source,Target,Floater.HorizontalAlignmentProperty);
            CopyTo(Source,Target,Floater.WidthProperty);
            }
        #endregion
        #region M:CopyTo(ContentElement,ContentElement)
        private static void CopyTo(ContentElement Source,ContentElement Target,FrameworkContentElement Host)
            {
            CopyTo(Source,Target,ContentElement.AllowDropProperty);
            CopyTo(Source,Target,ContentElement.FocusableProperty);
            CopyTo(Source,Target,ContentElement.IsEnabledProperty);
            }
        #endregion
        #region M:CopyTo(Section,Section)
        private static void CopyTo(Section Source,Section Target,FrameworkContentElement Host)
            {
            CopyTo((Block)Source,Target,Host);
            Target.HasTrailingParagraphBreakOnPaste = Source.HasTrailingParagraphBreakOnPaste;
            var SourceBlocks = Source.Blocks;
            var TargetBlocks = Target.Blocks;
            foreach (var Block in SourceBlocks) {
                TargetBlocks.Add(Clone(Block,Host));
                }
            }
        #endregion
        #region M:CopyTo(Table,Table)
        private static void CopyTo(Table Source,Table Target,FrameworkContentElement Host)
            {
            CopyTo((Block)Source,Target,Host);
            CopyTo(Source,Target,Table.CellSpacingProperty);
            var SourceColumns = Source.Columns;
            var TargetColumns = Target.Columns;
            foreach (var Column in SourceColumns) {
                TargetColumns.Add(Clone(Column,Host));
                }
            var SourceRowGroups = Source.RowGroups;
            var TargetRowGroups = Target.RowGroups;
            foreach (var RowGroup in SourceRowGroups) {
                TargetRowGroups.Add(Clone(RowGroup,Host));
                }
            }
        #endregion
        #region M:CopyTo(TableColumn,TableColumn)
        private static void CopyTo(TableColumn Source,TableColumn Target,FrameworkContentElement Host)
            {
            CopyTo((FrameworkContentElement)Source,Target,Host);
            CopyTo(Source,Target,TableColumn.BackgroundProperty);
            CopyTo(Source,Target,TableColumn.WidthProperty);
            }
        #endregion
        #region M:CopyTo(TableRowGroup,TableRowGroup)
        private static void CopyTo(TableRowGroup Source,TableRowGroup Target,FrameworkContentElement Host)
            {
            CopyTo((TextElement)Source,Target,Host);
            var SourceRows = Source.Rows;
            var TargetRows = Target.Rows;
            foreach (var Row in SourceRows) {
                TargetRows.Add(Clone(Row,Host));
                }
            }
        #endregion
        #region M:CopyTo(TableRow,TableRow)
        private static void CopyTo(TableRow Source,TableRow Target,FrameworkContentElement Host)
            {
            CopyTo((TextElement)Source,Target,Host);
            var SourceCells = Source.Cells;
            var TargetCells = Target.Cells;
            foreach (var Cell in SourceCells) {
                TargetCells.Add(Clone(Cell,Host));
                }
            }
        #endregion

        public static TableColumn Clone(TableColumn Source, FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new TableColumn();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static TableRow Clone(TableRow Source, FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new TableRow();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static TableRowGroup Clone(TableRowGroup Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new TableRowGroup();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static TableCell Clone(TableCell Source, FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new TableCell();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static ContextMenu Clone(ContextMenu Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new ContextMenu();
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static Paragraph Clone(Paragraph Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new Paragraph();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static BlockUIContainer Clone(BlockUIContainer Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new BlockUIContainer();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static List Clone(List Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new List();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static Section Clone(Section Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new Section();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static Table Clone(Table Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new Table();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        private static Block Clone(Block Source,FrameworkContentElement Host)
            {
            return (Block)Clone(Source as BlockUIContainer,Host)
                ?? (Block)Clone(Source as List,Host)
                ?? (Block)Clone(Source as Paragraph,Host)
                ?? (Block)Clone(Source as Section,Host)
                ?? (Block)Clone(Source as Table,Host);
            }

        private static AnchoredBlock Clone(AnchoredBlock Source,FrameworkContentElement Host)
            {
            return (AnchoredBlock)Clone(Source as Figure,Host)
                ?? Clone(Source as Floater,Host);
            }

        private static Figure Clone(Figure Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new Figure();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        private static Floater Clone(Floater Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new Floater();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static InlineUIContainer Clone(InlineUIContainer Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new InlineUIContainer();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static LineBreak Clone(LineBreak Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new LineBreak();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static Run Clone(Run Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new Run();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        public static Span Clone(Span Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new Span();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        private static UIElement Clone(UIElement Source,FrameworkContentElement Host)
            {
            throw new NotImplementedException();
            }

        private static ListItem Clone(ListItem Source,FrameworkContentElement Host)
            {
            if (Source == null) { return null; }
            var Target = new ListItem();
            ApplyStyle(Target,Host);
            CopyTo(Source,Target,Host);
            return Target;
            }

        private static Inline Clone(Inline Source,FrameworkContentElement Host)
            {
            return (Inline)Clone(Source as AnchoredBlock,Host)
                ?? (Inline)Clone(Source as InlineUIContainer,Host)
                ?? (Inline)Clone(Source as LineBreak,Host)
                ?? (Inline)Clone(Source as Run,Host)
                ?? (Inline)Clone(Source as Span,Host);
            }

        private static void ApplyStyle<T>(T Target,FrameworkContentElement Host)
            where T: FrameworkContentElement
            {
            if (Host != null) {
                if (Host.TryFindResource(typeof(T)) is Style Style) {
                    Target.Style = Style;
                    }
                }
            }
        }
    }