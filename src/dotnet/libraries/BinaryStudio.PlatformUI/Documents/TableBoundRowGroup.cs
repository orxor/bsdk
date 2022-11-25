using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Extensions;

// ReSharper disable LocalVariableHidesMember

namespace BinaryStudio.PlatformUI.Documents
    {
    public class TableBoundRowGroup : TableRowGroup
        {
        public static readonly DependencyProperty RowsSourceProperty = DependencyProperty.Register("RowsSource", typeof(IEnumerable), typeof(TableBoundRowGroup), new PropertyMetadata(default(IEnumerable)));
        public IEnumerable RowsSource
            {
            get { return (IEnumerable)GetValue(RowsSourceProperty); }
            set { SetValue(RowsSourceProperty, value); }
            }

        public TableBoundRowGroup()
            {
            Loaded += OnLoaded;
            }

        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            var rows = Rows.ToArray();
            var Table = this.Ancestors<Table>().FirstOrDefault();
            Rows.Clear();
            var Foreground = this.GetForeground();
            var items = RowsSource;
            if (items != null) {
                foreach (var item in items) {
                    foreach (var row in rows) {
                        row.DataContext = item;
                        var target = CloneFactory.Clone(row,this);
                        if (target.IsDefaultValue(ForegroundProperty)) {
                            target.SetForegroundToSelfAndDescendants(Foreground);
                            }
                        target.DataContext = item;
                        Rows.Add(target);
                        AddLogicalChild(target);
                        }
                    }
                }
            if (RichTextBoxOptions.GetIsAutoFit(Table)) {
                RichTextBoxOptions.AutoFitTable(Table);
                }
            #if DEBUG
            var range = new TextRange(ContentStart,ContentEnd);
            using (var memory = new MemoryStream()) {
                range.Save(memory,DataFormats.Xaml);
                var builder = new StringBuilder();
                using (var writer = XmlWriter.Create(new StringWriter(builder),new XmlWriterSettings{
                    Indent = true,
                    IndentChars = "  "
                    })) {
                    using (var reader = XmlReader.Create(new StreamReader(new MemoryStream(memory.ToArray())))) {
                        writer.WriteNode(reader,false);
                        }
                    }
                Debug.Print(builder.ToString());
                }
            #endif
            }
        }
    }