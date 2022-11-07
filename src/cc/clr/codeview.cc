#include "hdrstop.h"
#include "omf.h"
#include "td32.h"
#include "codeview.h"

CommonObjectFileSource::CommonObjectFileSource(const ObjectSource& ObjectSource):
    MetadataObject<ICommonObjectFileSource>(ObjectSource)
    {
    }

HRESULT CommonObjectFileSource::Load(LPSAFEARRAY Source, const LONG64 Size) {
    if (Source == nullptr) { return E_INVALIDARG; }
    if (Size == 0) { return E_INVALIDARG; }
    if (!((Source->fFeatures & FADF_HAVEVARTYPE) && (Source->cDims == 1))) { return E_INVALIDARG; }
    if (Source->rgsabound[0].cElements != 2)     { return E_INVALIDARG; }
    if (Source->cbElements != sizeof(DWORD_PTR)) { return E_INVALIDARG; }
    vector<LPBYTE> Target(Source->rgsabound[0].cElements);
    memcpy(&Target[0],Source->pvData,Source->cbElements*Source->rgsabound[0].cElements);
    return Load(Target,Size);
    }

HRESULT CommonObjectFileSource::Load(const vector<LPBYTE>& Source, const LONG64 Size) {
    if (Source.empty()) { return E_INVALIDARG; }
    if (Size < sizeof(IMAGE_FILE_HEADER)) { return E_INVALIDARG; }
    return Load(Source[0],PIMAGE_FILE_HEADER(Source[1]),Size);
    }

HRESULT CommonObjectFileSource::Load(const LPBYTE BaseAddress, const PIMAGE_FILE_HEADER ImageFileHeader, const LONG64 Size) {
    if (ImageFileHeader == nullptr)      { return E_INVALIDARG; }
    if (BaseAddress == nullptr) { return E_INVALIDARG; }
    if (Size < sizeof(IMAGE_FILE_HEADER)) { return E_INVALIDARG; }
    auto r = (LPBYTE)(ImageFileHeader + 1);
    vector<PIMAGE_DATA_DIRECTORY> Directories;
    if (ImageFileHeader->SizeOfOptionalHeader > 0) {
        const auto Magic = *(WORD*)r;
        switch (Magic) {
            case IMAGE_NT_OPTIONAL_HDR32_MAGIC: { Load(Directories,PIMAGE_OPTIONAL_HEADER32(r)); } break;
            case IMAGE_NT_OPTIONAL_HDR64_MAGIC: { Load(Directories,PIMAGE_OPTIONAL_HEADER64(r)); } break;
            default: return COR_E_NOTSUPPORTED;
            }
        r += ImageFileHeader->SizeOfOptionalHeader;
        }
    const auto Sections = (IMAGE_SECTION_HEADER*)r;
    if (!Directories.empty() && (ImageFileHeader->NumberOfSections > 0)) {
        const auto sz = ImageFileHeader->NumberOfSections;
        list<tuple<PIMAGE_DATA_DIRECTORY,PIMAGE_SECTION_HEADER,WORD>> entries;
        for (size_t i = 0; i < Directories.size(); i++) {
            if (Directories[i]->Size > 0) {
                for (WORD j = 0; j < sz; j++) {
                    if ((Sections[j].VirtualAddress == Directories[i]->VirtualAddress) ||
                        ((Directories[i]->VirtualAddress >= Sections[j].VirtualAddress) &&
                         (Directories[i]->VirtualAddress <  Sections[j].VirtualAddress + Sections[j].SizeOfRawData))) {
                        entries.emplace_back(make_tuple(
                            Directories[i],
                            &Sections[j],(WORD)i));
                        break;
                        }
                    }
                }
            }
        if (!entries.empty()) {
            list<thread> brokers;
            for (auto& e:entries) {
                brokers.emplace_back([&](){
                    Load(BaseAddress,ImageFileHeader,
                        get<0>(e),
                        get<1>(e),
                        get<2>(e));
                    });
                }
            for (auto& broker:brokers)
                {
                broker.join();
                }
            }
        }
    return S_OK;
    }

HRESULT CommonObjectFileSource::Load(const LPBYTE BaseAddress,PIMAGE_FILE_HEADER ImageFileHeader,PIMAGE_DATA_DIRECTORY ImageDataDirectory,PIMAGE_SECTION_HEADER ImageSectionHeader,const WORD Index) {
    const auto VirtualAddress = BaseAddress + (LONG64)ImageSectionHeader->PointerToRawData - (LONG64)ImageSectionHeader->VirtualAddress;
    switch (Index) {
        case IMAGE_DIRECTORY_ENTRY_DEBUG: { return Load(BaseAddress,VirtualAddress,ImageSectionHeader,ImageDataDirectory,(IMAGE_DEBUG_DIRECTORY*)(VirtualAddress + ImageDataDirectory->VirtualAddress)); }
        }
    return S_OK;
    }

HRESULT CommonObjectFileSource::Load(LPBYTE BaseAddress, LPBYTE VirtualAddress, PIMAGE_SECTION_HEADER ImageSectionHeader, PIMAGE_DATA_DIRECTORY ImageDataDirectory, PIMAGE_DEBUG_DIRECTORY ImageDebugDirectory)
    {
    make_shared<CodeViewSection>(__EFILESRC__,BaseAddress,VirtualAddress,ImageSectionHeader,ImageDebugDirectory);
    return S_OK;
    }

CodeViewSection::CodeViewSection(const ObjectSource& ObjectSource, const LPBYTE BaseAddress, const LPBYTE VirtualAddress, PIMAGE_SECTION_HEADER ImageSectionHeader, PIMAGE_DEBUG_DIRECTORY ImageDebugDirectory):
    CommonObjectFileSection(ObjectSource)
    {
    const auto BegOfDebugData = VirtualAddress + ((ImageDebugDirectory->AddressOfRawData == 0) ? ImageDebugDirectory->PointerToRawData : ImageDebugDirectory->AddressOfRawData);
    const auto EndOfDebugData = BegOfDebugData + ImageDebugDirectory->SizeOfData;
    const auto Header = (OMFDirectorySignatureHeader*)BegOfDebugData;
    const auto Signature = Header->Signature;
    shared_ptr<OMFDirectory> Directory;
    switch (Signature) {
        #define SIGNATURE(E) OMFDirectorySignature::E: { Directory = make_shared<CodeViewDirectory##E>(__EFILESRC__,BaseAddress,BegOfDebugData,EndOfDebugData); } break
        case SIGNATURE(NB00);
        case SIGNATURE(NB01);
        case SIGNATURE(NB02);
        case SIGNATURE(NB03);
        case SIGNATURE(NB04);
        case SIGNATURE(NB05);
        case SIGNATURE(NB06);
        case SIGNATURE(NB07);
        case SIGNATURE(NB08);
        case SIGNATURE(NB09);
        case SIGNATURE(NB10);
        case SIGNATURE(RSDS);
        #undef  SIGNATURE
        #define SIGNATURE(E) OMFDirectorySignature::E: { Directory = make_shared<TD32Directory##E>(__EFILESRC__,BaseAddress,BegOfDebugData,EndOfDebugData); } break
        case SIGNATURE(FB09);
        case SIGNATURE(FB0A);
        default: break;
        }
    }
