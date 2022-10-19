using System;
using System.Windows;

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
        }
    }