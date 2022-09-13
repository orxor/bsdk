using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    using static Double;
    public class XYViewportPanel : Panel, IScrollInfo
        {
        #region P:LinkedScrollInfo:IScrollInfo
        public static readonly DependencyProperty LinkedScrollInfoProperty = DependencyProperty.Register("LinkedScrollInfo", typeof(IScrollInfo), typeof(XYViewportPanel), new PropertyMetadata(default(IScrollInfo)));
        public IScrollInfo LinkedScrollInfo
            {
            get { return (IScrollInfo)GetValue(LinkedScrollInfoProperty); }
            set { SetValue(LinkedScrollInfoProperty, value); }
            }
        #endregion
        #region P:PhysicalViewport:Vector
        private static readonly DependencyPropertyKey PhysicalViewportPropertyKey = DependencyProperty.RegisterReadOnly("PhysicalViewport", typeof(Vector), typeof(XYViewportPanel), new PropertyMetadata(default(Vector), OnPhysicalViewportChanged));
        /// <summary>Identifies the <see cref="PhysicalViewport" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="PhysicalViewport" /> dependency property.</returns>
        public static readonly DependencyProperty PhysicalViewportProperty = PhysicalViewportPropertyKey.DependencyProperty;
        private static void OnPhysicalViewportChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportPanel source) {
                source.OnPhysicalViewportChanged();
                }
            }

        protected virtual void OnPhysicalViewportChanged() {
            Viewport = PhysicalViewport;
            InvalidateVisual();
            }

        public Vector PhysicalViewport {
            get { return (Vector)GetValue(PhysicalViewportProperty); }
            private set { SetValue(PhysicalViewportPropertyKey, value); }
            }
        #endregion
        #region P:IScrollInfo.ViewportWidth(ViewportHeight):Vector
        private static readonly DependencyPropertyKey ViewportPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Viewport), typeof(Vector), typeof(XYViewportPanel), new PropertyMetadata(default(Vector), OnViewportChanged));
        /// <summary>Identifies the <see cref="Viewport"/> dependency property.</summary>
        /// <returns>The identifier for the <see cref="Viewport"/> dependency property.</returns>
        public static readonly DependencyProperty ViewportProperty = ViewportPropertyKey.DependencyProperty;
        private static void OnViewportChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportPanel source) {
                source.OnViewportChanged(
                    (Vector)e.NewValue,
                    (Vector)e.OldValue);
                }
            }

        protected virtual void OnViewportChanged(Vector NewValue, Vector OldValue) {
            if ((Offset.X > 0) || (Offset.Y > 0)) {
                var offsetP = NewValue-OldValue;
                var offsetO = Offset - offsetP;
                Offset = new Vector(
                    Math.Max(offsetO.X,0.0),
                    Math.Max(offsetO.Y,0.0));
                }
            InvalidateScrollInfo();
            }

        /// <summary>
        /// Gets the vertical and horizontal size of the viewport for this content.
        /// </summary>
        public Vector Viewport {
            get { return (Vector)GetValue(ViewportProperty); }
            protected set { SetValue(ViewportPropertyKey, value); }
            }
        #region P:IScrollInfo.ViewportWidth:Double
        /// <summary>Gets the horizontal size of the viewport for this content.</summary>
        /// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the horizontal size of the viewport for this content. This property has no default value.</returns>
        public Double ViewportWidth { get {
            return Viewport.X;
            }}
        #endregion
        #region P:IScrollInfo.ViewportHeight:Double
        /// <summary>Gets the vertical size of the viewport for this content.</summary>
        /// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the vertical size of the viewport for this content. This property has no default value.</returns>
        public Double ViewportHeight { get {
            return Viewport.Y;
            }}
        #endregion
        #endregion
        #region P:IScrollInfo.ExtentWidth(ExtentHeight):Vector
        private static readonly DependencyPropertyKey ExtentPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Extent), typeof(Vector), typeof(XYViewportPanel), new PropertyMetadata(default(Vector), OnExtentChanged));
        /// <summary>Identifies the <see cref="Extent"/> dependency property.</summary>
        /// <returns>The identifier for the <see cref="Extent"/> dependency property.</returns>
        public static readonly DependencyProperty ExtentProperty = ExtentPropertyKey.DependencyProperty;
        private static void OnExtentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportPanel source) {
                source.OnExtentChanged();
                }
            }

        protected virtual void OnExtentChanged() {
            InvalidateScrollInfo();
            }

        /// <summary>
        /// Gets the vertical and horizontal size of the extent for this content.
        /// </summary>
        public Vector Extent {
            get { return (Vector)GetValue(ExtentProperty); }
            protected set { SetValue(ExtentPropertyKey, value); }
            }
        #region P:IScrollInfo.ExtentWidth:Double
        /// <summary>Gets the horizontal size of the extent.</summary>
        /// <returns>A <see cref="T:System.Double"/> that represents, in device independent pixels, the horizontal size of the extent. This property has no default value.</returns>
        public Double ExtentWidth { get {
            return Extent.X;
            }}
        #endregion
        #region P:IScrollInfo.ExtentHeight:Double
        /// <summary>Gets the vertical size of the extent.</summary>
        /// <returns>A <see cref="T:System.Double"/> that represents, in device independent pixels, the vertical size of the extent.
        /// This property has no default value.</returns>
        public Double ExtentHeight { get {
            return Extent.Y;
            }}
        #endregion
        #endregion
        #region P:IScrollInfo.HorizontalOffset(VerticalOffset):Vector
        private static readonly DependencyPropertyKey OffsetPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Offset), typeof(Vector), typeof(XYViewportPanel),new PropertyMetadata(default(Vector), OnOffsetChanged, OnOffsetCoerceValue));

        private static Object OnOffsetCoerceValue(DependencyObject sender, Object basevalue) {
            return DoubleUtil.Round((Vector)basevalue);
            }

        /// <summary>Identifies the <see cref="Offset"/> dependency property.</summary>
        /// <returns>The identifier for the <see cref="Offset"/> dependency property.</returns>
        public static readonly DependencyProperty OffsetProperty = OffsetPropertyKey.DependencyProperty;
        private static void OnOffsetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportPanel source) {
                if (source.LinkedScrollInfo != null) {
                    var value = (Vector)e.NewValue;
                    source.LinkedScrollInfo.SetHorizontalOffset(value.X);
                    source.LinkedScrollInfo.SetVerticalOffset(value.Y);
                    }
                source.OnOffsetChanged();
                }
            }

        protected virtual void OnOffsetChanged() {
            InvalidateScrollInfo();
            }

        /// <summary>
        /// Gets the vertical and horizontal offset of scrolled content.
        /// </summary>
        public Vector Offset {
            get { return (Vector)GetValue(OffsetProperty); }
            protected set { SetValue(OffsetPropertyKey, value); }
            }
        /// <summary>Gets the horizontal offset of the scrolled content.</summary>
        /// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the horizontal offset. This property has no default value.</returns>
        Double IScrollInfo.HorizontalOffset
            {
            get { return Offset.X; }
            }
        Double IScrollInfo.VerticalOffset
            {
            get { return Offset.Y; }
            }
        #endregion
        #region M:IScrollInfo.LineUp
        /// <summary>Scrolls up within content by one logical unit.</summary>
        void IScrollInfo.LineUp()
            {
            ((IScrollInfo)this).SetVerticalOffset(Offset.Y - 1);
            }
        #endregion
        #region M:IScrollInfo.LineDown
        /// <summary>Scrolls down within content by one logical unit.</summary>
        void IScrollInfo.LineDown()
            {
            ((IScrollInfo)this).SetVerticalOffset(Math.Round(Offset.Y + 1));
            }
        #endregion
        #region M:IScrollInfo.LineLeft
        /// <summary>Scrolls left within content by one logical unit.</summary>
        void IScrollInfo.LineLeft()
            {
            ((IScrollInfo)this).SetVerticalOffset(Math.Round(Offset.Y - 1));
            }
        #endregion
        #region M:IScrollInfo.LineRight
        /// <summary>Scrolls right within content by one logical unit.</summary>
        void IScrollInfo.LineRight()
            {
            ((IScrollInfo)this).SetVerticalOffset(Math.Round(Offset.X + 1));
            }
        #endregion
        #region M:IScrollInfo.PageUp
        /// <summary>Scrolls up within content by one page.</summary>
        void IScrollInfo.PageUp()
            {
            ((IScrollInfo)this).SetVerticalOffset(Offset.Y - Viewport.Y);
            }
        #endregion
        #region M:IScrollInfo.PageDown
        /// <summary>Scrolls down within content by one page.</summary>
        void IScrollInfo.PageDown()
            {
            ((IScrollInfo)this).SetVerticalOffset(Offset.Y + Viewport.Y);
            }
        #endregion
        #region M:IScrollInfo.PageLeft
        /// <summary>Scrolls left within content by one page.</summary>
        void IScrollInfo.PageLeft()
            {
            ((IScrollInfo)this).SetHorizontalOffset(Offset.X - Viewport.X);
            }
        #endregion
        #region M:IScrollInfo.PageRight
        /// <summary>Scrolls right within content by one page.</summary>
        void IScrollInfo.PageRight()
            {
            ((IScrollInfo)this).SetHorizontalOffset(Offset.X + Viewport.X);
            }
        #endregion
        #region M:IScrollInfo.MouseWheelUp
        /// <summary>Scrolls up within content after a user clicks the wheel button on a mouse.</summary>
        void IScrollInfo.MouseWheelUp()
            {
            ((IScrollInfo)this).LineUp();
            }
        #endregion
        #region M:IScrollInfo.MouseWheelDown
        /// <summary>Scrolls down within content after a user clicks the wheel button on a mouse.</summary>
        void IScrollInfo.MouseWheelDown()
            {
            ((IScrollInfo)this).LineDown();
            }
        #endregion
        #region M:IScrollInfo.MouseWheelLeft
        /// <summary>Scrolls left within content after a user clicks the wheel button on a mouse.</summary>
        void IScrollInfo.MouseWheelLeft()
            {
            ((IScrollInfo)this).LineLeft();
            }
        #endregion
        #region M:IScrollInfo.MouseWheelRight
        /// <summary>Scrolls right within content after a user clicks the wheel button on a mouse.</summary>
        void IScrollInfo.MouseWheelRight()
            {
            ((IScrollInfo)this).LineRight();
            }
        #endregion
        #region M:IScrollInfo.SetHorizontalOffset(Double)
        /// <summary>Sets the amount of horizontal offset.</summary>
        /// <param name="offset">The degree to which content is horizontally offset from the containing viewport.</param>
        void IScrollInfo.SetHorizontalOffset(Double offset)
            {
            Offset = new Vector(offset,Offset.Y);
            }
        #endregion
        #region M:IScrollInfo.SetVerticalOffset(Double)
        /// <summary>Sets the amount of vertical offset.</summary>
        /// <param name="offset">The degree to which content is vertically offset from the containing viewport.</param>
        void IScrollInfo.SetVerticalOffset(Double offset)
            {
            Offset = new Vector(Offset.X,offset);
            }
        #endregion
        #region M:IScrollInfo.MakeVisible(Visual,Rect)
        /// <summary>Forces content to scroll until the coordinate space of a <see cref="T:System.Windows.Media.Visual"/> object is visible.</summary>
        /// <param name="visual">A <see cref="T:System.Windows.Media.Visual"/> that becomes visible.</param>
        /// <param name="rectangle">A bounding rectangle that identifies the coordinate space to make visible.</param>
        /// <returns>A <see cref="T:System.Windows.Rect"/> that is visible.</returns>
        Rect IScrollInfo.MakeVisible(Visual visual, Rect rectangle) {
            if (rectangle.IsEmpty || (visual == null) || (visual == this) || !IsAncestorOf(visual)) { return Rect.Empty; }
            throw new NotImplementedException();
            }
        #endregion
        #region P:IScrollInfo.CanHorizontallyScroll:Boolean
        public static readonly DependencyProperty CanHorizontallyScrollProperty = DependencyProperty.Register("CanHorizontallyScroll", typeof(Boolean), typeof(XYViewportPanel), new PropertyMetadata(default(Boolean), OnCanHorizontallyScrollChanged));
        private static void OnCanHorizontallyScrollChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportPanel source) {
                if (source.LinkedScrollInfo != null) { source.LinkedScrollInfo.CanHorizontallyScroll = (Boolean)e.NewValue; }
                source.InvalidateScrollInfo();
                }
            }

        /// <summary>Gets or sets a value that indicates whether scrolling on the horizontal axis is possible.</summary>
        /// <returns>true if scrolling is possible; otherwise, false. This property has no default value.</returns>
        public Boolean CanHorizontallyScroll
            {
            get { return LinkedScrollInfo?.CanHorizontallyScroll ?? (Boolean)GetValue(CanHorizontallyScrollProperty); }
            set { SetValue(CanHorizontallyScrollProperty, value); }
            }
        #endregion
        #region P:IScrollInfo.CanVerticallyScroll:Boolean
        public static readonly DependencyProperty CanVerticallyScrollProperty = DependencyProperty.Register("CanVerticallyScroll", typeof(Boolean), typeof(XYViewportPanel), new PropertyMetadata(default(Boolean), OnCanVerticallyScrollChanged));
        private static void OnCanVerticallyScrollChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportPanel source) {
                if (source.LinkedScrollInfo != null) { source.LinkedScrollInfo.CanVerticallyScroll = (Boolean)e.NewValue; }
                source.InvalidateScrollInfo();
                }
            }

        /// <summary>Gets or sets a value that indicates whether scrolling on the vertical axis is possible. </summary>
        /// <returns>true if scrolling is possible; otherwise, false. This property has no default value.</returns>
        public Boolean CanVerticallyScroll
            {
            get { return LinkedScrollInfo?.CanVerticallyScroll ?? (Boolean)GetValue(CanVerticallyScrollProperty); }
            set { SetValue(CanVerticallyScrollProperty, value); }
            }
        #endregion
        #region P:IScrollInfo.ScrollOwner:ScrollViewer
        public static readonly DependencyProperty ScrollOwnerProperty = DependencyProperty.Register("ScrollOwner", typeof(ScrollViewer), typeof(XYViewportPanel), new PropertyMetadata(default(ScrollViewer), OnScrollOwnerChanged));
        private static void OnScrollOwnerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportPanel source) {
                source.OnScrollOwnerChanged(
                    e.OldValue as ScrollViewer,
                    e.NewValue as ScrollViewer);
                }
            }

        protected virtual void OnScrollOwnerChanged(ScrollViewer o, ScrollViewer n) {
            if (o != null) { o.LayoutUpdated -= OnScrollOwnerLayoutUpdated; }
            if (n != null) { n.LayoutUpdated += OnScrollOwnerLayoutUpdated; }
            OnScrollOwnerChanged();
            }

        private void OnScrollOwnerChanged()
            {
            InvalidateScrollInfo();
            }

        #region M:OnScrollOwnerLayoutUpdated(Object,EventArgs)
        private void OnScrollOwnerLayoutUpdated(Object sender, EventArgs e) {
            OnScrollOwnerLayoutUpdated(ScrollOwner);
            }
        #endregion
        #region M:OnScrollOwnerLayoutUpdated(ScrollViewer)
        protected virtual void OnScrollOwnerLayoutUpdated(ScrollViewer sender) {
            if (sender != null) {
                var W = ((sender.HorizontalScrollBarVisibility == ScrollBarVisibility.Visible) || ((sender.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto) && (sender.ComputedHorizontalScrollBarVisibility == Visibility.Visible)))
                    ? SystemParameters.VerticalScrollBarWidth
                    : 0.0;
                var H = ((sender.VerticalScrollBarVisibility == ScrollBarVisibility.Visible) || (
                            (sender.VerticalScrollBarVisibility == ScrollBarVisibility.Auto) &&
                            (sender.ComputedVerticalScrollBarVisibility == Visibility.Visible)))
                    ? SystemParameters.HorizontalScrollBarHeight
                    : 0.0;
                OnScrollOwnerLayoutUpdated(new Size(
                    Math.Max(0, sender.ActualWidth  - W),
                    Math.Max(0, sender.ActualHeight - H)));
                }
            }
        #endregion
        #region M:OnScrollOwnerLayoutUpdated(Size)
        protected virtual void OnScrollOwnerLayoutUpdated(Size sz) {
            PhysicalViewport = new Vector(sz.Width, sz.Height);
            }
        #endregion

        /// <summary>Gets or sets a <see cref="T:System.Windows.Controls.ScrollViewer"/> element that controls scrolling behavior.</summary>
        /// <returns>A <see cref="T:System.Windows.Controls.ScrollViewer"/> element that controls scrolling behavior. This property has no default value.</returns>
        public ScrollViewer ScrollOwner {
            get
                {
                return (LinkedScrollInfo != null)
                    ? LinkedScrollInfo.ScrollOwner
                    : (ScrollViewer)GetValue(ScrollOwnerProperty);
                }
            set
                {
                if (LinkedScrollInfo != null) {
                    LinkedScrollInfo.ScrollOwner = value;
                    return;
                    }
                SetValue(ScrollOwnerProperty, value);
                }
            }
        #endregion
        #region M:InvalidateScrollInfo
        protected virtual void InvalidateScrollInfo() {
            if (ScrollOwner != null) {
                ScrollOwner.InvalidateScrollInfo();
                InvalidateVisual();
                }
            }
        #endregion
        #region M:void OnRender(DrawingContext)
        /// <summary>Draws the content of a <see cref="T:System.Windows.Media.DrawingContext"/> object during the render pass of a <see cref="T:System.Windows.Controls.Panel"/> element.</summary>
        /// <param name="context">The <see cref="T:System.Windows.Media.DrawingContext"/> object to draw.</param>
        protected override void OnRender(DrawingContext context)
            {
            base.OnRender(context);
            context.PushGuidelineSet(new GuidelineSet(
                new []{0.1, 0.1, 0.5},
                new []{0.1, 0.1, 0.5}));
            var pen = new Pen(Brushes.Gray.Clone(0.2), 1.0);
            var brush = Brushes.Gray.Clone(0.05);
            context.DrawRectangle(brush, pen, new Rect(new Point(-Offset.X - 1,-Offset.Y - 1),Extent));
            context.Pop();
            context.DrawText(new Point(10.0, 0.0), $"Offset:{{{Offset.X},{Offset.Y}}}");
            context.DrawText(new Point(10.0,10.0), $"Viewport:{{{Viewport.X},{Viewport.Y}}}");
            context.DrawText(new Point( 5.0-Offset.X,Extent.Y-Offset.Y-15.0), $"Extent.Y:{{{Extent.Y}}}");
            var r = new FormattedText($"Extent.X:{{{Extent.X}}}", CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,new Typeface("Segoe UI"),10.0, Brushes.Gray);
            context.PushTransform(new RotateTransform(-90.0,-Offset.X + Extent.X - 15.0,-Offset.Y + r.Width + 5.0));
            context.DrawText(new Point(-Offset.X + Extent.X - 15.0,-Offset.Y + r.Width + 5.0), r);
            context.Pop();
            }
        #endregion
        #region M:MeasureOverride(Size):Size
        /// <summary>When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement"/>-derived class.</summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize) {
            var rc = Rect.Empty;
            var i = 0;
            foreach (UIElement e in InternalChildren) {
                if (e != null) {
                    e.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    var α = GetBounds(availableSize,e);
                    if (i == 0)
                        {
                        rc = α;
                        }
                    else
                        {
                        rc.Union(α);
                        }
                    ++i;
                    }
                }
            rc.Union(new Point(0.0,0.0));
            rc = DoubleUtil.Round(rc);
            Extent   = new Vector(rc.Width, rc.Height);
            Viewport = new Vector(availableSize.Width,availableSize.Height);
            return rc.Size;
            }
        #endregion
        #region M:ArrangeOverride(Size):Size
        /// <summary>When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement"/> derived class.</summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize) {
            foreach (UIElement e in InternalChildren) {
                if (e != null) {
                    var α = DoubleUtil.Round(GetBounds(finalSize, e));
                    α.Offset(-Offset.X,-Offset.Y);
                    e.Arrange(α);
                    }
                }
            return finalSize;
            }
        #endregion
        #region M:GetBounds(Size,UIElement):Rect
        private static Rect GetBounds(Size availablesize, UIElement e)
            {
            var size = e.DesiredSize;
            var α = new Point(XYViewport.GetLeft(e),XYViewport.GetTop(e));
            var β = new Point();
            if (!IsNaN(α.X)) {
                β.X = α.X + size.Width;
                }
            else
                {
                β.X = XYViewport.GetRight(e);
                if (!IsNaN(β.X)) {
                    α.X = availablesize.Width - size.Width - β.X;
                    }
                else
                    {
                    α.X = 0.0;
                    β.X = size.Width;
                    }
                }
            if (!IsNaN(α.Y)) {
                β.Y = α.Y + size.Height;
                }
            else
                {
                β.Y = XYViewport.GetBottom(e);
                if (!IsNaN(β.Y)) {
                    α.Y = availablesize.Height - size.Height - β.Y;
                    }
                else
                    {
                    α.Y = 0.0;
                    β.Y = size.Height;
                    }
                }
            return new Rect(α,β);
            }
        #endregion
        }
    }