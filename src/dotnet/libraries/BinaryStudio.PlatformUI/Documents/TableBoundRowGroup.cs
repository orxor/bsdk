using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Extensions.Transfer;

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

        /// <summary>Returns a value that indicates whether serialization processes should serialize the value for the provided dependency property.</summary>
        /// <param name="dp">The identifier for the dependency property that should be serialized.</param>
        /// <returns> <see langword="true"/> if the dependency property that is supplied should be value-serialized; otherwise, <see langword="false"/>.</returns>
        protected override Boolean ShouldSerializeProperty(DependencyProperty dp) {
            if (ReferenceEquals(dp,ItemsSourceProperty)) { return false; }
            return base.ShouldSerializeProperty(dp);
            }

        #region M:OnPropertyChanged(DependencyPropertyChangedEventArgs)
        /// <summary>Handles notifications that one or more of the dependency properties that exist on the element have had their effective values changed.</summary>
        /// <param name="e">Arguments associated with the property value change. The <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.Property"/> property specifies which property has changed, the <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.OldValue"/> property specifies the previous property value, and the <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.NewValue"/> property specifies the new property value.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            if (ReferenceEquals(e.Property,DataContextProperty)) {
                BindingOperations.GetBindingExpressionBase(this,ItemsSourceProperty)?.UpdateTarget();
                }
            base.OnPropertyChanged(e);
            }
        #endregion
        #region M:OnItemsSourceChanged
        private void OnItemsSourceChanged() {
            if (State > 0) { return; }
            using (new DebugScope()) {
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
                            TargetRow.ClearValue(DataContextProperty);
                            SourceRow.ClearValue(DataContextProperty);
                            TransferFactory.GetFactory(SourceRow.GetType()).Transfer(SourceRow,TargetRow);
                            TransferFactory.TransferDataContext(TargetRow,item);
                            if (TargetRow.IsDefaultValue(ForegroundProperty)) {
                                TargetRow.SetForegroundToSelfAndDescendants(Foreground);
                                }
                            }
                        }
                    }
                if (TextProperties.GetIsAutoSize(Table)) {
                    TextProperties.DoAutoSize(Table);
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
            }
        #endregion

        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            OnItemsSourceChanged();
            }

        private Int32 State;
        }
    }