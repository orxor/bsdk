using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Threading;
using System.Xml;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Extensions;
using BinaryStudio.PlatformUI.Extensions.Transfer;

namespace BinaryStudio.PlatformUI.Documents
    {
    using TriggerBase = System.Windows.Interactivity.TriggerBase;
    using UDataTrigger=System.Windows.DataTrigger;
    public class DocumentSectionContent : Section
        {
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

        #region M:ShouldSerializeProperty(DependencyProperty):Boolean
        /// <summary>Returns a value that indicates whether serialization processes should serialize the value for the provided dependency property.</summary>
        /// <param name="dp">The identifier for the dependency property that should be serialized.</param>
        /// <returns><see langword="true"/> if the dependency property that is supplied should be value-serialized; otherwise, <see langword="false"/>.</returns>
        protected override Boolean ShouldSerializeProperty(DependencyProperty dp)
            {
            if (ReferenceEquals(dp,ContentProperty)) { return false; }
            return base.ShouldSerializeProperty(dp);
            }
        #endregion
        #region M:OnContentChanged(DependencyObject,DependencyPropertyChangedEventArgs)
        private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is DocumentSectionContent source) {
                source.OnContentChanged();
                }
            }
        #endregion
        #region M:OnContentChanged
        private void OnContentChanged() {
            if (State > 0) { return; }
            using (new DebugScope()) {
                State = 1;
                try
                    {
                    var Source = Content;
                    Debug.Print($@"DocumentSectionContent:{{{Diagnostics.GetKey(this)}}}:{{{DataContext}}}:OnContentChanged:{{{Source}}}");
                    Blocks.Clear();
                    if (Source != null) {
                        var template = TryFindDataTemplate(Source.GetType());
                        if (template != null) {
                            var content = template.LoadContent();
                            if (content is Section TemplatedContent) {
                                TransferFactory.CopyTo(this,TemplatedContent,DataContextProperty);
                                TransferFactory.GetFactory(TemplatedContent.GetType()).CopyTo(TemplatedContent,this);
                                TemplatedContent.DataContext = null;
                                var triggers = this.LogicalDescendants().SelectMany(Interaction.GetTriggers).OfType<DataTrigger>().ToArray();
                                var expressions = triggers.Select(i => i.BindingExpression).Where(i => i != null).ToArray();
                                if (expressions.Length > 0) {
                                    if (expressions.Any(i => i.Status != BindingStatus.Active)) {
                                        var task = Task.Factory.StartNew(()=>{
                                            while(true) {
                                                if ((Boolean)Dispatcher.Invoke(DispatcherPriority.DataBind,new Func<Boolean>(()=>{
                                                    var r = expressions.All(i => i.Status == BindingStatus.Active);
                                                    return r;
                                                    })))
                                                    {
                                                    break;
                                                    }
                                                Thread.Yield();
                                                }
                                            });
                                        task.Wait();
                                        }
                                    Debug.Print($"{{{Source}}}:4");
                                    }
                                foreach (var trigger in triggers) {
                                    trigger.Detach();
                                    }
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
            }
        #endregion
        #region M:OnPropertyChanged(DependencyPropertyChangedEventArgs)
        /// <summary>Handles notifications that one or more of the dependency properties that exist on the element have had their effective values changed.</summary>
        /// <param name="e">Arguments associated with the property value change. The <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.Property"/> property specifies which property has changed, the <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.OldValue"/> property specifies the previous property value, and the <see cref="P:System.Windows.DependencyPropertyChangedEventArgs.NewValue"/> property specifies the new property value.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            if (ReferenceEquals(e.Property,DataContextProperty)) {
                Debug.Print($@"{GetType().Name}.OnPropertyChanged[""{Name}"":{{{DataContext}}}].{e.Property.Name}");
                BindingOperations.GetBindingExpressionBase(this,ContentProperty)?.UpdateTarget();
                }
            base.OnPropertyChanged(e);
            }
        #endregion
        #region M:OnLoaded(Object,RoutedEventArgs)
        private void OnLoaded(Object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            }
        #endregion
        #region M:TryFindDataTemplate(Type):DataTemplate
        private DataTemplate TryFindDataTemplate(Type source) {
            if (source == null) { return null; }
            while ((source != null) && (source != typeof(Object))) {
                if (TryFindResource(new DataTemplateKey(source)) is DataTemplate r) { return r; }
                source = source.BaseType;
                }
            return null;
            }
        #endregion

        public DocumentSectionContent()
            {
            Loaded += OnLoaded;
            }

        private Int32 State;
        }
    }