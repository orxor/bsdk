using System;
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
            EventHandler handler = null;
            handler = delegate {
                predicate.Invoke();
                source.LayoutUpdated -= handler;
                };
            source.LayoutUpdated += handler;
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
        #region M:DoAfterLoaded<T>({this}T,Action<T>)
        public static void DoAfterLoaded<T>(this T source,Action<T> predicate)
            where T: FrameworkElement {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (predicate == null) { throw new ArgumentNullException("predicate"); }
            RoutedEventHandler handler = null;
            handler = delegate {
                predicate.Invoke(source);
                source.Loaded -= handler;
                };
            source.Loaded += handler;
            }
        #endregion
        #region M:DoAfterLoaded<T>({this}T,Action)
        public static void DoAfterLoaded<T>(this T source,Action predicate)
            where T: FrameworkElement {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (predicate == null) { throw new ArgumentNullException("predicate"); }
            RoutedEventHandler handler = null;
            handler = delegate {
                predicate.Invoke();
                source.Loaded -= handler;
                };
            source.Loaded += handler;
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
            public BindingEvaluator(Binding binding) { SetBinding(SourceProperty, binding); }
            public BindingEvaluator(BindingBase binding) { SetBinding(SourceProperty, binding); }
            protected override void OnTargetChanged()
                {
                base.OnTargetChanged();
                }
            }
        public static Object GetValue(this Binding binding) {
            return (new BindingEvaluator(binding)).Target;
            }
        public static Object GetValue(this BindingBase binding) {
            return (new BindingEvaluator(binding)).Target;
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

        private static readonly DependencyPropertyKey ActualWidthPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ActualWidth", typeof(Double), typeof(Extensions), new PropertyMetadata(default(Double)));
        public static readonly DependencyProperty ActualWidthProperty=ActualWidthPropertyKey.DependencyProperty;
        }
    }