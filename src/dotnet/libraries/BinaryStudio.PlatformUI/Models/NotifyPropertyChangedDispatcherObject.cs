using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace BinaryStudio.PlatformUI.Models
    {
    public abstract class NotifyPropertyChangedDispatcherObject : INotifyPropertyChanged, ICustomTypeDescriptor
        {
        private class CollectionChangedObject
            {
            private INotifyCollectionChanged source;
            private NotifyPropertyChangedDispatcherObject host;
            private String propertyname;

            public CollectionChangedObject(NotifyPropertyChangedDispatcherObject host, INotifyCollectionChanged source, String propertyname) {
                this.source = source;
                this.host = host;
                this.propertyname = propertyname;
                source.CollectionChanged += OnCollectionChanged;
                }

            private void OnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
                {
                host.OnPropertyChanged(propertyname);
                }

            ~CollectionChangedObject()
                {
                source.CollectionChanged -= OnCollectionChanged;
                }
            }

        [Browsable(false)] public Dispatcher Dispatcher { get; }
        [Browsable(false)] public Boolean InvokeRequired { get { return Dispatcher.Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId; }}

        #region P:IsLoaded:Boolean
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Boolean IsLoadedProperty;
        [Browsable(false)]
        public Boolean IsLoaded
            {
            get { return IsLoadedProperty; }
            private set { SetValue(ref IsLoadedProperty,value,nameof(IsLoaded)); }
            }
        #endregion
        #region P:IsSelected:Boolean
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Boolean IsSelectedProperty;
        [Browsable(false)]
        public Boolean IsSelected
            {
            get { return IsSelectedProperty; }
            set { SetValue(ref IsSelectedProperty,value,nameof(IsSelected)); }
            }
        #endregion
        #region P:IsExpanded:Boolean
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private Boolean IsExpandedProperty;
        [Browsable(false)]
        public Boolean IsExpanded
            {
            get { return IsExpandedProperty; }
            set { SetValue(ref IsExpandedProperty,value,nameof(IsExpanded)); }
            }
        #endregion

        #region M:OnPropertyChanged(String)
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event when the specified property has been changed.</summary>
        /// <param name="PropertyName">The property that has been changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] String PropertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) {
                var e = new PropertyChangedEventArgs(PropertyName);
                if (InvokeRequired) {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(()=>{
                        handler.Invoke(this, e);
                        }));
                    }
                else
                    {
                    handler.Invoke(this, e);
                    }
                }
            }
        #endregion
        #region M:SetValue<T>(ref T,T,String):Boolean
        protected Boolean SetValue<T>(ref T field, T value, [CallerMemberName] String PropertyName = null) {
            var r = true;
            var equatable = value as IEquatable<T>;
            if (equatable != null) {
                r = equatable.Equals(field);
                }
            else if (typeof(T).IsSubclassOf(typeof(Enum)))
                {
                r = Equals(value, field);
                }
            else
                {
                r = Equals(value, field);
                }
            if (!r)
                {
                RemoceCollectionChangedHandler(PropertyName, field as INotifyCollectionChanged);
                field = value;
                AddCollectionChangedHandler(PropertyName, field as INotifyCollectionChanged);
                OnPropertyChanged(PropertyName);
                }
            return !r;
            }
        #endregion
        #region M:AddCollectionChangedHandler(String,INotifyCollectionChanged)
        private void AddCollectionChangedHandler(String propertyname, INotifyCollectionChanged value) {
            if (value != null) {
                map[value] = new CollectionChangedObject(this, value, propertyname);
                }
            }
        #endregion
        #region M:RemoceCollectionChangedHandler(String,INotifyCollectionChanged)
        private void RemoceCollectionChangedHandler(String propertyname, INotifyCollectionChanged value) {
            if (value != null) {
                map.Remove(propertyname);
                }
            }
        #endregion

        protected NotifyPropertyChangedDispatcherObject()
            :this(Dispatcher.CurrentDispatcher)
            {
            }

        protected NotifyPropertyChangedDispatcherObject(Dispatcher dispatcher) {
            if (dispatcher == null) { throw new ArgumentNullException(nameof(dispatcher)); }
            Dispatcher = dispatcher;
            Task.Factory.StartNew(delegate
                {
                try
                    {
                    OnLoad();
                    }
                finally
                    {
                    IsLoaded = true;
                    OnLoadCompleted();
                    }
                });
            }

        private readonly IDictionary<Object, CollectionChangedObject> map = new Dictionary<Object, CollectionChangedObject>();

        #region M:ICustomTypeDescriptor.GetAttributes:AttributeCollection
        /// <summary>Returns a collection of custom attributes for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.ComponentModel.AttributeCollection"/> containing the attributes for this object.</returns>
        AttributeCollection ICustomTypeDescriptor.GetAttributes()
            {
            return new AttributeCollection(GetAttributes().ToArray());
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetClassName:String
        /// <summary>Returns the class name of this instance of a component.</summary>
        /// <returns>The class name of the object, or <see langword="null"/> if the class does not have a name.</returns>
        String ICustomTypeDescriptor.GetClassName()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetComponentName:String
        String ICustomTypeDescriptor.GetComponentName()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetConverter:TypeConverter
        /// <summary>Returns a type converter for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.TypeConverter"/> that is the converter for this object, or <see langword="null"/> if there is no <see cref="T:System.ComponentModel.TypeConverter"/> for this object.</returns>
        TypeConverter ICustomTypeDescriptor.GetConverter()
            {
            return TypeDescriptor.GetConverter(GetType());
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetDefaultEvent:EventDescriptor
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetDefaultProperty:PropertyDescriptor
        /// <summary>Returns the default property for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the default property for this object, or null if this object does not have properties.</returns>
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
            {
            return GetDefaultProperty();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetEditor(Type):Object
        /// <summary>Returns an editor of the specified type for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.Object"/> of the specified type that is the editor for this object, or null if the editor cannot be found.</returns>
        /// <param name="editorBaseType">A <see cref="T:System.Type"/> that represents the editor for this object.</param>
        public virtual Object GetEditor(Type editorBaseType)
            {
            return TypeDescriptor.GetEditor(GetType(),editorBaseType);
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetEvents):EventDescriptorCollection
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetEvents(Attribute[]):EventDescriptorCollection
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetProperties:PropertyDescriptorCollection
        /// <summary>Returns the properties for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties for this component instance.</returns>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() {
            return new PropertyDescriptorCollection(GetProperties().ToArray());
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetProperties(Attribute[]):PropertyDescriptorCollection
        /// <summary>Returns the properties for this instance of a component using the attribute array as a filter.</summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the filtered properties for this component instance.</returns>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes) {
            return new PropertyDescriptorCollection(GetProperties(attributes).ToArray());
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor):Object
        /// <summary>Returns an object that contains the property described by the specified property descriptor.</summary>
        /// <param name="descriptor">A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the property whose owner is to be found.</param>
        /// <returns>An <see cref="T:System.Object"/> that represents the owner of the specified property.</returns>
        Object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor descriptor)
            {
            return GetPropertyOwner(descriptor);
            }
        #endregion
        #region M:GetAttributes:IEnumerable<Attribute>
        /// <summary>Returns a collection of custom attributes for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.ComponentModel.AttributeCollection"/> containing the attributes for this object.</returns>
        protected virtual IEnumerable<Attribute> GetAttributes() {
            foreach (var attribute in TypeDescriptor.GetAttributes(GetType()).OfType<Attribute>()) {
                yield return attribute;
                }
            }
        #endregion
        #region M:GetProperties:IEnumerable<PropertyDescriptor>
        /// <summary>Returns the properties for this instance of a component.</summary>
        /// <returns>A collection that represents the properties for this component instance.</returns>
        protected virtual IEnumerable<PropertyDescriptor> GetProperties() {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(GetType())) {
                yield return descriptor;
                }
            }
        #endregion
        #region M:GetProperties(Attribute[]):IEnumerable<PropertyDescriptor>
        /// <summary>Returns the properties for this instance of a component using the attribute array as a filter.</summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        /// <returns>A collection that represents the filtered properties for this component instance.</returns>
        protected virtual IEnumerable<PropertyDescriptor> GetProperties(Attribute[] attributes) {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(GetType(), attributes)) {
                yield return descriptor;
                }
            }
        #endregion
        #region M:GetPropertyOwner(PropertyDescriptor):Object
        /// <summary>Returns an object that contains the property described by the specified property descriptor.</summary>
        /// <param name="descriptor">A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the property whose owner is to be found.</param>
        /// <returns>An <see cref="T:System.Object"/> that represents the owner of the specified property.</returns>
        protected virtual Object GetPropertyOwner(PropertyDescriptor descriptor) {
            return this;
            }
        #endregion
        #region M:GetConverter:TypeConverter
        /// <summary>Returns a type converter for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.TypeConverter"/> that is the converter for this object, or <see langword="null"/> if there is no <see cref="T:System.ComponentModel.TypeConverter"/> for this object.</returns>
        protected virtual TypeConverter GetConverter()
            {
            return TypeDescriptor.GetConverter(GetType());
            }
        #endregion
        #region M:GetDefaultProperty:PropertyDescriptor
        /// <summary>Returns the default property for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the default property for this object, or null if this object does not have properties.</returns>
        protected virtual PropertyDescriptor GetDefaultProperty()
            {
            return TypeDescriptor.GetDefaultProperty(GetType());
            }
        #endregion
        #region M:OnLoad
        protected virtual void OnLoad()
            {
            }
        #endregion
        #region M:OnLoadCompleted
        protected virtual void OnLoadCompleted()
            {
            }
        #endregion
        }

    public abstract class NotifyPropertyChangedDispatcherObject<T> : Model
        {
        #region P:Source:T
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private T SourceProperty;
        [Browsable(false)]
        public T Source
            {
            get { return SourceProperty; }
            protected set { SetValue(ref SourceProperty, value, nameof(Source)); }
            }
        #endregion

        protected NotifyPropertyChangedDispatcherObject(T source)
            {
            Source = source;
            }

        #region M:GetAttributes:IEnumerable<Attribute>
        /// <summary>Returns a collection of custom attributes for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.ComponentModel.AttributeCollection"/> containing the attributes for this object.</returns>
        protected override IEnumerable<Attribute> GetAttributes() {
            return (Source != null)
                ? TypeDescriptor.GetAttributes(Source).OfType<Attribute>()
                : TypeDescriptor.GetAttributes(GetType()).OfType<Attribute>();
            }
        #endregion
        #region M:GetDefaultProperty:PropertyDescriptor
        /// <summary>Returns the default property for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the default property for this object, or null if this object does not have properties.</returns>
        protected override PropertyDescriptor GetDefaultProperty()
            {
            return TypeDescriptor.GetDefaultProperty((Source != null) ? (Object)Source: GetType());
            }
        #endregion
        //#region M:GetProperties(Attribute[]):IEnumerable<PropertyDescriptor>
        ///// <summary>Returns the properties for this instance of a component using the attribute array as a filter.</summary>
        ///// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        ///// <returns>A collection that represents the filtered properties for this component instance.</returns>
        //protected override IEnumerable<PropertyDescriptor> GetProperties(Attribute[] attributes) {
        //    return (Source != null)
        //        ? TypeDescriptor.GetProperties(Source, attributes).OfType<PropertyDescriptor>()
        //        : base.GetProperties(attributes);
        //    }
        //#endregion
        //#region M:GetProperties():IEnumerable<PropertyDescriptor>
        ///// <summary>Returns the properties for this instance of a component.</summary>
        ///// <returns>A collection that represents the properties for this component instance.</returns>
        //protected override IEnumerable<PropertyDescriptor> GetProperties() {
        //    return (Source != null)
        //        ? TypeDescriptor.GetProperties(Source).OfType<PropertyDescriptor>()
        //        : base.GetProperties();
        //    }
        //#endregion
        //#region M:GetPropertyOwner(PropertyDescriptor):Object
        ///// <summary>Returns an object that contains the property described by the specified property descriptor.</summary>
        ///// <param name="descriptor">A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the property whose owner is to be found.</param>
        ///// <returns>An <see cref="T:System.Object"/> that represents the owner of the specified property.</returns>
        //protected override Object GetPropertyOwner(PropertyDescriptor descriptor)
        //    {
        //    return Source;
        //    }
        //#endregion
        #region M:GetEditor(Type):Object
        /// <summary>Returns an editor of the specified type for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.Object"/> of the specified type that is the editor for this object, or null if the editor cannot be found.</returns>
        /// <param name="editorBaseType">A <see cref="T:System.Type"/> that represents the editor for this object.</param>
        public override Object GetEditor(Type editorBaseType)
            {
            return TypeDescriptor.GetEditor((Source != null) ? (Object)Source : GetType(),editorBaseType);
            }
        #endregion
        #region M:GetConverter:TypeConverter
        /// <summary>Returns a type converter for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.TypeConverter"/> that is the converter for this object, or <see langword="null"/> if there is no <see cref="T:System.ComponentModel.TypeConverter"/> for this object.</returns>
        protected override TypeConverter GetConverter()
            {
            return TypeDescriptor.GetConverter(Source);
            }
        #endregion
        //#region M:ToString:String
        ///// <summary>Returns a string that represents the current object.</summary>
        ///// <returns>A string that represents the current object.</returns>
        ///// <filterpriority>2</filterpriority>
        //public override String ToString()
        //    {
        //    return (Source != null)
        //        ? Source.ToString()
        //        : base.ToString();
        //    }
        //#endregion
        }
    }