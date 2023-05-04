#pragma once
#include "object.h"

enum class OMFDirectorySignature
    {
    NB00 = 0x3030424e,
    NB01 = 0x3130424e,
    NB02 = 0x3230424e,
    NB03 = 0x3330424e,
    NB04 = 0x3430424e,
    NB05 = 0x3530424e,
    NB06 = 0x3630424e,
    NB07 = 0x3730424e,
    NB08 = 0x3830424e,
    NB09 = 0x3930424e,
    NB10 = 0x3031424e,
    FB09 = 0x39304246,
    FB0A = 0x41304246,
    RSDS = 0x53445352
    };

enum class OMFSSectionIndex : short
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
    };

#pragma pack(push)
#pragma pack(1)
struct OMFDirectorySignatureHeader
    {
    OMFDirectorySignature Signature;
    LONG32 Offset;
    };

/// <summary>
/// The subsection directory is prefixed with a directory header structure
/// indicating size and number of subsection directory entries that follow.
/// </summary>
struct CodeViewSubsectionDirectoryHeader
    {
    SHORT  Size;           // Length of this structure
    SHORT  DirEntrySize;   // Length of each directory entry
    LONG32 DirEntryCount;  // Number of directory entries
    LONG32 lfoNextDir;     // Offset from lfoBase of next directory.
private:
    DWORD32 Flags;
    };

/// <summary>
/// Subsection directory header structure.
/// The directory header structure is followed by the directory entries
/// which specify the subsection type, module index, file offset, and size.
/// The subsection directory gives the location (LFO) and size of each subsection,
/// as well as its type and module number if applicable.
/// </summary>
struct CodeViewSubsectionDirectoryEntry
    {
    OMFSSectionIndex SDirectoryIndex; // Subdirectory type
    SHORT  ModuleIndex;               // Module index
    LONG32 Offset;                    // Offset from the base offset lfoBase
    LONG32 Size;                      // Number of bytes in subsection
    };

#pragma pack(pop)

class OMFSSection : public Object<IUnknown>
    {
public:
    SHORT ModuleIndex;
    LONG32 Offset;
    LONG64 FileOffset;
    LONG32 Size;
protected:
    OMFSSection(const ObjectSource& ObjectSource);
protected:
    STDMETHOD(Load)(LPBYTE BaseAddress, LPBYTE Source,LONG32 Size) = 0;
    friend class OMFDirectoryFactory;
    };

class OMFDirectoryFactory : public Object<IUnknown>
    {
public:
    map<OMFSSectionIndex,shared_ptr<OMFSSection>> Sections;
protected:
    OMFDirectoryFactory(const ObjectSource& ObjectSource);
    virtual shared_ptr<OMFSSection> CreateSection(OMFSSectionIndex Index);
public:
    STDMETHOD(Load)(LPBYTE BaseAddress, LPBYTE BegOfDebugData,LPBYTE EndOfDebugData);
    };

#define DEFAULT(E) \
class CodeViewDirectoryFactory##E : public OMFDirectoryFactory \
    { \
public: \
    CodeViewDirectoryFactory##E(const ObjectSource& ObjectSource); \
    };

DEFAULT(NB00)
DEFAULT(NB01)
DEFAULT(NB02)
DEFAULT(NB03)
DEFAULT(NB04)
DEFAULT(NB05)
DEFAULT(NB06)
DEFAULT(NB07)
DEFAULT(NB08)
DEFAULT(NB09)
DEFAULT(NB10)
DEFAULT(RSDS)

class OMFSSectionModule : public OMFSSection
    {
public:
    OMFSSectionModule(const ObjectSource& ObjectSource):
        OMFSSection(ObjectSource)
        {
        }
protected:
    STDMETHOD(Load)(LPBYTE BaseAddress, LPBYTE Source,LONG32 Size) override;
    };

