using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Xml;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI.Documents
    {
    public class DocumentSectionContent : Section
        {
        internal Int32 CloneCount;
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(Object), typeof(DocumentSectionContent), new PropertyMetadata(default(Object)));
        public Object Content
            {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
            }

        private static readonly DependencyPropertyKey IsContentAppliedPropertyKey = DependencyProperty.RegisterReadOnly("IsContentApplied", typeof(Boolean), typeof(DocumentSectionContent), new PropertyMetadata(default(Boolean)));
        public static readonly DependencyProperty IsContentAppliedProperty = IsContentAppliedPropertyKey.DependencyProperty;

        public Boolean IsContentApplied
            {
            get { return (Boolean)GetValue(IsContentAppliedProperty); }
            private set { SetValue(IsContentAppliedPropertyKey,value); }
            }

        public DocumentSectionContent()
            {
            Loaded += OnLoaded;
            Initialized += OnInitialized;
            SourceUpdated += OnSourceUpdated;
            TargetUpdated += OnTargetUpdated;
            }

        private void OnTargetUpdated(Object sender, DataTransferEventArgs e)
            {
            Debug.Print("DocumentSectionContent.OnTargetUpdated");
            }

        private void OnSourceUpdated(Object sender, DataTransferEventArgs e)
            {
            Debug.Print("DocumentSectionContent.OnSourceUpdated");
            }

        private void OnInitialized(Object sender, EventArgs e)
            {
            Debug.Print("DocumentSectionContent.OnInitialized");
            }

        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Debug.Print("DocumentSectionContent.OnLoaded");
            Loaded -= OnLoaded;
            Blocks.Clear();
            var Source = Content;
            if (Source != null) {
                if (TryFindResource(new DataTemplateKey(Source.GetType())) is DataTemplate ContentTemplate) {
                    if (ContentTemplate.LoadContent() is Section TemplatedContent) {
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
                var host = this.Ancestors<FlowDocument>().FirstOrDefault();
                if (host != null) {
                    
                    }
                }
            IsContentApplied = true;
            }
        }
    }