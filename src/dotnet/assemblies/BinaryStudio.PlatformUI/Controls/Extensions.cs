using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace BinaryStudio.PlatformUI.Controls
    {
    public static class Extensions
        {
        #region M:FindDescendant({this}DependencyObject,Predicate<DependencyObject>):DependencyObject
        public static DependencyObject FindDescendant(this DependencyObject source, Predicate<DependencyObject> predicate) {
            if (source == null) { return null; }
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
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