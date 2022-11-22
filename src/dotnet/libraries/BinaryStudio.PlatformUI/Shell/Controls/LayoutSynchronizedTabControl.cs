using System;
using System.Windows;
using System.Windows.Controls;
using BinaryStudio.PlatformUI.Shell;

namespace BinaryStudio.PlatformUI
    {
    public class LayoutSynchronizedTabControl : TabControl
        {
        static LayoutSynchronizedTabControl()
            {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutSynchronizedTabControl), new FrameworkPropertyMetadata(typeof(LayoutSynchronizedTabControl)));
            }

        /// <summary>Prepares the specified element to display the specified item.</summary>
        /// <param name="element">The element that is used to display the specified item.</param>
        /// <param name="item">The specified item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, Object item)
            {
            base.PrepareContainerForItemOverride(element, item);
            var d = item as DependencyObject;
            if (d != null) {
                ViewManager.BindViewManager(element, d);
                }
            }
        }
    }
