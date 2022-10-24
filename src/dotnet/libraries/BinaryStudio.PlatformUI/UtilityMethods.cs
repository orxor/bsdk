using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace BinaryStudio.PlatformUI
    {
    public static class UtilityMethods
        {
        private static Point lastContentPos = new Point(Double.NaN, Double.NaN);
        private static Point currentFloatPos = new Point(Double.NaN, Double.NaN);
        private const Double FloatStepX = 20.0;
        private const Double FloatStepY = 20.0;
        private const Int32 PositionRetries = 100;

        internal static void AddPresentationSourceCleanupAction(UIElement element, Action handler)
            {
            var relayHandler = (SourceChangedEventHandler)null;
            relayHandler = (sender, args) =>
            {
                if (args.NewSource != null)
                    return;
                if (!element.Dispatcher.HasShutdownStarted)
                    handler();
                PresentationSource.RemoveSourceChangedHandler(element, relayHandler);
            };
            PresentationSource.AddSourceChangedHandler(element, relayHandler);
            }

        public static void HitTestVisibleElements(Visual visual, HitTestResultCallback resultCallback, HitTestParameters parameters)
            {
            VisualTreeHelper.HitTest(visual, ExcludeNonVisualElements, resultCallback, parameters);
            }

        private static HitTestFilterBehavior ExcludeNonVisualElements(DependencyObject potentialHitTestTarget)
            {
            if (!(potentialHitTestTarget is Visual))
                return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
            var uiElement = potentialHitTestTarget as UIElement;
            return uiElement == null || uiElement.IsVisible && uiElement.IsEnabled ? HitTestFilterBehavior.Continue : HitTestFilterBehavior.ContinueSkipSelfAndChildren;
            }

        internal static Boolean ModifyStyle(IntPtr hWnd, Int32 styleToRemove, Int32 styleToAdd)
            {
            var windowLong = NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL.STYLE);
            var dwNewLong = windowLong & ~styleToRemove | styleToAdd;
            if (dwNewLong == windowLong)
                return false;
            NativeMethods.SetWindowLong(hWnd, NativeMethods.GWL.STYLE, dwNewLong);
            return true;
            }

        internal static HwndSource FindTopLevelHwndSource(UIElement element)
            {
            var hwndSource = (HwndSource)PresentationSource.FromVisual(element);
            if (hwndSource != null && IsChildWindow(hwndSource.Handle))
                hwndSource = HwndSource.FromHwnd(FindTopLevelWindow(hwndSource.Handle));
            return hwndSource;
            }

        internal static Boolean IsChildWindow(IntPtr hWnd)
            {
            return (NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL.STYLE) & 1073741824) == 1073741824;
            }

        internal static IntPtr FindTopLevelWindow(IntPtr hWnd)
            {
            while (hWnd != IntPtr.Zero && IsChildWindow(hWnd))
                hWnd = NativeMethods.GetParent(hWnd);
            return hWnd;
            }
        }
    }