using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using BinaryStudio.PlatformUI.Documents;

namespace BinaryStudio.PlatformUI.Controls
    {
    public class RichTextBoxOptions : DocumentProperties
        { 
        public static readonly DependencyProperty IsAutoFitProperty = DependencyProperty.RegisterAttached("IsAutoFit", typeof(Boolean), typeof(RichTextBoxOptions), new PropertyMetadata(default(Boolean),OnSetIsAutoFitChanged));
        private static void OnSetIsAutoFitChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            OnSetIsAutoFitChanged(sender as Table,(Boolean)e.NewValue);
            }

        internal static void AutoFitTable(Table target) {
            var DesiredWidth = new Double[target.Columns.Count];
            foreach (var RowGroup in target.RowGroups) {
                foreach (var Row in RowGroup.Rows) {
                    var j = 0;
                    for (var i = 0; i < Row.Cells.Count; i++) {
                        var Cell = Row.Cells[i];
                        if (Cell.ColumnSpan == 1) {
                            DesiredWidth[j] = Math.Max(DesiredWidth[j],GetDesiredSize(Cell).X +
                                Cell.Padding.Left + Cell.Padding.Right
                                 + Cell.BorderThickness.Left
                                 + Cell.BorderThickness.Right
                                );
                            }
                        j += Cell.ColumnSpan;
                        }
                    }
                }
            for (var i = 0; i < DesiredWidth.Length;i++) {
                if (GetIsAutoFit(target.Columns[i])) {
                    target.Columns[i].Width = new GridLength(DesiredWidth[i],GridUnitType.Pixel);
                    SetIsAutoFit(target.Columns[i], true);
                    }
                }
            }

        private static void OnSetIsAutoFitChanged(Table source,Boolean value) {
            if (source != null) {
                if (value) {
                    var host = source.Ancestors<RichTextBox>().FirstOrDefault();
                    if (host != null) {
                        var document = host.Document;
                        if (document != null) {
                            if (!document.IsLoaded) {
                                void Handler(Object sender, RoutedEventArgs args) {
                                    document.Loaded -= Handler;
                                    AutoFitTable(source);
                                    Debug.Print("FlowDocumentLoaded");
                                    };
                                document.Loaded += Handler;
                                }
                            else
                                {
                                AutoFitTable(source);
                                }
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