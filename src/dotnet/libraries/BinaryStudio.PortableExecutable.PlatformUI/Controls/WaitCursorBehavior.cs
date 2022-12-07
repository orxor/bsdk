using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace BinaryStudio.PortableExecutable.PlatformUI.Controls
    {
    internal class WaitCursorBehavior : Behavior<DependencyObject>
        {
        /// <summary>Called after the behavior is attached to an AssociatedObject.</summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached() {
            Mouse.OverrideCursor = Cursors.Wait;
            #if NET40
            AssociatedObject.Dispatcher.BeginInvoke(new Action(() =>
                {
                Mouse.OverrideCursor = null;
                }), DispatcherPriority.ApplicationIdle);
            #else
            AssociatedObject.Dispatcher.InvokeAsync(() =>
                {
                Mouse.OverrideCursor = null;
                }, DispatcherPriority.ApplicationIdle);
            #endif
            base.OnAttached();
            }
        }
    }