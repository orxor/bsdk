using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Extensions;

// ReSharper disable LocalVariableHidesMember

namespace BinaryStudio.PlatformUI.Documents
    {
    public class TableBoundRowGroup : TableRowGroup
        {
        #region P:ItemsSource:IEnumerable
        public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(typeof(TableBoundRowGroup), new PropertyMetadata(default(IEnumerable),OnItemsSourceChanged));
        private static void OnItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is TableBoundRowGroup source) {
                source.OnItemsSourceChanged();
                }
            }

        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty,value); }
            }
        #endregion

        public TableBoundRowGroup()
            {
            Loaded += OnLoaded;
            }

        private void OnItemsSourceChanged() {
            if (State > 0) { return; }
            State = 1;
            var rows = Rows.ToArray();
            var Table = this.Ancestors<Table>().FirstOrDefault();
            Rows.Clear();
            var Foreground = this.GetForeground();
            var items = ItemsSource;
            if (items != null) {
                foreach (var item in items) {
                    foreach (var SourceRow in rows) {
                        var TargetRow = (TableRow)Activator.CreateInstance(SourceRow.GetType());
                        Rows.Add(TargetRow);
                        TargetRow.SetValue(DataContextProperty,null);
                        SourceRow.SetValue(DataContextProperty,null);
                        CloneFactory.CopyTo(SourceRow,TargetRow,this);
                        TargetRow.SetValue(DataContextProperty,item);
                        CloneFactory.ApplyBindings(TargetRow);
                        if (TargetRow.IsDefaultValue(ForegroundProperty)) {
                            TargetRow.SetForegroundToSelfAndDescendants(Foreground);
                            }
                        }
                    }
                }
            if (DocumentProperties.GetIsAutoSize(Table)) {
                DocumentProperties.DoAutoSize(Table);
                }
            #if DEBUG2
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
            State = 0;
            }

        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            OnItemsSourceChanged();
            }

        private Int32 State;
        }
    }