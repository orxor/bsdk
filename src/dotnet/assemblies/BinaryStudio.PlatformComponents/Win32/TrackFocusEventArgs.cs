using System;

namespace BinaryStudio.PlatformComponents.Win32
    {
    public class TrackFocusEventArgs : EventArgs {
        public IntPtr HwndGainFocus { get; set; }
        public IntPtr HwndLoseFocus { get; set; }

        public TrackFocusEventArgs(IntPtr hwndGainFocus, IntPtr hwndLoseFocus) {
            HwndGainFocus = hwndGainFocus;
            HwndLoseFocus = hwndLoseFocus;
            }
        }
    }