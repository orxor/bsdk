using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using BinaryStudio.PlatformUI.Controls;

namespace BinaryStudio.PlatformUI.Documents
    {
    public class DocumentProperties
        {
        private class SharedSizeGroupObject : DependencyObject
            {
            public String SharedSizeGroup { get;set; }
            public SharedSizeGroupObject(String SharedSizeGroup)
                {
                this.SharedSizeGroup = SharedSizeGroup;
                }
            }

        private static readonly IDictionary<Object,DependencyObject> Values = new Dictionary<Object,DependencyObject>();

        #region P:DocumentProperties.SharedGroupObject:DependencyObject
        internal static readonly DependencyProperty SharedGroupObjectProperty = DependencyProperty.RegisterAttached("SharedGroupObject", typeof(DependencyObject), typeof(DocumentProperties), new PropertyMetadata(default(DependencyObject)));
        private static void SetSharedGroupObject(DependencyObject element, DependencyObject value)
            {
            element.SetValue(SharedGroupObjectProperty, value);
            }
        private static DependencyObject GetSharedGroupObject(DependencyObject element)
            {
            return (DependencyObject)element.GetValue(SharedGroupObjectProperty);
            }
        #endregion
        #region P:DocumentProperties.SharedSizeGroup:String
        public static readonly DependencyProperty SharedSizeGroupProperty = DependencyProperty.RegisterAttached("SharedSizeGroup", typeof(String), typeof(DocumentProperties), new PropertyMetadata(default(String),OnSharedSizeGroupChanged));
        private static void OnSharedSizeGroupChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            OnSharedSizeGroupChanged(sender as TableColumn, (String)e.NewValue);
            }
        private static void OnSharedSizeGroupChanged(TableColumn Source,String SharedSizeGroup) {
            foreach (var o in Source.Ancestors<DependencyObject>()) {
                if (GetIsSharedSizeScope(o)) {
                    var Key = Tuple.Create(SharedSizeGroup,o);
                    if (!Values.TryGetValue(Key, out var SharedGroupObject)) {
                        Values[Key] = SharedGroupObject = new SharedSizeGroupObject(SharedSizeGroup);
                        }
                    SetSharedGroupObject(Source,SharedGroupObject);
                    Source.SetBinding(WidthProperty,SharedGroupObject,WidthProperty,BindingMode.TwoWay);
                    break;
                    }
                }
            }

        public static void SetSharedSizeGroup(DependencyObject element, String value)
            {
            element.SetValue(SharedSizeGroupProperty, value);
            }

        public static String GetSharedSizeGroup(DependencyObject element)
            {
            return (String) element.GetValue(SharedSizeGroupProperty);
            }
        #endregion
        #region P:DocumentProperties.IsSharedSizeScope:Boolean
        public static readonly DependencyProperty IsSharedSizeScopeProperty = DependencyProperty.RegisterAttached("IsSharedSizeScope", typeof(Boolean), typeof(DocumentProperties), new PropertyMetadata(default(Boolean)));
        public static void SetIsSharedSizeScope(DependencyObject element, Boolean value)
            {
            element.SetValue(IsSharedSizeScopeProperty, value);
            }

        public static Boolean GetIsSharedSizeScope(DependencyObject element)
            {
            return (Boolean) element.GetValue(IsSharedSizeScopeProperty);
            }
        #endregion
        #region P:DocumentProperties.IsAutoSize:Boolean
        public static readonly DependencyProperty IsAutoSizeProperty = DependencyProperty.RegisterAttached("IsAutoSize", typeof(Boolean), typeof(DocumentProperties), new PropertyMetadata(default(Boolean),OnIsAutoSizeChanged));
        private static void OnIsAutoSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            OnIsAutoSizeChanged(sender as Table,(Boolean)e.NewValue);
            }

        private static void OnIsAutoSizeChanged(Table Source,Boolean IsAutoSize) {
            if ((Source != null) && IsAutoSize) {
                var Host = Source.Ancestors<RichTextBox>().FirstOrDefault();
                if (Host != null) {
                    var Document = Host.Document;
                    if (Document != null) {
                        if (!Document.IsLoaded) {
                            void Handler(Object sender, RoutedEventArgs args) {
                                Document.Loaded -= Handler;
                                DoAutoSize(Source);
                                };
                            Document.Loaded += Handler;
                            }
                        else
                            {
                            DoAutoSize(Source);
                            }
                        }
                    }
                }
            }

        public static void SetIsAutoSize(DependencyObject element, Boolean value)
            {
            element.SetValue(IsAutoSizeProperty, value);
            }

        public static Boolean GetIsAutoSize(DependencyObject element)
            {
            return (Boolean) element.GetValue(IsAutoSizeProperty);
            }
        #endregion
        #region P:DocumentProperties.Width:Double
        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(Double), typeof(DocumentProperties), new PropertyMetadata(default(Double),OnWidthChanged));
        private static void OnWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is TableColumn TableColumn) {
                var value = (Double)e.NewValue;
                TableColumn.Width = Double.IsNaN(value)
                    ? new GridLength(0,GridUnitType.Auto)
                    : new GridLength(value,GridUnitType.Pixel);
                }
            }
        public static void SetWidth(DependencyObject element, Double value)
            {
            element.SetValue(WidthProperty, value);
            }

        public static Double GetWidth(DependencyObject element)
            {
            return (Double) element.GetValue(WidthProperty);
            }
        #endregion
        #region P:DocumentProperties.DesiredSize:Size
        public static readonly DependencyProperty DesiredSizeProperty = DependencyProperty.RegisterAttached("DesiredSize", typeof(Size), typeof(DocumentProperties), new PropertyMetadata(default(Size)));
        public static void SetDesiredSize(DependencyObject element, Size value)
            {
            element.SetValue(DesiredSizeProperty, value);
            }

        public static Size GetDesiredSize(DependencyObject element)
            {
            return (Size) element.GetValue(DesiredSizeProperty);
            }
        #endregion

        #region M:GetDesiredSize(Inline):Vector
        protected static Vector GetDesiredSize(Inline Source) {
            if (Source == null) { return new Vector(0,0); }
            return GetDesiredSize(Source as AnchoredBlock) +
                   GetDesiredSize(Source as InlineUIContainer) +
                   GetDesiredSize(Source as LineBreak) +
                   GetDesiredSize(Source as Run) +
                   GetDesiredSize(Source as Span);
            }
        #endregion
        #region M:GetDesiredSize(Block):Vector
        protected static Vector GetDesiredSize(Block Source) {
            if (Source == null) { return new Vector(0,0); }
            return GetDesiredSize(Source as BlockUIContainer) +
                   GetDesiredSize(Source as List) +
                   GetDesiredSize(Source as Paragraph) +
                   GetDesiredSize(Source as Section) +
                   GetDesiredSize(Source as Table);
            }
        #endregion
        #region M:GetDesiredSize(BlockUIContainer):Vector
        protected static Vector GetDesiredSize(BlockUIContainer Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            if (Source.Child != null) {
                Source.Child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                var Size = Source.Child.DesiredSize;
                r.X += Size.Width;
                r.Y += Size.Height;
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(List):Vector
        protected static Vector GetDesiredSize(List Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(Source.MarkerOffset,0);
            foreach (var ListItem in Source.ListItems) {
                r += GetDesiredSize(ListItem);
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(ListItem):Vector
        protected static Vector GetDesiredSize(ListItem Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            foreach (var Block in Source.Blocks) {
                r += GetDesiredSize(Block);
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(Paragraph):Vector
        protected static Vector GetDesiredSize(Paragraph Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            foreach (var Inline in Source.Inlines) {
                var Target = GetDesiredSize(Inline);
                r.Y = Math.Max(r.Y,Target.Y);
                r.X += Target.X;
                }
            r.X += Source.TextIndent;
            r.X += Source.Padding.Left + Source.Padding.Right  + FilterNaN(Source.Margin.Left,0) + FilterNaN(Source.Margin.Right,0);
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + FilterNaN(Source.Margin.Top,0)  + FilterNaN(Source.Margin.Bottom,0);
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(AnchoredBlock):Vector
        protected static Vector GetDesiredSize(AnchoredBlock Source) {
            if (Source == null) { return new Vector(0,0); }
            return GetDesiredSize(Source as Figure) +
                   GetDesiredSize(Source as Floater);
            }
        #endregion
        #region M:GetDesiredSize(Figure):Vector
        protected static Vector GetDesiredSize(Figure Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(Source.VerticalOffset,Source.HorizontalOffset);
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(Floater):Vector
        protected static Vector GetDesiredSize(Floater Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(InlineUIContainer):Vector
        protected static Vector GetDesiredSize(InlineUIContainer Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            if (Source.Child != null) {
                Source.Child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                var Size = Source.Child.DesiredSize;
                r.X += Size.Width;
                r.Y += Size.Height;
                }
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(LineBreak):Vector
        protected static Vector GetDesiredSize(LineBreak Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(Run):Vector
        protected static Vector GetDesiredSize(Run Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new FormattedText(
                Source.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(
                    Source.FontFamily,
                    Source.FontStyle,
                    Source.FontWeight,
                    Source.FontStretch),
                    Source.FontSize,
                Brushes.Black,
                null,
                TextFormattingMode.Display);
            SetDesiredSize(Source,new Size(r.Width,r.Height));
            return new Vector(r.Width,r.Height);
            }
        #endregion
        #region M:GetDesiredSize(Span):Vector
        protected static Vector GetDesiredSize(Span Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            foreach (var Inline in Source.Inlines) {
                r += GetDesiredSize(Inline);
                }
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(Section):Vector
        protected static Vector GetDesiredSize(Section Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            foreach (var Block in Source.Blocks) {
                r += GetDesiredSize(Block);
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(Table):Vector
        protected static Vector GetDesiredSize(Table Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            foreach (var RowGroup in Source.RowGroups) {
                var Target = GetDesiredSize(RowGroup);
                r.X = Math.Max(r.X, Target.X);
                r.Y += Target.Y;
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right  + Source.BorderThickness.Left + Source.BorderThickness.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom + Source.BorderThickness.Top  + Source.BorderThickness.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(TableRowGroup):Vector
        protected static Vector GetDesiredSize(TableRowGroup Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            foreach (var Row in Source.Rows) {
                var Target = GetDesiredSize(Row);
                r.X = Math.Max(r.X,Target.X);
                r.Y += Target.Y;
                }
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(TableRow):Vector
        protected static Vector GetDesiredSize(TableRow Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            foreach (var Cell in Source.Cells) {
                var Target = GetDesiredSize(Cell);
                r.Y = Math.Max(r.Y,Target.Y);
                r.X += Target.X;
                }
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:GetDesiredSize(TableCell):Vector
        protected static Vector GetDesiredSize(TableCell Source) {
            if (Source == null) { return new Vector(0,0); }
            var r = new Vector(0,0);
            foreach (var Block in Source.Blocks) {
                var Target = GetDesiredSize(Block);
                r.X = Math.Max(r.X,Target.X);
                r.Y += Target.Y;
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.BorderThickness.Left + Source.BorderThickness.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.BorderThickness.Top  + Source.BorderThickness.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            return r;
            }
        #endregion
        #region M:FilterNaN(Double,Double):Double
        private static Double FilterNaN(Double Source, Double Default)
            {
            return (!Double.IsNaN(Source))
                ? Source
                : Default;
            }
        #endregion

        internal static void DoAutoSize(Table Source) {
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            //return;
            #if DEBUG
            GetDesiredSize(Source);
            #endif
            var DesiredWidth = new Double[Source.Columns.Count];
            for (var i = 0; i < DesiredWidth.Length;i++) {
                DesiredWidth[i] = -1;
                }
            foreach (var RowGroup in Source.RowGroups) {
                foreach (var Row in RowGroup.Rows) {
                    var j = 0;
                    for (var i = 0; i < Row.Cells.Count; i++) {
                        var Cell = Row.Cells[i];
                        if (Cell.ColumnSpan == 1) {
                            DesiredWidth[j] = Math.Max(DesiredWidth[j],GetDesiredSize(Cell).X);
                            }
                        j += Cell.ColumnSpan;
                        }
                    }
                }
            for (var i = 0; i < DesiredWidth.Length;i++) {
                if (DesiredWidth[i] >= 0) {
                    var SharedGroupObject = GetSharedGroupObject(Source.Columns[i]);
                    if (SharedGroupObject != null) {
                        SetWidth(SharedGroupObject,DesiredWidth[i]);
                        }
                    else if (GetIsAutoSize(Source.Columns[i])) { Source.Columns[i].Width = new GridLength(DesiredWidth[i],GridUnitType.Pixel); }
                    }
                }
            }

        //public static void WriteXaml(
        }
    }