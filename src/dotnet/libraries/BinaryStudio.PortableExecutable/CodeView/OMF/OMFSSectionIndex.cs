namespace BinaryStudio.PortableExecutable
    {
    /// <summary>SubSection Types.</summary>
    public enum OMFSSectionIndex : short
        {
        Module      = 0x0120,
        Types       = 0x0121,
        Public      = 0x0122,
        PublicSym   = 0x0123,
        Symbols     = 0x0124,
        AlignSym    = 0x0125,
        SrcLnSeg    = 0x0126,
        SrcModule   = 0x0127,
        Libraries   = 0x0128,
        GlobalSym   = 0x0129,
        GlobalPub   = 0x012a,
        GlobalTypes = 0x012b,
        MPC         = 0x012c,
        SegMap      = 0x012d,
        SegName     = 0x012e,
        PreComp     = 0x012f,
        Names       = 0x0130,
        FileIndex   = 0x0133,
        StaticSym   = 0x0134
        }
    }