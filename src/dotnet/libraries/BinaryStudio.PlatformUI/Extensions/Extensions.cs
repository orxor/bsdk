using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BinaryStudio.PlatformUI.Controls
    {
    public static partial class Extensions
        {
        #region M:DoAfterLayoutUpdated({this}UIElement,Action)
        public static void DoAfterLayoutUpdated(this UIElement source,Action predicate) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            void Handler(Object sender, EventArgs e) {
                predicate.Invoke();
                source.LayoutUpdated -= Handler;
                }
            source.LayoutUpdated += Handler;
            }
        #endregion
        #region M:DoAfterLayoutUpdated<T>({this}T,Action<T>)
        public static void DoAfterLayoutUpdated<T>(this T source,Action<T> predicate)
            where T: UIElement {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            EventHandler handler = null;
            handler = delegate {
                predicate.Invoke(source);
                source.LayoutUpdated -= handler;
                };
            source.LayoutUpdated += handler;
            }
        #endregion
        //#region M:DoAfterLoaded<T>({this}T,Action<T>)
        //public static void DoAfterLoaded<T>(this T source,Action<T> predicate)
        //    where T: FrameworkElement
        //    {
        //    if (source == null) { throw new ArgumentNullException(nameof(source)); }
        //    if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
        //    RoutedEventHandler handler = null;
        //    handler = delegate {
        //        predicate.Invoke(source);
        //        source.Loaded -= handler;
        //        };
        //    source.Loaded += handler;
        //    }
        //#endregion
        #region M:DoAfterLoaded({this}FrameworkElement,Action)
        public static void DoAfterLoaded(this FrameworkElement source,Action predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                void Handler(Object sender, RoutedEventArgs e) {
                    predicate.Invoke();
                    source.Loaded -= Handler;
                    }
                source.Loaded += Handler;
                }
            }
        #endregion
        //#region M:DoAfterLoaded<T:FrameworkContentElement>({this}T,Action<T>)
        //public static void DoAfterLoaded<T>(this T source,Action<T> predicate)
        //    where T: FrameworkContentElement {
        //    if (source == null) { throw new ArgumentNullException(nameof(source)); }
        //    if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
        //    RoutedEventHandler handler = null;
        //    handler = delegate {
        //        predicate.Invoke(source);
        //        source.Loaded -= handler;
        //        };
        //    source.Loaded += handler;
        //    }
        //#endregion
        #region M:DoAfterLoaded({this}FrameworkContentElement,Action)
        public static void DoAfterLoaded(this FrameworkContentElement source,Action predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                void Handler(Object sender, RoutedEventArgs e) {
                    predicate.Invoke();
                    source.Loaded -= Handler;
                    }
                source.Loaded += Handler;
                }
            }
        #endregion
        #region M:DoAfterPreviewLostKeyboardFocus<T>({this}T,Action)
        public static void DoAfterPreviewLostKeyboardFocus<T>(this T source,Action predicate)
            where T: FrameworkElement {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (predicate == null) { throw new ArgumentNullException("predicate"); }
            KeyboardFocusChangedEventHandler handler = null;
            handler = delegate {
                predicate.Invoke();
                source.PreviewLostKeyboardFocus -= handler;
                };
            source.PreviewLostKeyboardFocus += handler;
            }
        #endregion
        #region M:DoAfterPreviewLostKeyboardFocus<T>({this}T,Action<KeyboardFocusChangedEventArgs>)
        public static void DoAfterPreviewLostKeyboardFocus<T>(this T source,Action<KeyboardFocusChangedEventArgs> predicate)
            where T: FrameworkElement {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (predicate == null) { throw new ArgumentNullException("predicate"); }
            KeyboardFocusChangedEventHandler handler = null;
            handler = delegate (Object sender, KeyboardFocusChangedEventArgs e) {
                predicate.Invoke(e);
                source.PreviewLostKeyboardFocus -= handler;
                };
            source.PreviewLostKeyboardFocus += handler;
            }
        #endregion
        #region M:DoAfterDataContextChanged({this}FrameworkContentElement,Action)
        public static void DoAfterDataContextChanged(this FrameworkContentElement source,Action predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                void Handler(Object sender, DependencyPropertyChangedEventArgs e) {
                    predicate.Invoke();
                    source.DataContextChanged -= Handler;
                    }
                source.DataContextChanged += Handler;
                }
            }
        #endregion
        #region M:DoAfterInitialization({this}FrameworkContentElement,Action)
        public static void DoAfterInitialization(this FrameworkContentElement source,Action predicate) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            void Handler(Object sender, EventArgs e) {
                predicate.Invoke();
                source.Initialized -= Handler;
                }
            source.Initialized += Handler;
            }
        #endregion

        #region M:OnDataContextChanged({this}FrameworkContentElement,Action)
        public static void OnDataContextChanged(this FrameworkContentElement source,Action predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                void Handler(Object sender, DependencyPropertyChangedEventArgs e) {
                    predicate.Invoke();
                    }
                source.DataContextChanged += Handler;
                }
            }
        #endregion
        #region M:SetFocusAfterLayoutUpdated({this}Control)
        public static void SetFocusAfterLayoutUpdated(this Control source) {
            if (source == null) { throw new ArgumentNullException("source"); }
            DoAfterLayoutUpdated(source, i => {
                source.Focus();
                });
            }
        #endregion
        #region M:IsConnectedToPresentationSource({this}DependencyObject):Boolean
        public static Boolean IsConnectedToPresentationSource(this DependencyObject obj) {
            return PresentationSource.FromDependencyObject(obj) != null;
            }
        #endregion
        #region M:SetBinding({this}DependencyObject,DependencyProperty,DependencyObject,DependencyProperty,BindingMode):BindingExpressionBase
        public static BindingExpressionBase SetBinding(this DependencyObject target, DependencyProperty targetProperty, DependencyObject source, DependencyProperty sourceProperty, BindingMode mode) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (sourceProperty != null) {
                return BindingOperations.SetBinding(target, targetProperty, new Binding() {
                    Source = source,
                    Path = new PropertyPath(sourceProperty),
                    Mode = mode
                    });
                }
            return null;
            }
        #endregion
        #region M:SetBinding({this}DependencyObject,DependencyProperty,DependencyObject,DependencyProperty,BindingMode,IValueConverter):BindingExpressionBase
        public static BindingExpressionBase SetBinding(this DependencyObject target, DependencyProperty targetProperty, DependencyObject source, DependencyProperty sourceProperty, BindingMode mode, IValueConverter converter) {
            if (target == null) { throw new ArgumentNullException(nameof(target)); }
            if (sourceProperty != null) {
                return BindingOperations.SetBinding(target, targetProperty, new Binding() {
                    Source = source,
                    Path = new PropertyPath(sourceProperty),
                    Mode = mode,
                    Converter = converter
                    });
                }
            return null;
            }
        #endregion
        #region M:GetValue({this}Binding):Object
        private class BindingEvaluator : BridgeReference
            {
            public BindingEvaluator(BindingBase binding) { SetBinding(SourceProperty, binding); }
            protected override void OnTargetChanged()
                {
                base.OnTargetChanged();
                }
            }
        public static Object GetValue(this BindingBase binding) {
            return (new BindingEvaluator(binding)).Target;
            }
        #endregion

        #region M:IsDefaultValue({this}DependencyProperty,DependencyObject):Boolean
        public static Boolean IsDefaultValue(this DependencyProperty source, DependencyObject o)
            {
            var metadata = source.GetMetadata(o);
            var value = o.GetValue(source);
            return Equals(metadata.DefaultValue,value);
            }
        #endregion
        #region M:IsDefaultValue({this}DependencyObject,DependencyProperty):Boolean
        public static Boolean IsDefaultValue(this DependencyObject source, DependencyProperty property)
            {
            return IsDefaultValue(source,property,out var value);
            }
        #endregion
        #region M:IsDefaultValue({this}DependencyObject,DependencyProperty,{out}Object):Boolean
        public static Boolean IsDefaultValue(this DependencyObject source, DependencyProperty property,out Object value)
            {
            var metadata = property.GetMetadata(source);
            value = source.GetValue(property);
            return Equals(metadata.DefaultValue,value);
            }
        #endregion
        #region M:IsOwnedBy({this}DependencyProperty,Type):Boolean
        public static Boolean IsOwnedBy(this DependencyProperty source, Type type) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            var types = PropertyFromName.
                OfType<DictionaryEntry>().
                Where(i => ReferenceEquals(i.Value,source)).
                Select(i=> (Type)FromNameKeyTypeOwnerField.GetValue(i.Key)).
                ToArray();
            return types.Any(i => (type == i) || type.IsSubclassOf(i));
            }
        #endregion

        #region P:UseExtensions:Boolean
        public static readonly DependencyProperty IsExtendedProperty = DependencyProperty.RegisterAttached("IsExtended", typeof(Boolean), typeof(Extensions), new PropertyMetadata(default(Boolean),OnIsExtendedChanged));
        public static void SetIsExtended(DependencyObject e, Boolean value)
            {
            e.SetValue(IsExtendedProperty, value);
            }

        public static Boolean GetIsExtended(DependencyObject e)
            {
            return (Boolean)e.GetValue(IsExtendedProperty);
            }
        private static void OnIsExtendedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if ((Boolean)e.NewValue) {
                if (sender is FrameworkElement source) {
                    source.SetValue(ActualWidthPropertyKey, source.ActualWidth);
                    source.SizeChanged += OnSizeChanged;
                    }
                }
            }

        private static void OnSizeChanged(Object sender, SizeChangedEventArgs e) {
            if (e.Source is FrameworkElement source) {
                source.SetValue(ActualWidthPropertyKey, source.ActualWidth);
                }
            }
        #endregion

        public static void ForAll<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var i in source) {
                action(i);
                }
            }

        public static void WaitForLoad(this FrameworkContentElement source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (!source.IsLoaded) {
                using (var e = new ManualResetEventSlim(false)) {
                    DoAfterLoaded(source,()=>{
                        e.Set();
                        });
                    e.Wait();
                    }
                }
            }

        static Extensions() {
            var PropertyFromNameFieldInfo = typeof(DependencyProperty).GetField("PropertyFromName",BindingFlags.Static|BindingFlags.NonPublic);
            PropertyFromName = (PropertyFromNameFieldInfo != null)
                ? (Hashtable)PropertyFromNameFieldInfo.GetValue(null)
                : new Hashtable();
            FromNameKeyType = typeof(DependencyProperty).GetNestedType("FromNameKey",BindingFlags.NonPublic);
            FromNameKeyTypeOwnerField = FromNameKeyType.GetField("_ownerType",BindingFlags.NonPublic|BindingFlags.Instance);
            }

        private static readonly DependencyPropertyKey ActualWidthPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ActualWidth", typeof(Double), typeof(Extensions), new PropertyMetadata(default(Double)));
        public static readonly DependencyProperty ActualWidthProperty=ActualWidthPropertyKey.DependencyProperty;
        private static readonly Hashtable PropertyFromName;
        private static readonly Type FromNameKeyType;
        private static readonly FieldInfo FromNameKeyTypeOwnerField;
        }
    }