using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI.Documents
    {
    public class TableBoundRow : TableRow
        {
        public static readonly DependencyProperty CellsSourceProperty = DependencyProperty.Register("CellsSource", typeof(IEnumerable), typeof(TableBoundRow), new PropertyMetadata(default(IEnumerable)));
        public IEnumerable CellsSource
            {
            get { return (IEnumerable)GetValue(CellsSourceProperty); }
            set { SetValue(CellsSourceProperty, value); }
            }

        public TableBoundRow()
            {
            Loaded += OnLoaded;
            }

        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            var cells = Cells.ToArray();
            var table = this.Ancestors<Table>().FirstOrDefault();
            Cells.Clear();
            var items = CellsSource;
            if (items != null) {
                foreach (var item in items) {
                    foreach (var source in cells) {
                        source.DataContext = item;
                        var target = CloneFactory.Clone(source);
                        target.DataContext = item;
                        Cells.Add(target);
                        }
                    }
                }
            if (RichTextBoxOptions.GetIsAutoFit(table)) {
                RichTextBoxOptions.AutoFitTable(table);
                }
            }
        }
    }