using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BinaryStudio.PlatformUI.Controls
    {
    public class RichTextBoxOptions
        { 
        public static readonly DependencyProperty IsAutoFitProperty = DependencyProperty.RegisterAttached("IsAutoFit", typeof(Boolean), typeof(RichTextBoxOptions), new PropertyMetadata(default(Boolean),OnSetIsAutoFitChanged));
        private static void OnSetIsAutoFitChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            OnSetIsAutoFitChanged(sender as TableColumn,(Boolean)e.NewValue);
            }

        #region M:GetDesiredWidth(Run):Double
        private static Double GetDesiredWidth(Run source) {
            var r = new FormattedText(
                source.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(
                    source.FontFamily,
                    source.FontStyle,
                    source.FontWeight,
                    source.FontStretch),
                    source.FontSize,
                Brushes.Black,
                null,
                TextFormattingMode.Display).Width;
            return Math.Max(r, source.FontSize/2);
            }
        #endregion
        #region M:GetDesiredWidth(TextRange):Double
        private static Double GetDesiredWidth(TextRange source) {
            var r = 0.0;
            var fontweight = source.GetPropertyValue(TextElement.FontWeightProperty);
            var fontsize = source.GetPropertyValue(TextElement.FontSizeProperty);
            if (fontsize == DependencyProperty.UnsetValue) {
                for (var i = source.Start; (i != null) && (i.CompareTo(source.End) <= 0);i = i.GetNextContextPosition(LogicalDirection.Forward)) {
                    if (i.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd) {
                        if (i.Parent is Run run) {
                            r += GetDesiredWidth(run);
                            }
                        }
                    }
                }
            else
                {
                r = new FormattedText(
                    source.Text,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(
                        source.GetPropertyValue(TextElement.FontFamilyProperty) as FontFamily,
                        (FontStyle)source.GetPropertyValue(TextElement.FontStyleProperty),
                        (FontWeight)fontweight,
                        FontStretches.Normal),
                        (Double)fontsize,
                    Brushes.Black,
                    null,
                    TextFormattingMode.Display).Width;
                }
            return r;
            }
        #endregion

        private static void OnSetIsAutoFitChanged(TableColumn source,Boolean value) {
            if (source != null) {
                if (value) {
                    var host = source.Ancestors<RichTextBox>().FirstOrDefault();
                    if (host != null) {
                        var document = host.Document;
                        if (document != null) {
                            void Handler(Object sender, RoutedEventArgs args) {
                                document.Loaded -= Handler;
                                var Table = source.Ancestors<Table>().FirstOrDefault();
                                var ColumnIndex = -1;
                                for (var i = 0; i < Table.Columns.Count; i++) {
                                    if (ReferenceEquals(Table.Columns[i],source)) {
                                        ColumnIndex = i;
                                        break;
                                        }
                                    }
                                if (ColumnIndex >= 0) {
                                    var DesiredWidth = 0.0;
                                    foreach (var Rowgroup in Table.RowGroups) {
                                        foreach (var Row in Rowgroup.Rows) {
                                            var j = 0;
                                            for (var i = 0; i < Row.Cells.Count; i++) {
                                                var Cell = Row.Cells[i];
                                                if (ColumnIndex == j) {
                                                    if (Cell.ColumnSpan == 1) {
                                                        DesiredWidth = Math.Max(DesiredWidth,GetDesiredWidth(new TextRange(Cell.ContentStart,Cell.ContentEnd)) +
                                                            Cell.Padding.Left + Cell.Padding.Right
                                                             + Cell.BorderThickness.Left
                                                             + Cell.BorderThickness.Right
                                                            );
                                                        }
                                                    }
                                                j += Cell.ColumnSpan;
                                                }
                                            }
                                        }
                                    source.Width = new GridLength(DesiredWidth,GridUnitType.Pixel);
                                    }
                                };
                            document.Loaded += Handler;
                            }
                        }
                    }
                }
            }

        public static void SetIsAutoFit(DependencyObject element, Boolean value)
            {
            element.SetValue(IsAutoFitProperty,value);
            }

        public static Boolean GetIsAutoFit(DependencyObject element)
            {
            return (Boolean) element.GetValue(IsAutoFitProperty);
            }
        }
    }