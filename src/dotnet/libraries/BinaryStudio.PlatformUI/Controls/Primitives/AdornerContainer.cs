using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    public class AdornerContainer<T> : Adorner
        where T: UIElement
        {
        #region P:Child:T
        private T child;
        public virtual T Child {
            get
                {
                return child;
                }
            set
                {
                AddVisualChild(value);
                child = value;
                }
            }
        #endregion
        #region P:VisualChildrenCount:Int32
        /// <summary>Gets the number of visual child elements within this element.</summary>
        /// <returns>The number of visual child elements for this element.</returns>
        protected override Int32 VisualChildrenCount { get {
            return (child != null)
                ? 1
                : 0;
            }}
        #endregion

        public AdornerContainer(UIElement adornedElement)
            : base(adornedElement)
            {
            }

        #region M:ArrangeOverride(Size):Size
        /// <summary>When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement"/> derived class.</summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize) {
            if (child != null) {
                child.Arrange(new Rect(finalSize));
                }
            return finalSize;
            }
        #endregion
        #region M:GetVisualChild(Int32):Visual
        /// <summary>Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)"/>, and returns a child at the specified index from a collection of child elements.</summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>The requested child element. This should not return <see langword="null"/>; if the provided index is out of range, an exception is thrown.</returns>
        protected override Visual GetVisualChild(Int32 index) {
            return ((index == 0) && (child != null))
                ? child
                : base.GetVisualChild(index);
            }
        #endregion
        }

    public class AdornerContainer : AdornerContainer<UIElement>
        {
        public AdornerContainer(UIElement adornedElement)
            : base(adornedElement)
            {
            }
        }
    }