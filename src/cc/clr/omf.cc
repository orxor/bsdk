#include "hdrstop.h"
#include "object.h"
#include "omf.h"

OMFDirectoryFactory::OMFDirectoryFactory(const ObjectSource& ObjectSource):
    Object<IUnknown>(ObjectSource)
    {
    }

HRESULT OMFDirectoryFactory::Load(const LPBYTE BaseAddress, const LPBYTE BegOfDebugData, const LPBYTE EndOfDebugData) {
    const auto Signature = (OMFDirectorySignatureHeader*)BegOfDebugData;
    const auto Header  = (CodeViewSubsectionDirectoryHeader*)(BegOfDebugData + Signature->Offset);
    const auto Entries = (CodeViewSubsectionDirectoryEntry*)(Header + 1);
    for (auto i = 0; i < Header->DirEntryCount; i++) {
        auto Section = CreateSection(Entries[i].SDirectoryIndex);
        Section->FileOffset = BegOfDebugData + Entries[i].Offset - BaseAddress;
        Section->ModuleIndex = Entries[i].ModuleIndex;
        Section->Offset = Entries[i].Offset;
        Section->Size   = Entries[i].Size;
        Sections[Entries[i].SDirectoryIndex] = Section;
        Section->Load(BaseAddress,BegOfDebugData + Entries[i].Offset,Entries[i].Size);
        }
    return S_OK;
    }

OMFSSection::OMFSSection(const ObjectSource& ObjectSource):
    Object<IUnknown>(ObjectSource),ModuleIndex(0),
    Offset(0),FileOffset(0),Size(0)
    {
    }

HRESULT OMFSSectionModule::Load(LPBYTE BaseAddress, LPBYTE Source, LONG32 Size)
    {
    return S_OK;
    }

#undef  DEFAULT
#define DEFAULT(E) \
class OMFSSection##E : public OMFSSection \
    { \
public: \
    OMFSSection##E(const ObjectSource& ObjectSource): \
        OMFSSection(ObjectSource) \
        { \
        } \
protected: \
    STDMETHOD(Load)(LPBYTE BaseAddress, LPBYTE Source,LONG32 Size) override \
        { \
        return S_OK; \
        } \
    }

DEFAULT(Types);
DEFAULT(Public);
DEFAULT(PublicSym);
DEFAULT(Symbols);
DEFAULT(AlignSym);
DEFAULT(SrcLnSeg);
DEFAULT(SrcModule);
DEFAULT(Libraries);
DEFAULT(GlobalSym);
DEFAULT(GlobalPub);
DEFAULT(GlobalTypes);
DEFAULT(MPC);
DEFAULT(SegMap);
DEFAULT(SegName);
DEFAULT(PreComp);
DEFAULT(Names);
DEFAULT(FileIndex);
DEFAULT(StaticSym);

shared_ptr<OMFSSection> OMFDirectoryFactory::CreateSection(const OMFSSectionIndex Index) {
    shared_ptr<OMFSSection> Target;
    switch (Index) {
        #define INDEX(E) OMFSSectionIndex::E: { Target = make_shared<OMFSSection##E>(__EFILESRC__); } break;
        case INDEX(Module);
        case INDEX(Types);
        case INDEX(Public);
        case INDEX(PublicSym);
        case INDEX(Symbols);
        case INDEX(AlignSym);
        case INDEX(SrcLnSeg);
        case INDEX(SrcModule);
        case INDEX(Libraries);
        case INDEX(GlobalSym);
        case INDEX(GlobalPub);
        case INDEX(GlobalTypes);
        case INDEX(MPC);
        case INDEX(SegMap);
        case INDEX(SegName);
        case INDEX(PreComp);
        case INDEX(Names);
        case INDEX(FileIndex);
        case INDEX(StaticSym);
        }
    return Target;
    }
