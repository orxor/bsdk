using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace BinaryStudio.PlatformUI.Controls
    {
    public static partial class Extensions
        {
        #region M:FindDescendant({this}DependencyObject,Predicate<DependencyObject>):DependencyObject
        public static DependencyObject FindDescendant(this DependencyObject source, Predicate<DependencyObject> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source == null) { return null; }
            foreach (var i in Descendants<DependencyObject>(source)) {
                if (predicate(i)) {
                    return i;
                    }
                }
            return null;
            }
        #endregion
        #region M:FindDescendant<T>({this}DependencyObject):T
        public static T FindDescendant<T>(this DependencyObject source)
            where T : class
            {
            return Descendants<T>(source).FirstOrDefault();
            }
        #endregion
        #region M:FindAncestor<T>({this}DependencyObject):T
        public static T FindAncestor<T>(this DependencyObject source)
            where T : class
            {
            return Ancestors<T>(source,GetVisualOrLogicalParent).FirstOrDefault();
            }
        #endregion
        #region M:Descendants<T>({this}DependencyObject):IEnumerable<T>
        public static IEnumerable<T> Descendants<T>(this DependencyObject source)
            where T : class
            {
            if (source == null) { yield break; }
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(source); ++i) {
                var child = VisualTreeHelper.GetChild(source, i);
                if (child != null) {
                    if (child is T e) { yield return e; }
                    foreach (var o in Descendants<T>(child)) {
                        yield return o;
                        }
                    }
                }
            }
        #endregion
        #region M:GetVisualOrLogicalParent({this}DependencyObject):DependencyObject
        public static DependencyObject GetVisualOrLogicalParent(this DependencyObject source) {
            if (source == null) { return null; }
            return (source is Visual)
                ? VisualTreeHelper.GetParent(source) ?? LogicalTreeHelper.GetParent(source)
                : LogicalTreeHelper.GetParent(source);
            }
        #endregion
        #region M:Ancestors<T>({this}DependencyObject,Func<DependencyObject,DependencyObject>):IEnumerable<T>
        public static IEnumerable<T> Ancestors<T>(this DependencyObject source, Func<DependencyObject,DependencyObject> selector)
            where T: class
            {
            if (selector == null) { throw new ArgumentNullException(nameof(selector)); }
            if (source == null) { yield break; }
            for (var i = selector(source); i != null; i = selector(i)) {
                if (i is T e) {
                    yield return e;
                    }
                }
            }
        #endregion
        #region M:Ancestors<T>({this}DependencyObject):IEnumerable<T>
        public static IEnumerable<T> Ancestors<T>(this DependencyObject source)
            where T: class
            {
            return Ancestors<T>(source,GetVisualOrLogicalParent);
            }
        #endregion
        #region M:LogicalDescendants({this}DependencyObject):DependencyObject
        public static IEnumerable<DependencyObject> LogicalDescendants(this DependencyObject source) {
            if (source != null) {
                foreach (var i in LogicalTreeHelper.GetChildren(source).OfType<DependencyObject>()) {
                    yield return i;
                    foreach (var j in LogicalDescendants(i)) {
                        yield return j;
                        }
                    }
                }
            }
        #endregion

        public static void DrawText(this DrawingContext context, Point origin, String text) {
            #if NET40 || NET45
            var r = new FormattedText(text, CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,new Typeface("Segoe UI"),
                10.0, Brushes.Gray);
            #else
            var r = new FormattedText(text, CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,new Typeface("Segoe UI"),
                10.0, Brushes.Gray, pixelsPerDip: 1.0);
            #endif
            context.DrawText(r, origin);
            }

        public static void DrawText(this DrawingContext context, Point origin, FormattedText text) {
            context.DrawText(text, origin);
            }

        public static T Clone<T>(this T source, Double opacity)
            where T: Brush
            {
            var r = (T)source.Clone();
            r.Opacity = opacity;
            return r;
            }
        }
    }