#pragma once
#include "omf.h"

#pragma pack(push)
#pragma pack(1)
struct TD32ModuleInfo
    {
    SHORT OverlayNumber;  // Overlay number
    SHORT LibraryIndex;   // Index into sstLibraries subsection if this module was linked from a library
    SHORT SegmentCount;   // Count of the number of code segments this module contributes to
    SHORT DebuggingStyle; // Debugging style  for this  module.
    LONG32 NameIndex;     // Name index of module.
    LONG32 TimeStamp;     // Time stamp from the OBJ file.
private:
    BYTE Reserved[12];
    };
#pragma pack(pop)

class TD32SSectionModule final : public OMFSSection
    {
public:
    TD32SSectionModule(const ObjectSource& ObjectSource):
        OMFSSection(ObjectSource)
        {
        }
protected:
    STDMETHOD(Load)(LPBYTE BaseAddress, LPBYTE Source,LONG32 Size) override;
    STDMETHOD(Load)(const TD32ModuleInfo* Source);
    };

class TD32DirectoryFactoryFB09 : public OMFDirectoryFactory
    {
public:
    TD32DirectoryFactoryFB09(const ObjectSource& ObjectSource);
protected:
    shared_ptr<OMFSSection> CreateSection(OMFSSectionIndex Index) override;
    };

class TD32DirectoryFactoryFB0A : public TD32DirectoryFactoryFB09
    {
public:
    TD32DirectoryFactoryFB0A(const ObjectSource& ObjectSource);
    };
