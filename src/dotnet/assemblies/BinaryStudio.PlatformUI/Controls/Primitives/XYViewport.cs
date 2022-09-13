using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        protected override void PrepareContainerForItemOverride(DependencyObject element, Object item)
            {
            base.PrepareContainerForItemOverride(element, item);
            }
        #endregion
        }
    }
