﻿using System;
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
        //internal Int32 CloneCount;
        #region P:Content:Object
        public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(DocumentSectionContent), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, OnContentChanged));
        public Object Content
            {
            get { return GetValue(ContentControl.ContentProperty); }
            set { SetValue(ContentControl.ContentProperty, value); }
            }
        #endregion
        #region P:IsContentApplied:Boolean
        private static readonly DependencyPropertyKey IsContentAppliedPropertyKey = DependencyProperty.RegisterReadOnly("IsContentApplied", typeof(Boolean), typeof(DocumentSectionContent), new PropertyMetadata(default(Boolean)));
        public static readonly DependencyProperty IsContentAppliedProperty = IsContentAppliedPropertyKey.DependencyProperty;
        public Boolean IsContentApplied
            {
            get { return (Boolean)GetValue(IsContentAppliedProperty); }
            private set { SetValue(IsContentAppliedPropertyKey,value); }
            }
        #endregion

        protected override Boolean ShouldSerializeProperty(DependencyProperty dp)
            {
            if (ReferenceEquals(dp,ContentProperty)) { return false; }
            return base.ShouldSerializeProperty(dp);
            }

        private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is DocumentSectionContent source) {
                source.OnContentChanged();
                }
            }

        private void OnContentChanged() {
            if (State > 0) { return; }
            State = 1;
            try
                {
                Blocks.Clear();
                var Source = Content;
                if (Source != null) {
                    if (TryFindResource(new DataTemplateKey(Source.GetType())) is DataTemplate ContentTemplate) {
                        var content = ContentTemplate.LoadContent();
                        if (content is Section TemplatedContent) {
                            CloneFactory.CopyTo(this,TemplatedContent,DataContextProperty);
                            CloneFactory.CopyTo(TemplatedContent,this,this);
                            }
                        }
                    }
                #if DEBUG2
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
                }
            finally
                {
                IsContentApplied = true;
                State = 0;
                }
            }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            if (ReferenceEquals(e.Property,DataContextProperty)) {
                BindingOperations.GetBindingExpressionBase(this,ContentProperty)?.UpdateTarget();
                }
            base.OnPropertyChanged(e);
            }

        public DocumentSectionContent()
            {
            //this.SetBinding(ContentProperty,this,DataContextProperty,BindingMode.OneWay);
            Loaded += OnLoaded;
            }

        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            //OnContentChanged();
            }

        private Int32 State;
        }
    }