using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    public class XYViewport : MultiSelector
        {
        static XYViewport()
            {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XYViewport), new FrameworkPropertyMetadata(typeof(XYViewport)));
            }

        #region P:XYViewport.Left:Double
        public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(Double), typeof(XYViewport), new PropertyMetadata(Double.NaN));
        public static void SetLeft(DependencyObject element, Double value)
            {
            element.SetValue(LeftProperty, value);
            }

        public static Double GetLeft(DependencyObject element)
            {
            return (Double)element.GetValue(LeftProperty);
            }
        #endregion
        #region P:XYViewport.Top:Double
        public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(Double), typeof(XYViewport), new PropertyMetadata(Double.NaN));
        public static void SetTop(DependencyObject element, Double value)
            {
            element.SetValue(TopProperty, value);
            }

        public static Double GetTop(DependencyObject element)
            {
            return (Double)element.GetValue(TopProperty);
            }
        #endregion
        #region P:XYViewport.Right:Double
        public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(Double), typeof(XYViewport), new PropertyMetadata(Double.NaN));
        public static void SetRight(DependencyObject element, Double value)
            {
            element.SetValue(RightProperty, value);
            }

        public static Double GetRight(DependencyObject element)
            {
            return (Double) element.GetValue(RightProperty);
            }
        #endregion
        #region P:XYViewport.Bottom:Double
        public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(Double), typeof(XYViewport), new PropertyMetadata(Double.NaN));
        public static void SetBottom(DependencyObject element, Double value)
            {
            element.SetValue(BottomProperty, value);
            }

        public static Double GetBottom(DependencyObject element)
            {
            return (Double) element.GetValue(BottomProperty);
            }
        #endregion
        #region M:ClearContainerForItemOverride(DependencyObject,Object)
        /// <summary>Returns an item container to the state it was in before <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/>.</summary>
        /// <param name="element">The item container element.</param>
        /// <param name="item">The data item.</param>
        protected override void ClearContainerForItemOverride(DependencyObject element, Object item)
            {
            base.ClearContainerForItemOverride(element, item);
            }
        #endregion
        #region M:GetContainerForItemOverride:DependencyObject
        /// <summary>Creates or identifies the element that is used to display the given item.</summary>
        /// <returns>The element that is used to display the given item.</returns>
        protected override DependencyObject GetContainerForItemOverride()
            {
            return new XYViewportNode();
            }
        #endregion
        #region M:PrepareContainerForItemOverride(DependencyObject,Object)
        /// <summary>Prepares the specified element to display the specified item.</summary>
        /// <param name="element">The element that is used to display the specified item.</param>
        /// <param name="item">The specified item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, Object item) {
            base.PrepareContainerForItemOverride(element, item);
            if (element is XYViewportNode container) {
                container.Owner = this;
                }
            }
        #endregion
        #region M:OnSelectionChanged(SelectionChangedEventArgs)
        /// <summary>Called when the selection changes.</summary>
        /// <param name="e">The event data.</param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e) {
            base.OnSelectionChanged(e);
            if (ItemsHost is XYViewportPanel panel) {
                panel.OnSelectionChanged(e);
                }
            }
        #endregion
        #region M:OnMouseLeftButtonDown(MouseButtonEventArgs)
        /// <summary>Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> routed event is raised on this element. Implement this method to add class handling for this event.</summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
            {
            Diagnostics.Print((new StackTrace()).GetFrame(0).GetMethod());
            base.OnMouseLeftButtonDown(e);
            }
        #endregion
        #region M:OnPreviewMouseLeftButtonDown(MouseButtonEventArgs)
        /// <summary>Invoked when an unhandled <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.</summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
            {
            Diagnostics.Print((new StackTrace()).GetFrame(0).GetMethod());
            base.OnPreviewMouseLeftButtonDown(e);
            }
        #endregion

        internal void SelectItem(DependencyObject item) {
            if (!IsUpdatingSelectedItems) {
                BeginUpdateSelectedItems();
                SelectedItems.Add(item);
                EndUpdateSelectedItems();
                if (ItemsHost is XYViewportPanel panel) {
                    panel.BringTop(item as UIElement);
                    }
                }
            }

        internal void UnselectItem(DependencyObject item) {
            if (!IsUpdatingSelectedItems) {
                BeginUpdateSelectedItems();
                SelectedItems.Remove(item);
                EndUpdateSelectedItems();
                }
            }

        public Panel ItemsHost { get {
            var pi = typeof(ItemsControl).GetProperty("ItemsHost", BindingFlags.Instance | BindingFlags.NonPublic);
            #if NET40
            return (Panel)pi.GetValue(this, null);
            #else
            return (Panel)pi.GetValue(this);
            #endif
            }}
        }
    }
