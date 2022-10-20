using System;
using System.Windows;
using System.Windows.Data;

namespace BinaryStudio.PlatformUI.Extensions
    {
    public static class Extensions
        {
        #region M:DoAfterLayoutUpdated(UIElement,Action)
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
        #region M:DoAfterLayoutUpdated<T>(T,Action<T>)
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
        }
    }