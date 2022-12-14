using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Controls;
using log4net;

namespace BinaryStudio.PlatformUI.Documents
    {
    public class TextProperties
        {
        private class SharedSizeGroupObject : DependencyObject
            {
            public String SharedSizeGroup { get;set; }
            public SharedSizeGroupObject(String SharedSizeGroup)
                {
                this.SharedSizeGroup = SharedSizeGroup;
                }
            }

        private static readonly ILog logger = LogManager.GetLogger(nameof(TextProperties));
        private static readonly IDictionary<Object,DependencyObject> Values = new Dictionary<Object,DependencyObject>();

        #region P:TextProperties.SharedGroupObject:DependencyObject
        internal static readonly DependencyProperty SharedGroupObjectProperty = DependencyProperty.RegisterAttached("SharedGroupObject", typeof(DependencyObject), typeof(TextProperties), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.Inherits));
        private static void SetSharedGroupObject(DependencyObject element, DependencyObject value)
            {
            element.SetValue(SharedGroupObjectProperty, value);
            }
        private static SharedSizeGroupObject GetSharedGroupObject(DependencyObject element)
            {
            return (SharedSizeGroupObject)element.GetValue(SharedGroupObjectProperty);
            }
        #endregion
        #region P:TextProperties.SharedSizeGroup:String
        public static readonly DependencyProperty SharedSizeGroupProperty = DependencyProperty.RegisterAttached("SharedSizeGroup", typeof(String), typeof(TextProperties), new PropertyMetadata(default(String),OnSharedSizeGroupChanged));
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
                    return;
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
        #region P:TextProperties.IsSharedSizeScope:Boolean
        public static readonly DependencyProperty IsSharedSizeScopeProperty = DependencyProperty.RegisterAttached("IsSharedSizeScope", typeof(Boolean), typeof(TextProperties), new PropertyMetadata(default(Boolean)));
        public static void SetIsSharedSizeScope(DependencyObject element, Boolean value)
            {
            element.SetValue(IsSharedSizeScopeProperty, value);
            }

        public static Boolean GetIsSharedSizeScope(DependencyObject element)
            {
            return (Boolean) element.GetValue(IsSharedSizeScopeProperty);
            }
        #endregion
        #region P:TextProperties.IsAutoSize:Boolean
        public static readonly DependencyProperty IsAutoSizeProperty = DependencyProperty.RegisterAttached("IsAutoSize", typeof(Boolean), typeof(TextProperties), new PropertyMetadata(default(Boolean),OnIsAutoSizeChanged));
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
        #region P:TextProperties.Width:Double
        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(Double), typeof(TextProperties), new PropertyMetadata(default(Double),OnWidthChanged));
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
        #region P:TextProperties.DesiredSize:Size
        public static readonly DependencyProperty DesiredSizeProperty = DependencyProperty.RegisterAttached("DesiredSize", typeof(Size), typeof(TextProperties), new PropertyMetadata(default(Size)));
        public static void SetDesiredSize(DependencyObject element, Size value)
            {
            element.SetValue(DesiredSizeProperty, value);
            }

        public static Size GetDesiredSize(DependencyObject element) {
            return (Size)element.GetValue(DesiredSizeProperty);
            }
        #endregion

        #region M:GetDesiredSize(IDictionary<String,Vector>,Inline):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Inline Source) {
            if (Source == null) { return new Vector(0,0); }
            return GetDesiredSize(context,Source as AnchoredBlock) +
                   GetDesiredSize(context,Source as InlineUIContainer) +
                   GetDesiredSize(context,Source as LineBreak) +
                   GetDesiredSize(context,Source as Run) +
                   GetDesiredSize(context,Source as Span);
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,Block):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Block Source) {
            if (Source == null) { return new Vector(0,0); }
            return GetDesiredSize(context,Source as BlockUIContainer) +
                   GetDesiredSize(context,Source as List) +
                   GetDesiredSize(context,Source as Paragraph) +
                   GetDesiredSize(context,Source as Section) +
                   GetDesiredSize(context,Source as Table);
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,BlockUIContainer):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,BlockUIContainer Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            if (Source.Child != null) {
                Source.Child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                var Size = Source.Child.DesiredSize;
                r.X += Size.Width;
                r.Y += Size.Height;
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,List):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,List Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(Source.MarkerOffset,0);
            foreach (var ListItem in Source.ListItems) {
                r += GetDesiredSize(context,ListItem);
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,ListItem):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,ListItem Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            foreach (var Block in Source.Blocks) {
                r += GetDesiredSize(context,Block);
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,Paragraph):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Paragraph Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            foreach (var Inline in Source.Inlines) {
                var Target = GetDesiredSize(context,Inline);
                r.Y = Math.Max(r.Y,Target.Y);
                r.X += Target.X;
                }
            r.X += Source.TextIndent;
            r.X += Source.Padding.Left + Source.Padding.Right  + FilterNaN(Source.Margin.Left,0) + FilterNaN(Source.Margin.Right,0);
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + FilterNaN(Source.Margin.Top,0)  + FilterNaN(Source.Margin.Bottom,0);
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,AnchoredBlock):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,AnchoredBlock Source) {
            if (Source == null) { return new Vector(0,0); }
            return GetDesiredSize(context,Source as Figure) +
                   GetDesiredSize(context,Source as Floater);
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,Figure):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Figure Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(Source.VerticalOffset,Source.HorizontalOffset);
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,Floater):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Floater Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,InlineUIContainer):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,InlineUIContainer Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            if (Source.Child != null) {
                Source.Child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                var Size = Source.Child.DesiredSize;
                r.X += Size.Width;
                r.Y += Size.Height;
                }
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,LineBreak):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,LineBreak Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,Run):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Run Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            var o = new FormattedText(
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
            r = new Vector(Math.Max(o.Width,Source.FontSize*0.5),o.Height);
            SetDesiredSize(Source,new Size(Math.Max(o.Width,Source.FontSize*0.5),o.Height));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,Span):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Span Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            foreach (var Inline in Source.Inlines) {
                r += GetDesiredSize(context,Inline);
                }
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,Section):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Section Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            foreach (var Block in Source.Blocks) {
                r += GetDesiredSize(context,Block);
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom;
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,Table):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,Table Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            foreach (var RowGroup in Source.RowGroups) {
                var Target = GetDesiredSize(context,RowGroup);
                r.X = Math.Max(r.X, Target.X);
                r.Y += Target.Y;
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.Margin.Left + Source.Margin.Right  + Source.BorderThickness.Left + Source.BorderThickness.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.Margin.Top  + Source.Margin.Bottom + Source.BorderThickness.Top  + Source.BorderThickness.Bottom;
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,TableRowGroup):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,TableRowGroup Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            foreach (var Row in Source.Rows) {
                var Target = GetDesiredSize(context,Row);
                r.X = Math.Max(r.X,Target.X);
                r.Y += Target.Y;
                }
            SetDesiredSize(Source,new Size(r.X,r.Y));
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,TableRow):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,TableRow Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            foreach (var Cell in Source.Cells) {
                var Target = GetDesiredSize(context,Cell);
                r.Y = Math.Max(r.Y,Target.Y);
                r.X += Target.X;
                }
            context[Key] = r;
            return r;
            }
        #endregion
        #region M:GetDesiredSize(IDictionary<String,Vector>,TableCell):Vector
        protected static Vector GetDesiredSize(IDictionary<String,Vector> context,TableCell Source) {
            if (Source == null) { return new Vector(0,0); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return r; }
            r = new Vector(0,0);
            foreach (var Block in Source.Blocks) {
                var Target = GetDesiredSize(context,Block);
                r.X = Math.Max(r.X,Target.X);
                r.Y += Target.Y;
                }
            r.X += Source.Padding.Left + Source.Padding.Right  + Source.BorderThickness.Left + Source.BorderThickness.Right;
            r.Y += Source.Padding.Top  + Source.Padding.Bottom + Source.BorderThickness.Top  + Source.BorderThickness.Bottom;
            context[Key] = r;
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

        internal static void DoAutoSize(Table Source, IDictionary<String,Vector> context) {
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            var Key = Diagnostics.GetKey(Source).ToString();
            if (context.TryGetValue(Key, out var r)) { return; }
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
                            DesiredWidth[j] = Math.Max(DesiredWidth[j],GetDesiredSize(context,Cell).X);
                            }
                        else
                            {
                            //DesiredWidth[j] = Math.Max(DesiredWidth[j],GetDesiredSize(Cell).X);
                            }
                        j += Cell.ColumnSpan;
                        }
                    }
                }
            for (var i = 0; i < DesiredWidth.Length;i++) {
                if (DesiredWidth[i] >= 0) {
                    var SharedGroupObject = GetSharedGroupObject(Source.Columns[i]);
                    if (SharedGroupObject != null) {
                        var value = GetWidth(SharedGroupObject);
                        SetWidth(SharedGroupObject,0);
                        SetWidth(SharedGroupObject,Math.Max(DesiredWidth[i],value));
                        }
                    else if (GetIsAutoSize(Source.Columns[i])) { Source.Columns[i].Width = new GridLength(DesiredWidth[i],GridUnitType.Pixel); }
                    }
                }
            }

        internal static void DoAutoSize(Table Source) {
            DoAutoSize(Source,new Dictionary<String,Vector>());
            }
        }
    }