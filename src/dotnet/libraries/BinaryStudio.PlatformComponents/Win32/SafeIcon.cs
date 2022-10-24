using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryStudio.PlatformComponents.Win32
    {
    public class SafeIcon : SafeHandleZeroOrMinusOneIsInvalid
        {
        private SafeIcon() : base(true) {
            }

        public SafeIcon(IntPtr icon, Boolean ownsHandle) : base(ownsHandle) {
            SetHandle(icon);
            }

        protected override Boolean ReleaseHandle() {
            return DestroyIcon(handle);
            }

        [DllImport("user32.dll", SetLastError = true)][return: MarshalAs(UnmanagedType.Bool)] public static extern Boolean DestroyIcon(IntPtr hIcon);
        }
    }