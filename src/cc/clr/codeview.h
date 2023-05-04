#pragma once
#include "object.h"
#include "omf.h"
#include <vector>

EXTERN_C const IID IID_IMetadataObject;
MIDL_INTERFACE("c37594e2-bbc1-4e28-8ca8-aada25f74634")
IMetadataObject : IUnknown
    {
    };

EXTERN_C const IID IID_ICommonObjectFileSource;
MIDL_INTERFACE("2f857d78-03a7-40fe-9500-cf2d2f235b4c")
ICommonObjectFileSource : IUnknown
    {
    STDMETHOD(Load)(LPSAFEARRAY Source,LONG64 Size) = 0;
    };

EXTERN_C const IID IID_ICommonObjectFileSection;
MIDL_INTERFACE("bda3443c-d7b0-41dc-9de6-392e41fc4e73")
ICommonObjectFileSection : IUnknown
    {
    };

EXTERN_C const IID IID_ICodeViewSection;
MIDL_INTERFACE("4abe8307-7157-445e-9504-9b87976f9ea0")
ICodeViewSection : IUnknown
    {
    };

template<class... T> 
class MetadataObject : public Object<IUnknown,IMetadataObject,T...>
    {
protected:
    explicit MetadataObject(const ObjectSource& ObjectSource):
        Object<IUnknown,IMetadataObject,T...>(ObjectSource)
        {
        }
    };

class CommonObjectFileSection : public Object<IUnknown,ICommonObjectFileSection>
    {
protected:
    explicit CommonObjectFileSection(const ObjectSource& ObjectSource):
        Object<IUnknown,ICommonObjectFileSection>(ObjectSource)
        {
        }
    };

class CodeViewSection :
    public CommonObjectFileSection
    {
public:

public:
    explicit CodeViewSection(const ObjectSource& ObjectSource,LPBYTE BaseAddress, LPBYTE VirtualAddress, PIMAGE_SECTION_HEADER ImageSectionHeader, PIMAGE_DEBUG_DIRECTORY ImageDebugDirectory);
protected:
    };

class CommonObjectFileSource : public MetadataObject<ICommonObjectFileSource>
    {
public:
    explicit CommonObjectFileSource(const ObjectSource& ObjectSource);
public:
    STDMETHOD(Load)(LPSAFEARRAY Source,LONG64 Size);
private:
    STDMETHOD(Load)(const vector<LPBYTE>& Source,LONG64 Size);
    STDMETHOD(Load)(LPBYTE BaseAddress,PIMAGE_FILE_HEADER ImageFileHeader,LONG64 Size);
    template<class T> static void Load(vector<PIMAGE_DATA_DIRECTORY>& Target,const T* Source) {
        for (DWORD i = 0; i < Source->NumberOfRvaAndSizes; i++) {
            Target.emplace_back(PIMAGE_DATA_DIRECTORY(&(Source->DataDirectory[i])));
            }
        }
    STDMETHOD(Load)(LPBYTE BaseAddress,PIMAGE_FILE_HEADER ImageFileHeader,PIMAGE_DATA_DIRECTORY,PIMAGE_SECTION_HEADER,WORD);
    STDMETHOD(Load)(LPBYTE BaseAddress,LPBYTE VirtualAddress,PIMAGE_SECTION_HEADER ImageSectionHeader,PIMAGE_DATA_DIRECTORY ImageDataDirectory,PIMAGE_DEBUG_DIRECTORY ImageDebugDirectory);
    };

