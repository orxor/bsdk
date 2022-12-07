using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using BinaryStudio.PlatformUI.Controls;
using TriggerActionCollection = System.Windows.TriggerActionCollection;
using UDataTrigger=System.Windows.DataTrigger;

namespace BinaryStudio.PlatformUI
    {
    [ContentProperty("Setters")]
    public class DataTrigger : TriggerBase<DependencyObject>
        {
        #region E:DataTrigger.AttachedEvent:RoutedEvent
        public static readonly RoutedEvent AttachedEvent = EventManager.RegisterRoutedEvent("Attached", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataTrigger));
        public static void AddAttachedHandler(DependencyObject source, RoutedEventHandler handler) {
            if (source is UIElement e) {
                e.AddHandler(AttachedEvent,handler);
                }
            }
        public static void RemoveAttachedHandler(DependencyObject source, RoutedEventHandler handler) {
            if (source is UIElement e) {
                e.RemoveHandler(AttachedEvent,handler);
                }
            }
        #endregion

        public DataTrigger() {
            EnterActions = new TriggerActionCollection();
            ExitActions  = new TriggerActionCollection();
            EnterSetters = Setters;
            ExitSetters = new SetterBaseCollection();
            }

        public BindingExpressionBase BindingExpression { get;private set; }

        #region P:Binding:BindingBase
        private BindingBase binding;
        public BindingBase Binding {
            get
                {
                VerifyAccess();
                return binding;
                }
            set
                {
                VerifyAccess();
                if (IsSealed) { throw new InvalidOperationException(); }
                binding = value;
                if (binding != null) {
                    BindingExpression = BindingOperations.SetBinding(this, TriggerValueProperty,binding);
                    }
                }
            }
        #endregion
        #region P:Value:Object
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Object), typeof(DataTrigger), new PropertyMetadata(default(Object)));
        public Object Value {
            get { return (Object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
            }
        #endregion
        #region P:Setters:SetterBaseCollection
        private SetterBaseCollection setters;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SetterBaseCollection Setters { get {
            VerifyAccess();
            return setters ?? (setters = new SetterBaseCollection());
            }}
        #endregion
        #region P:TriggerValue:Object
        internal static readonly DependencyProperty TriggerValueProperty = DependencyProperty.Register("TriggerValue", typeof(Object), typeof(DataTrigger), new PropertyMetadata(default(Object), OnTriggerValueChanged));
        private static void OnTriggerValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is DataTrigger source) {
                source.OnTriggerValueChanged();
                }
            }

        #region M:Equals(Object,Object):Boolean
        private new static Boolean Equals(Object x, Object y) {
            if (ReferenceEquals(x,y)) { return true; }
            if ((x == null) || (y == null)) { return false; }
            if (x.GetType() == y.GetType()) { return Object.Equals(x,y); }
            if ((x is String) && (y.GetType().IsValueType)) {
                var converter = TypeDescriptor.GetConverter(y);
                if (converter != null) {
                    if (converter.CanConvertFrom(typeof(String))) {
                        return Object.Equals(y, converter.ConvertFromString((String)x));
                        }
                    }
                }
            if ((y is String) && (x.GetType().IsValueType)) {
                var converter = TypeDescriptor.GetConverter(x);
                if (converter != null) {
                    if (converter.CanConvertFrom(typeof(String))) {
                        return Object.Equals(x, converter.ConvertFromString((String)y));
                        }
                    }
                }
            return Object.Equals(x,y);
            }
        #endregion
        #region M:InvokeSetters(DependencyObject,SetterBaseCollection)
        private static void InvokeSetters(DependencyObject source, IEnumerable setters) {
            foreach (var setter in setters.OfType<Setter>()) {
                var resolver = NameResolver.Create(source,setter.TargetName);
                var o = resolver.Object;
                if (o != null) {
                    if (setter.Property != null) {
                        if (setter.Value is DynamicResourceExtension DynamicResourceExtension) {
                            if (source is FrameworkContentElement FrameworkContentElement) {
                                var r = FrameworkContentElement.TryFindResource(DynamicResourceExtension.ResourceKey);
                                if (r != null) {
                                    o.SetValue(setter.Property, r);
                                    continue;
                                    }
                                }
                            continue;
                            }
                        o.SetValue(setter.Property, setter.Value);
                        }
                    }
                }
            }
        #endregion

        private void PlayActions(FrameworkElement source, TriggerActionCollection actions) {
            if (source != null) {
                foreach (var action in actions) {
                    if (action is BeginStoryboard) {
                        var storyboard = ((BeginStoryboard)action).Storyboard;
                        if (storyboard != null) {
                            if (Scope != null) {
                                NameScope.SetNameScope(storyboard, Scope);
                                NameScope.SetNameScope(action, Scope);
                                }
                            storyboard.Begin(source);
                            }
                        }
                    }
                }
            }

        #region M:OnEnter(FrameworkElement)
        private void OnEnter(FrameworkElement source) {
            if (source != null) {
                if (source.IsLoaded) {
                    InvokeSetters(source, EnterSetters);
                    PlayActions(source, EnterActions);
                    }
                else
                    {
                    source.DoAfterLoaded(()=> {
                        InvokeSetters(source, EnterSetters);
                        PlayActions(source, EnterActions);
                        });
                    }
                }
            }
        #endregion
        #region M:OnEnter(FrameworkContentElement)
        private void OnEnter(FrameworkContentElement source) {
            if (source != null) {
                InvokeSetters(source, EnterSetters);
                }
            }
        #endregion
        #region M:OnExit(FrameworkElement)
        private void OnExit(FrameworkElement source) {
            if (source != null) {
                if (source.IsLoaded) {
                    PlayActions(source, ExitActions);
                    InvokeSetters(source, ExitSetters);
                    }
                else
                    {
                    source.DoAfterLoaded(()=> {
                        PlayActions(source, ExitActions);
                        InvokeSetters(source, ExitSetters);
                        });
                    }
                }
            }
        #endregion
        #region M:OnExit(FrameworkContentElement)
        private void OnExit(FrameworkContentElement source) {
            if (source != null) {
                if (source.IsLoaded) {
                    InvokeSetters(source, ExitSetters);
                    }
                else
                    {
                    source.DoAfterLoaded(()=> {
                        InvokeSetters(source, ExitSetters);
                        });
                    }
                }
            }
        #endregion

        private void OnTriggerValueChanged() {
            #if DEBUG_TRIGGER
            Debug.Print(@"DataTrigger:OnTriggerValueChanged");
            #endif
            if (Equals(TriggerValue, Value)) {
                OnEnter(AssociatedObject as FrameworkElement);
                OnEnter(AssociatedObject as FrameworkContentElement);
                TriggerActivated?.Invoke(this, new RoutedEventArgs(TriggerActivatedEvent));
                }
            else
                {
                OnExit(AssociatedObject as FrameworkElement);
                }
            }

        internal Object TriggerValue {
            get { return (Object)GetValue(TriggerValueProperty); }
            set { SetValue(TriggerValueProperty, value); }
            }
        #endregion

        public TriggerActionCollection EnterActions {get; }
        public TriggerActionCollection ExitActions {get; }
        public SetterBaseCollection EnterSetters {get; }
        public SetterBaseCollection ExitSetters {get; }
        private INameScope Scope {get;set; }

        #region M:OnAttached
        protected override void OnAttached() {
            base.OnAttached();
            #if DEBUG_TRIGGER
            Debug.Print(@"DataTrigger:OnAttached");
            #endif
            var o = AssociatedObject;
            (o as FrameworkElement       ).DoAfterLoaded(()=>{ Scope = FindScope(o); });
            (o as FrameworkContentElement).DoAfterLoaded(()=>{ Scope = FindScope(o); });
            (o as FrameworkContentElement).OnDataContextChanged(()=>{
                var e = BindingExpression;
                if (e != null) {
                    e.UpdateTarget();
                    }
                return;
                });
            OnTriggerValueChanged();
            }
        #endregion

        #region M:Clone(UDataTrigger):DataTrigger
        internal static DataTrigger Clone(UDataTrigger source) {
            if (source != null) {
                var r = new DataTrigger{
                    Binding = source.Binding,
                    Value = source.Value
                    };
                foreach (var setter in source.Setters.OfType<Setter>()) {
                    r.Setters.Add(Clone(setter));
                    }
                return r;
                }
            return null;
            }
        #endregion
        #region M:Clone(Setter):Setter
        private static Setter Clone(Setter Source) {
            return (Source != null)
                ? new Setter(Source.Property,Source.Value,Source.TargetName)
                : null;
            }
        #endregion
        #region M:Clone(SetterBase):SetterBase
        private static SetterBase Clone(SetterBase Source) {
            if (Source != null) {
                if (Source is Setter Setter) {
                    return Clone(Setter);
                    }
                throw new NotSupportedException();
                }
            return null;
            }
        #endregion
        #region M:Attach(IList<DataTrigger>,DependencyObject)
        internal static void Attach(IList<DataTrigger> triggers, DependencyObject source) {
            if (triggers != null) {
                foreach (var trigger in triggers) {
                    //trigger.Attach(source);
                    }
                }
            }
        #endregion
        #region M:CopyTo(SetterBaseCollection,SetterBaseCollection)
        private static void CopyTo(SetterBaseCollection Source,SetterBaseCollection Target) {
            if ((Source != null) && (Target != null)) {
                foreach (var SourceSetter in Source) {
                    Target.Add(Clone(SourceSetter));
                    }
                }
            }
        #endregion

        /// <summary>Makes the instance a clone (deep copy) of the specified <see cref="T:System.Windows.Freezable"/> using base (non-animated) property values.</summary>
        /// <param name="Source">The object to clone.</param>
        protected override void CloneCore(Freezable Source) {
            if (Source is DataTrigger Trigger) {
                Value   = Trigger.Value;
                Binding = Trigger.Binding;
                CopyTo(Trigger.EnterSetters,EnterSetters);
                CopyTo(Trigger.ExitSetters,ExitSetters);
                CopyTo(Trigger.Setters,Setters);
                }
            }

        #if DEBUG_TRIGGER
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            base.OnPropertyChanged(e);
            Debug.Print(@"DataTrigger:OnPropertyChanged:Scope:""{0}""",Scope);
            Debug.Print(@"DataTrigger:OnPropertyChanged:{0}:{1}->{2}", e.Property.Name,
                (e.OldValue != null)
                    ? String.Format(@"""{0}""", e.OldValue)
                    : "(null)",
                (e.NewValue != null)
                    ? String.Format(@"""{0}""", e.NewValue)
                    : "(null)"
                );
            }
        #endif

        public static readonly RoutedEvent TriggerActivatedEvent = EventManager.RegisterRoutedEvent("TriggerActivated",RoutingStrategy.Bubble,typeof(RoutedEventHandler),typeof(DataTrigger));
        internal FrameworkElement NameScopeReferenceElement;

        public event RoutedEventHandler TriggerActivated;

        #region M:FindScope(FrameworkContentElement):INameScope
        internal static INameScope FindScope(FrameworkContentElement source) {
            if (source == null) { return null; }
            return null;
            }
        #endregion

        internal static INameScope FindScope(DependencyObject d) {
            return (INameScope)(typeof(FrameworkElement).
                GetMethod("FindScope", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(DependencyObject) },null).
                Invoke(null, new Object[] { d }));
            }

        //#region M:CreateCollection:TriggerActionCollection
        //private static TriggerActionCollection CreateCollection() {
        //    var ctor = typeof(TriggerActionCollection).GetConstructor(BindingFlags.NonPublic |BindingFlags.Instance, null, Type.EmptyTypes, null);
        //    return (TriggerActionCollection)ctor.Invoke(new Object[0]);
        //    }
        //#endregion
        //private static IDictionary<DataTrigger,DependencyObject> 
        }
    }