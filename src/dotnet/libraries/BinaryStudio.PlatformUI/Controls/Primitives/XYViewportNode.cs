using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    public class XYViewportNode : ContentControl
        {
        static XYViewportNode()
            {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XYViewportNode), new FrameworkPropertyMetadata(typeof(XYViewportNode)));
            }

        public XYViewportNode()
            {
            }

        #region P:IsSelected:Boolean
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(XYViewportNode), (PropertyMetadata) new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(OnIsSelectedChanged)));
        private static void OnIsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            if (sender is XYViewportNode source) {

                }
            }

        public Boolean IsSelected
            {
            get { return (Boolean)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
            }
        #endregion

        #region M:OnMouseLeftButtonDown(MouseButtonEventArgs)
        /// <summary>Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> routed event is raised on this element. Implement this method to add class handling for this event. </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            Diagnostics.Print((new StackTrace()).GetFrame(0).GetMethod());
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) {
                var Selected = !IsSelected;
                if (Selected)
                    {
                    Owner.SelectItem(this);
                    }
                else
                    {
                    Owner.UnselectItem(this);
                    }
                }
            else
                {
                Owner.UnselectAll();
                Owner.SelectItem(this);
                }
            e.Handled = true;
            }
        #endregion

        internal XYViewport Owner;
        }
    }
