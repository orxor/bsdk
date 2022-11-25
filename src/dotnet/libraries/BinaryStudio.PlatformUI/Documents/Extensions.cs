using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace BinaryStudio.PlatformUI.Controls
    {
    public static partial class Extensions
        {
        #region M:GetForeground({this}TableRow):Brush
        public static Brush GetForeground(this TableRow source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source.IsDefaultValue(TextElement.ForegroundProperty,out var value)) {
                }
            return (Brush)value;
            }
        #endregion
        #region M:GetForeground({this}TableRowGroup):Brush
        public static Brush GetForeground(this TableRowGroup source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source.IsDefaultValue(TextElement.ForegroundProperty,out var value)) {
                }
            return (Brush)value;
            }
        #endregion
        #region M:SetForegroundToSelfAndDescendants({this}DependencyObject,Brush)
        public static void SetForegroundToSelfAndDescendants(this DependencyObject source, Brush brush) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var r = SetForeground(source,brush);
            foreach (var child in LogicalTreeHelper.GetChildren(source).OfType<DependencyObject>()) {
                SetForegroundToSelfAndDescendants(child,r);
                }
            }
        #endregion

        private static Brush SetForeground(DependencyObject source,Brush brush) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source is TextElement) {
                if (source.IsDefaultValue(TextElement.ForegroundProperty,out var value)) {
                    TextElement.SetForeground(source,brush);
                    return brush;
                    }
                return (Brush)value;
                }
            throw new NotSupportedException();
            }
        }
    }