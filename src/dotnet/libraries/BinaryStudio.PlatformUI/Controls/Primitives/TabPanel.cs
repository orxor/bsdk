﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    public class TabPanel : Panel
        {
        private Int32 SelectedGroup = 0;
        private Int32 NumberOfGroups,NumberOfItems;
        private Size ItemSize;

        #region P:TabStripPlacement:Dock
        public static readonly DependencyProperty TabStripPlacementProperty = DependencyProperty.Register("TabStripPlacement", typeof(Dock), typeof(TabPanel), new PropertyMetadata(default(Dock), OnTabStripPlacementChanged));
        private static void OnTabStripPlacementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is TabPanel source) {
                source.OnTabStripPlacementChanged();
                }
            }

        private void OnTabStripPlacementChanged()
            {
            InvalidateMeasure();
            }

        public Dock TabStripPlacement
            {
            get { return (Dock)GetValue(TabStripPlacementProperty); }
            set { SetValue(TabStripPlacementProperty, value); }
            }
        #endregion

        //private Dock TabStripPlacement { get {
        //    return (TemplatedParent is TabControl control)
        //            ? control.TabStripPlacement
        //            : Dock.Top;
        //    }}

        private static Size GetDesiredSizeWithoutMargin(UIElement element) {
            var thickness = (Thickness)element.GetValue(MarginProperty);
            var result = default(Size);
            result.Height = Math.Max(0.0, element.DesiredSize.Height - thickness.Top  - thickness.Bottom);
            result.Width  = Math.Max(0.0, element.DesiredSize.Width  - thickness.Left - thickness.Right);
            return result;
            }

        /// <summary>When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement"/>-derived class.</summary>
        /// <param name="constraint">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size constraint) {
            var result = default(Size);
            NumberOfItems = 0;
            NumberOfGroups = 0;
            ItemSize = default;
            var i = 0;
            Double α = 0.0, β = 0.0;
            switch (TabStripPlacement) {
                case Dock.Top:
                case Dock.Bottom:
                    {
                    foreach (UIElement Child in InternalChildren) {
                        if (Child.Visibility != Visibility.Collapsed) {
                            NumberOfItems++;
                            Child.Measure(constraint);
                            var Size = Child.DesiredSize;
                            ItemSize.Width += Size.Width;
                            ItemSize.Height = (ItemSize.Height < Size.Height) ? Size.Height : ItemSize.Height;
                            if (((α + Size.Width) > constraint.Width) && (i > 0)) {
                                β = (β < α) ? α : β;
                                α = Size.Width;
                                i = 1;
                                NumberOfGroups++;
                                }
                            else
                                {
                                α += Size.Width;
                                i++;
                                }
                            }
                        }
                    β = (β < α) ? α : β;
                    result.Height = ItemSize.Height * (NumberOfGroups + 1);
                    result.Width = (Double.IsInfinity(result.Width) || DoubleUtil.IsNaN(result.Width) || (β < constraint.Width))
                        ? β
                        : constraint.Width;
                    NumberOfGroups++;
                    }
                    break;
                case Dock.Left:
                case Dock.Right:
                    {
                    foreach (UIElement Child in InternalChildren) {
                        if (Child.Visibility != Visibility.Collapsed) {
                            NumberOfItems++;
                            Child.Measure(constraint);
                            var Size = Child.DesiredSize;
                            ItemSize.Height += Size.Height;
                            ItemSize.Width = (ItemSize.Width < Size.Width) ? Size.Width : ItemSize.Width;
                            if (((α + Size.Height) > constraint.Height) && (i > 0)) {
                                β = (β < α) ? α : β;
                                α = Size.Height;
                                i = 1;
                                NumberOfGroups++;
                                }
                            else
                                {
                                α += Size.Height;
                                i++;
                                }
                            }
                        }
                    β = (β < α) ? α : β;
                    result.Width = ItemSize.Width * (NumberOfGroups + 1);
                    result.Height = (Double.IsInfinity(result.Height) || DoubleUtil.IsNaN(result.Height) || (β < constraint.Height))
                        ? β
                        : constraint.Height;
                    NumberOfGroups++;
                    }
                    break;
                }
            return result;
            }

        private class GroupInfo
            {
            public Boolean IsSelected;
            public Size Size;
            public readonly IList<UIElement> Children = new List<UIElement>();

            public GroupInfo()
                {
                }

            public GroupInfo(params UIElement[] children) {
                foreach (var child in children) {
                    Children.Add(child);
                    }
                }

            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            public override String ToString()
                {
                return $"Count = {Children.Count}";
                }
            }

        private IEnumerable<GroupInfo> PrepareHorizontal(Double constraint) {
            SelectedGroup = -1;
            var α = (constraint*NumberOfGroups)/ItemSize.Width;
            var β = new GroupInfo();
            var x = 0.0;
            Int32 i = 0, j = 0;
            foreach (UIElement Child in InternalChildren) {
                if (Child.Visibility != Visibility.Collapsed) {
                    var Size = Child.DesiredSize;
                    if (((x + α*Size.Width) > constraint) && (i > 0) && (j < NumberOfGroups - 1)) {
                        yield return β;
                        β = new GroupInfo(Child) {Size = {Width = Size.Width}};
                        x = α*Size.Width;
                        i = 1;
                        j++;
                        }
                    else
                        {
                        x += α*Size.Width;
                        β.Size.Width += Size.Width;
                        β.Children.Add(Child);
                        i++;
                        }
                    β.IsSelected |= (Boolean)Child.GetValue(Selector.IsSelectedProperty);
                    if (β.IsSelected) { SelectedGroup = j; }
                    }
                }
            yield return β;
            }

        private IEnumerable<GroupInfo> PrepareVertical(Double constraint) {
            SelectedGroup = -1;
            var α = (constraint*NumberOfGroups)/ItemSize.Height;
            var β = new GroupInfo();
            var y = 0.0;
            Int32 i = 0, j = 0;
            foreach (UIElement Child in InternalChildren) {
                if (Child.Visibility != Visibility.Collapsed) {
                    var Size = Child.DesiredSize;
                    if (((y + α*Size.Height) > constraint) && (i > 0) && (j < NumberOfGroups - 1)) {
                        yield return β;
                        β = new GroupInfo(Child) {Size = {Height = Size.Height}};
                        y = α*Size.Height;
                        i = 1;
                        j++;
                        }
                    else
                        {
                        y += α*Size.Height;
                        β.Size.Height += Size.Height;
                        β.Children.Add(Child);
                        i++;
                        }
                    β.IsSelected |= (Boolean)Child.GetValue(Selector.IsSelectedProperty);
                    if (β.IsSelected) { SelectedGroup = j; }
                    }
                }
            yield return β;
            }

        /// <summary>When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement"/> derived class.</summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize) {
            var placement = TabStripPlacement;
            switch (placement) {
                case Dock.Top:
                case Dock.Bottom:
                    {
                    if (NumberOfGroups == 1) {
                        var α = 0.0;
                        foreach (UIElement Child in InternalChildren) {
                            if (Child.Visibility != Visibility.Collapsed) {
                                var Size = Child.DesiredSize;
                                Child.Arrange(new Rect(α, 0, Size.Width, ItemSize.Height));
                                α += Size.Width;
                                }
                            }
                        }
                    else
                        {
                        var y = 0.0;
                        var groups = PrepareHorizontal(finalSize.Width).ToArray();
                        var i = (placement == Dock.Top)
                            ? NumberOfGroups - 1
                            : 0;
                        var β = groups[i];
                        groups[i] = groups[SelectedGroup];
                        groups[SelectedGroup] = β;
                        foreach (var g in groups) {
                            var α = finalSize.Width/g.Size.Width;
                            var x = 0.0;
                            foreach (var Child in g.Children) {
                                var Size = Child.DesiredSize;
                                Child.Arrange(new Rect(x, y, α*Size.Width, ItemSize.Height));
                                x += α*Size.Width;
                                }
                            y += ItemSize.Height;
                            }
                        }
                    }
                    break;
                case Dock.Left:
                case Dock.Right:
                    {
                    if (NumberOfGroups == 1) {
                        var α = 0.0;
                        foreach (UIElement Child in InternalChildren) {
                            if (Child.Visibility != Visibility.Collapsed) {
                                var Size = Child.DesiredSize;
                                Child.Arrange(new Rect(0, α, ItemSize.Width, Size.Height));
                                α += Size.Height;
                                }
                            }
                        }
                    else
                        {
                        var x = 0.0;
                        var groups = PrepareVertical(finalSize.Height).ToArray();
                        var i = (placement == Dock.Left)
                            ? NumberOfGroups - 1
                            : 0;
                        var β = groups[i];
                        groups[i] = groups[SelectedGroup];
                        groups[SelectedGroup] = β;
                        foreach (var g in groups) {
                            var α = finalSize.Height/g.Size.Height;
                            var y = 0.0;
                            foreach (var Child in g.Children) {
                                var Size = Child.DesiredSize;
                                Child.Arrange(new Rect(x, y,ItemSize.Width, α*Size.Height));
                                y += α*Size.Height;
                                }
                            x += ItemSize.Width;
                            }
                        }
                    }
                    break;
                }
            return finalSize;
            }
        }
    }