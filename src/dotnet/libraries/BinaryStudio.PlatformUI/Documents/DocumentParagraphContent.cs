using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI.Documents
    {
    public class DocumentParagraphContent : Paragraph
        {
        #region P:Content:Object
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(Object), typeof(DocumentParagraphContent), new PropertyMetadata(default(Object)));
        public Object Content
            {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
            }
        #endregion
        #region P:IsContentApplied:Boolean
        private static readonly DependencyPropertyKey IsContentAppliedPropertyKey = DependencyProperty.RegisterReadOnly("IsContentApplied", typeof(Boolean), typeof(DocumentParagraphContent), new PropertyMetadata(default(Boolean)));
        public static readonly DependencyProperty IsContentAppliedProperty = IsContentAppliedPropertyKey.DependencyProperty;
        public Boolean IsContentApplied
            {
            get { return (Boolean)GetValue(IsContentAppliedProperty); }
            private set { SetValue(IsContentAppliedPropertyKey,value); }
            }
        #endregion

        public DocumentParagraphContent()
            {
            Loaded += OnLoaded;
            }

        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            Inlines.Clear();
            var Source = Content;
            if (Source != null) {
                if (TryFindResource(new DataTemplateKey(Source.GetType())) is DataTemplate ContentTemplate) {
                    if (ContentTemplate.LoadContent() is Paragraph TemplatedContent) {
                        TemplatedContent.DataContext = Source;
                        CloneFactory.CopyTo(TemplatedContent,this,this);
                        DataContext = Source;
                        }
                    }
                }
            #if DEBUG
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
                #endif
                }
            IsContentApplied = true;
            }
        }
    }