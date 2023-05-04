using System;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    [Flags]
    public enum DraggingScrollHitTestResultKind
        {
        None   = 0,
        Left   = 1,
        Top    = 2,
        Right  = 4,
        Bottom = 8
        }
    }