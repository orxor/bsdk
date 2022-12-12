using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using BinaryStudio.PlatformUI.Extensions.Transfer;

namespace BinaryStudio.PlatformUI.Documents
    {
    public class DocumentParagraphContent : Paragraph
        {
        #region P:IsContentApplied:Boolean
        private static readonly DependencyPropertyKey IsContentAppliedPropertyKey = DependencyProperty.RegisterReadOnly("IsContentApplied", typeof(Boolean), typeof(DocumentParagraphContent), new PropertyMetadata(default(Boolean)));
        public static readonly DependencyProperty IsContentAppliedProperty = IsContentAppliedPropertyKey.DependencyProperty;
        public Boolean IsContentApplied
            {
            get { return (Boolean)GetValue(IsContentAppliedProperty); }
            private set { SetValue(IsContentAppliedPropertyKey,value); }
            }
        #endregion
        #region P:Content:Object
        public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(DocumentParagraphContent), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnContentChanged));
        private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is DocumentParagraphContent source) {
                source.OnContentChanged();
                }
            }
        public Object Content
            {
            get { return GetValue(ContentControl.ContentProperty); }
            set { SetValue(ContentControl.ContentProperty,value); }
            }
        #endregion

        public DocumentParagraphContent()
            {
            Loaded += OnLoaded;
            }

        /// <summary>Returns a value that indicates whether serialization processes should serialize the value for the provided dependency property.</summary>
        /// <param name="dp">The identifier for the dependency property that should be serialized.</param>
        /// <returns><see langword="true"/> if the dependency property that is supplied should be value-serialized; otherwise, <see langword="false"/>.</returns>
        protected override Boolean ShouldSerializeProperty(DependencyProperty dp) {
            if (ReferenceEquals(dp, ContentProperty)) { return false; }
            if (ReferenceEquals(dp, IsContentAppliedProperty)) { return false; }
            return base.ShouldSerializeProperty(dp);
            }

        private void OnContentChanged() {
            if (State > 0) { return; }
            State = 1;
            Inlines.Clear();
            var Source = Content;
            if (Source != null) {
                if (TryFindResource(new DataTemplateKey(Source.GetType())) is DataTemplate ContentTemplate) {
                    var content = ContentTemplate.LoadContent();
                    if (content is Paragraph TemplatedContent) {
                        TransferFactory.CopyTo(this,TemplatedContent,DataContextProperty);
                        TransferFactory.GetFactory(TemplatedContent.GetType()).Transfer(TemplatedContent,this);
                        }
                    }
                else
                    {
                    if (Source is Paragraph Paragraph) {
                        TransferFactory.CopyTo(this,Paragraph,DataContextProperty);
                        TransferFactory.GetFactory(Paragraph.GetType()).Transfer(Paragraph,this);
                        }
                    else
                        {
                        Inlines.Add(new Run(Source.ToString()));
                        }
                    }
                }
            #if DEBUG3
            var range = new TextRange(ContentStart,ContentEnd);
            if (!range.IsEmpty) {
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
                }
            #endif
            IsContentApplied = true;
            State = 0;
            }

        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            //OnContentChanged();
            }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            if (ReferenceEquals(e.Property,DataContextProperty)) {
                //Debug.Print($"{e.Property.Name}:{e.NewValue}");
                var x = BindingOperations.GetBindingExpressionBase(this,ContentProperty);
                x?.UpdateTarget();
                }
            base.OnPropertyChanged(e);
            }

        private Int32 State;
        }
    }