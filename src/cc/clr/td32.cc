#include "hdrstop.h"
#include "td32.h"

TD32DirectoryFactoryFB09::TD32DirectoryFactoryFB09(const ObjectSource& ObjectSource):
    OMFDirectoryFactory(ObjectSource)
    {
    }

TD32DirectoryFactoryFB0A::TD32DirectoryFactoryFB0A(const ObjectSource& ObjectSource):
    TD32DirectoryFactoryFB09(ObjectSource)
    {
    }

shared_ptr<OMFSSection> TD32DirectoryFactoryFB09::CreateSection(const OMFSSectionIndex Index)
    {
    shared_ptr<OMFSSection> Target;
    switch (Index) {
        #define INDEX(E) OMFSSectionIndex::E: { Target = make_shared<TD32SSection##E>(__EFILESRC__); } break
        case INDEX(Module);
        default:
            Target = OMFDirectoryFactory::CreateSection(Index);
            break;
        }
    return Target;
    }

HRESULT TD32SSectionModule::Load(LPBYTE,const LPBYTE Source,LONG32)
    {
    return Load((TD32ModuleInfo*)Source);
    }

HRESULT TD32SSectionModule::Load(const TD32ModuleInfo* Source)
    {
    return S_OK;
    }

