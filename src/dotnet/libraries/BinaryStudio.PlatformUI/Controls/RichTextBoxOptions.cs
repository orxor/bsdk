using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Documents;
using BinaryStudio.PlatformUI.Extensions.Transfer;

namespace BinaryStudio.PlatformUI.Controls
    {
    public class RichTextBoxOptions : TextProperties
        {
        #region P:RichTextBoxOptions.Document:FlowDocument
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.RegisterAttached("Document", typeof(FlowDocument), typeof(RichTextBoxOptions), new PropertyMetadata(default(FlowDocument),OnDocumentChanged));
        private static void OnDocumentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is RichTextBox source) {
                try
                    {
                    var document = (FlowDocument)e.NewValue;
                    if (document != null) {
                        if (document.IsInitialized) {
                            source.Document = (FlowDocument)Activator.CreateInstance(document.GetType());
                            TransferFactory.CopyTo(source,document,Control.ForegroundProperty);
                            TransferFactory.CopyTo(source,document,Control.FontFamilyProperty);
                            TransferFactory.CopyTo(source,document,Control.FontSizeProperty);
                            TransferFactory.CopyTo(source,document,Control.FontStretchProperty);
                            TransferFactory.CopyTo(source,document,Control.FontStyleProperty);
                            TransferFactory.CopyTo(source,document,Control.FontWeightProperty);
                            TransferFactory.CopyTo(source,document,FrameworkElement.DataContextProperty);
                            TransferFactory.GetFactory(document.GetType()).CopyTo(document,source.Document);
                            }
                        else
                            {
                            document.DoAfterInitialization(()=>{
                                source.Document = (FlowDocument)Activator.CreateInstance(document.GetType());
                                TransferFactory.CopyTo(source,document,Control.ForegroundProperty);
                                TransferFactory.CopyTo(source,document,Control.FontFamilyProperty);
                                TransferFactory.CopyTo(source,document,Control.FontSizeProperty);
                                TransferFactory.CopyTo(source,document,Control.FontStretchProperty);
                                TransferFactory.CopyTo(source,document,Control.FontStyleProperty);
                                TransferFactory.CopyTo(source,document,Control.FontWeightProperty);
                                TransferFactory.CopyTo(source,document,FrameworkElement.DataContextProperty);
                                TransferFactory.GetFactory(document.GetType()).CopyTo(document,source.Document);
                                });
                            }
                        }
                    }
                catch (Exception x)
                    {
                    Debug.Print(Exceptions.ToString(x));
                    }
                }
            }

        public static void SetDocument(DependencyObject element, FlowDocument value)
            {
            element.SetValue(DocumentProperty, value);
            }

        public static FlowDocument GetDocument(DependencyObject element)
            {
            return (FlowDocument) element.GetValue(DocumentProperty);
            }
        #endregion

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