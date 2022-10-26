namespace BinaryStudio.PortableExecutable
    {
    /// <summary>Subsection Types.</summary>
    public enum TD32SubsectionType : short
        {
        SUBSECTION_TYPE_MODULE         = 0x0120,
        SUBSECTION_TYPE_TYPES          = 0x0121,
        SUBSECTION_TYPE_SYMBOLS        = 0x0124,
        SUBSECTION_TYPE_ALIGN_SYMBOLS  = 0x0125,
        SUBSECTION_TYPE_SOURCE_MODULE  = 0x0127,
        SUBSECTION_TYPE_GLOBAL_SYMBOLS = 0x0129,
        SUBSECTION_TYPE_GLOBAL_TYPES   = 0x012B,
        SUBSECTION_TYPE_NAMES          = 0x0130
        }
    }