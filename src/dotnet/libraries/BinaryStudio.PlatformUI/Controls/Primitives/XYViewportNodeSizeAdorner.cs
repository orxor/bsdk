using System;
using System.Windows;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    internal class XYViewportNodeSizeAdorner : AdornerContainer<XYViewportNodeSizeDecorator>
        {
        public XYViewportNodeSizeAdorner(UIElement adornedElement)
            : base(adornedElement)
            {
            }

        #region P:Child:XYViewportNodeSizeDecorator
        public override XYViewportNodeSizeDecorator Child {
            get
                {
                base.Child = base.Child ?? new XYViewportNodeSizeDecorator();
                return base.Child;
                }
            set { throw new NotSupportedException(); }
            }
        #endregion
        }
    }