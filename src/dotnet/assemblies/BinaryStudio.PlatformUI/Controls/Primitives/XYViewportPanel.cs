using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    using static Double;
    public class XYViewportPanel : Panel, IScrollInfo
        {
        #region P:Can{Horizontally,Vertically}ScrollComputed:Boolean
        #region P:CanHorizontallyScrollComputed:Boolean
        private static readonly DependencyPropertyKey CanHorizontallyScrollComputedPropertyKey = DependencyProperty.RegisterReadOnly("CanHorizontallyScrollComputed", typeof(Boolean), typeof(XYViewportPanel), new PropertyMetadata(default(Boolean)));
        public static readonly DependencyProperty CanHorizontallyScrollComputedProperty = CanHorizontallyScrollComputedPropertyKey.DependencyProperty;
        public Boolean CanHorizontallyScrollComputed
            {
            get { return (Boolean)GetValue(CanHorizontallyScrollComputedProperty); }
            private set { SetValue(CanHorizontallyScrollComputedPropertyKey, value); }
            }
        #endregion
        #region P:CanVerticallyScrollComputed:Boolean
        private static readonly DependencyPropertyKey CanVerticallyScrollComputedPropertyKey = DependencyProperty.RegisterReadOnly("CanVerticallyScrollComputed", typeof(Boolean), typeof(XYViewportPanel), new PropertyMetadata(default(Boolean)));
        public static readonly DependencyProperty CanVerticallyScrollComputedProperty = CanVerticallyScrollComputedPropertyKey.DependencyProperty;
        public Boolean CanVerticallyScrollComputed
            {
            get { return (Boolean)GetValue(CanVerticallyScrollComputedProperty); }
            private set { SetValue(CanVerticallyScrollComputedPropertyKey, value); }
            }
        #endregion
        #endregion
        #region P:Can{Horizontally,Vertically}Scroll:Boolean
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
        #endregion
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
        #region P:Viewport{Width,Height}:Vector
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
                var offsetN = Offset;
                if (Offset.X > 0) { offsetN = new Vector(Math.Max(offsetO.X,0),offsetN.Y); }
                if (Offset.Y > 0) { offsetN = new Vector(offsetN.X,Math.Max(offsetO.Y,0)); }
                Offset = offsetN;
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
        #region P:Extent{Width,Height}:Vector
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
        #region P:{Horizontal,Vertical}Offset:Vector
        private static readonly DependencyPropertyKey OffsetPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Offset), typeof(Vector), typeof(XYViewportPanel),new PropertyMetadata(default(Vector), OnOffsetChanged, OnOffsetCoerceValue));

        private static Object OnOffsetCoerceValue(DependencyObject sender, Object basevalue) {
            var r = DoubleUtil.Round((Vector)basevalue);
            if (sender is XYViewportPanel source) {
                r.X = source.CanHorizontallyScrollComputed ? r.X : 0.0;
                r.Y = source.CanVerticallyScrollComputed   ? r.Y : 0.0;
                }
            return r;
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
        #region M:IScrollInfo.Line{Up,Down,Left,Right}
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
        #endregion
        #region M:IScrollInfo.Page{Up,Down,Left,Right}
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
        #endregion
        #region M:IScrollInfo.MouseWheel{Up,Down,Left,Right}
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
        #endregion
        #region M:IScrollInfo.Set{Horizontal,Vertical}Offset(Double)
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

        private void EnsureScrollOwner() {
            if (ScrollOwner == null) {
                ScrollOwner = this.FindAncestor<ScrollViewer>();
                }
            }
        #endregion
        #region P:SelectionRectangle:Rect
        internal static readonly DependencyProperty SelectionRectangleProperty = DependencyProperty.Register("SelectionRectangle", typeof(Rect), typeof(XYViewportPanel), new PropertyMetadata(default(Rect), OnSelectionRectangleChanged));
        private static void OnSelectionRectangleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportPanel source) {
                source.OnSelectionRectangleChanged();
                }
            }

        private void OnSelectionRectangleChanged() {
            var Source = SelectionRectangle;
            Debug.Print($"SelectionRectangle:{{{Source}}}");
            if (SelectionRectangleAdorner == null) {
                var layer = AdornerLayer.GetAdornerLayer(this);
                if (layer != null) {
                    SelectionRectangleAdorner = new GeometrySelectionAdorner(this)
                        {
                        Visibility = Visibility.Visible,
                        Width = 0,
                        Height = 0,
                        OffsetX = 0,
                        OffsetY = 0
                        };
                    layer.Add(SelectionRectangleAdorner);
                    }
                }
            if (SelectionRectangleAdorner != null) {
                if (Source.IsEmpty) {
                    SelectionRectangleAdorner.Visibility = Visibility.Hidden;
                    }
                else
                    {
                    Source = FromLogical(Source);
                    SelectionRectangleAdorner.Visibility = Visibility.Visible;
                    SelectionRectangleAdorner.OffsetX = Source.X;
                    SelectionRectangleAdorner.OffsetY = Source.Y;
                    SelectionRectangleAdorner.Width = Source.Width;
                    SelectionRectangleAdorner.Height = Source.Height;
                    }
                }
            }

        internal Rect SelectionRectangle
            {
            get { return (Rect)GetValue(SelectionRectangleProperty); }
            set { SetValue(SelectionRectangleProperty, value); }
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
        #region M:InvalidateScrollInfo
        protected virtual void InvalidateScrollInfo() {
            if (ScrollOwner != null) {
                ScrollOwner.InvalidateScrollInfo();
                InvalidateVisual();
                }
            CanHorizontallyScrollComputed = ExtentWidth  > ViewportWidth;
            CanVerticallyScrollComputed   = ExtentHeight > ViewportHeight;
            }
        #endregion
        #region M:OnRender(DrawingContext)
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
            context.DrawText(new Point(10.0,20.0), $"CanHorizontallyScrollComputed:{{{CanHorizontallyScrollComputed}}}");
            context.DrawText(new Point(10.0,30.0), $"CanVerticallyScrollComputed:{{{CanVerticallyScrollComputed}}}");
            context.DrawText(new Point( 5.0-Offset.X,Extent.Y-Offset.Y-15.0), $"Extent.Y:{{{Extent.Y}}}");
            var r = new FormattedText($"Extent.X:{{{Extent.X}}}", CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,new Typeface("Segoe UI"),10.0, Brushes.Gray);
            context.PushTransform(new RotateTransform(-90.0,-Offset.X + Extent.X - 15.0,-Offset.Y + r.Width + 5.0));
            context.DrawText(new Point(-Offset.X + Extent.X - 15.0,-Offset.Y + r.Width + 5.0), r);
            context.Pop();
            }
        #endregion
        #region M:OnMouseLeftButtonDown(MouseButtonEventArgs)
        /// <summary>Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> routed event is raised on this element. Implement this method to add class handling for this event.</summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            Diagnostic.Print((new StackTrace()).GetFrame(0).GetMethod());
            if (!IsSelecting) {
                IsSelecting = true;
                e.Handled = true;
                CaptureMouse();
                OriginSelectingPoint = ToLogical(e.GetPosition(this));
                }
            base.OnMouseLeftButtonDown(e);
            }
        #endregion
        #region M:OnPreviewMouseLeftButtonDown(MouseButtonEventArgs)
        /// <summary>Invoked when an unhandled <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.</summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
            {
            Diagnostic.Print((new StackTrace()).GetFrame(0).GetMethod());
            base.OnPreviewMouseLeftButtonDown(e);
            }
        #endregion
        #region M:OnMouseMove(MouseEventArgs)
        /// <summary>Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.</summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e) {
            if (IsSelecting) {
                e.Handled = true;
                var position = ToLogical(e.GetPosition(this));
                if (position != OriginSelectingPoint) {
                    var Δ = position - OriginSelectingPoint;
                    UpdateOffset(Δ.X,OriginSelectingPoint.X,position.X,out var X,out var OffsetX);
                    UpdateOffset(Δ.Y,OriginSelectingPoint.Y,position.Y,out var Y,out var OffsetY);
                    SelectionRectangle = new Rect(OffsetX,OffsetY,X,Y);
                    var HitTest = DraggingScrollHitTest();
                    if (HitTest != null) {
                        EnsureScrollOwner();
                        var scrollviewer = ScrollOwner;
                        if (scrollviewer != null) {
                            var offset = HitTest.Offset;
                            if (HitTest.Kind.HasFlag(DraggingScrollHitTestResultKind.Bottom)) { scrollviewer.ScrollToVerticalOffset(Math.Min(scrollviewer.VerticalOffset + offset.Y, ExtentHeight - ViewportHeight*0.5)); }
                            if (HitTest.Kind.HasFlag(DraggingScrollHitTestResultKind.Top))    { scrollviewer.ScrollToVerticalOffset(Math.Max(scrollviewer.VerticalOffset + offset.Y, 0)); }
                            if (HitTest.Kind.HasFlag(DraggingScrollHitTestResultKind.Left))   { scrollviewer.ScrollToHorizontalOffset(Math.Max(scrollviewer.HorizontalOffset + offset.X, 0)); }
                            if (HitTest.Kind.HasFlag(DraggingScrollHitTestResultKind.Right))  { scrollviewer.ScrollToHorizontalOffset(Math.Min(scrollviewer.HorizontalOffset + offset.X, ExtentWidth - ViewportWidth*0.5)); }
                            }
                        }
                    }
                }
            base.OnMouseMove(e);
            }
        #endregion
        #region M:OnMouseLeftButtonUp(MouseButtonEventArgs)
        /// <summary>Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonUp"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.</summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was released.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e) {
            if (IsSelecting) {
                Diagnostic.Print((new StackTrace()).GetFrame(0).GetMethod());
                IsSelecting = false;
                e.Handled = true;
                ReleaseMouseCapture();
                SelectionRectangle = Rect.Empty;
                }
            base.OnMouseLeftButtonUp(e);
            }
        #endregion
        #region M:OnSelectionChanged(SelectionChangedEventArgs)
        /// <summary>Called when the selection changes.</summary>
        /// <param name="e">The event data.</param>
        protected internal virtual void OnSelectionChanged(SelectionChangedEventArgs e) {
            if (e == null) { throw new ArgumentNullException(nameof(e)); }
            //var StaticSelectionAdornerVisibility = Visibility.Visible;
            //XYViewportNodeSizeDecorator SelectionDecorator = null;
            //switch (e.AddedItems.Count) {
            //    case 0:
            //        {
            //        StaticSelectionAdornerVisibility = Visibility.Hidden;
            //        }
            //        break;
            //    case 1:
            //        {
            //        SelectionDecorator = new XYViewportNodeSizeDecorator
            //            {
            //            };
            //        }
            //        break;
            //    default:
            //        {
            //        }
            //        break;
            //    }
            //if (StaticSelectionAdorner == null) {
            //    var layer = AdornerLayer.GetAdornerLayer(this);
            //    if (layer != null) {
            //        layer.Add(StaticSelectionAdorner = new AdornerContainer(this));
            //        }
            //    }
            //if (StaticSelectionAdorner != null) {
            //    StaticSelectionAdorner.Visibility = StaticSelectionAdornerVisibility;
            //    StaticSelectionAdorner.Child = SelectionDecorator;
            //    }
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
        #region M:BringTop(UIElement)
        public void BringTop(UIElement element) {
            if (element != null) {
                var n = InternalChildren.OfType<UIElement>().Max(GetZIndex);
                SetZIndex(element, n + 1);
                }
            }
        #endregion

        private static void UpdateOffset(Double δ,Double o, Double c, out Double α, out Double β) {
            if (δ >= 0)
                {
                α = Math.Max(δ,1.0);
                β = o;
                }
            else
                {
                α = -δ;
                β = c;
                }
            }

        #region M:ToLogical(Point):Point
        private Point ToLogical(Point value)
            {
            return value + Offset;
            }
        #endregion
        #region M:FromLogical(Point):Point
        private Point FromLogical(Point value) {
            return value - Offset;
            }
        #endregion
        #region M:FromLogical(Rect):Rect
        private Rect FromLogical(Rect value) {
            return new Rect(FromLogical(value.TopLeft),value.Size);
            }
        #endregion
        #region M:DraggingScrollHitTest:DraggingScrollHitTestResult
        public DraggingScrollHitTestResult DraggingScrollHitTest()
            {
            var r = DraggingScrollHitTestResultKind.None; 
            EnsureScrollOwner();
            var scrollviewer = ScrollOwner;
            if (scrollviewer != null) {
                var size = new Vector(scrollviewer.ActualWidth, scrollviewer.ActualHeight);
                size -= new Vector(
                    ((scrollviewer.HorizontalScrollBarVisibility == ScrollBarVisibility.Visible) || ((scrollviewer.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto) && (scrollviewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible))) ? SystemParameters.VerticalScrollBarWidth    : 0.0,
                    ((scrollviewer.VerticalScrollBarVisibility   == ScrollBarVisibility.Visible) || ((scrollviewer.VerticalScrollBarVisibility   == ScrollBarVisibility.Auto) && (scrollviewer.ComputedVerticalScrollBarVisibility   == Visibility.Visible))) ? SystemParameters.HorizontalScrollBarHeight : 0.0);
                var tolerance = new Vector(Math.Min(size.X * 0.25, 40), Math.Min(size.Y * 0.25, 40));
                var pt = Mouse.GetPosition(scrollviewer);
                var offset = new Vector(30, 30);
                if (pt.Y < tolerance.Y)
                    {
                    r = DraggingScrollHitTestResultKind.Top;
                    offset.Y *= (tolerance.Y - pt.Y)/tolerance.Y;
                    offset.Y = -offset.Y;
                    }
                else if (pt.Y > size.Y - tolerance.Y)
                    {
                    r = DraggingScrollHitTestResultKind.Bottom;
                    offset.Y *= (tolerance.Y - (size.Y - pt.Y))/tolerance.Y;
                    }
                if (pt.X < tolerance.X)
                    {
                    r |= DraggingScrollHitTestResultKind.Left;
                    offset.X *= (tolerance.X - pt.X)/tolerance.X;
                    offset.X = -offset.X;
                    }
                else if (pt.X > size.X - tolerance.X)
                    {
                    r |= DraggingScrollHitTestResultKind.Right;
                    offset.X *= (tolerance.X - (size.X - pt.X))/tolerance.X;
                    }
                if (r != DraggingScrollHitTestResultKind.None)
                    {
                    return new DraggingScrollHitTestResult(scrollviewer, pt, r, offset);
                    }
                }
            return null;
            }
        #endregion

        private GeometrySelectionAdorner SelectionRectangleAdorner;
        private Boolean IsSelecting;
        private Point OriginSelectingPoint;
        }
    }