#pragma once

#if defined(_WIN32) || defined(_WIN64)
#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files
#include <windows.h>
#include <unknwn.h>
#include <atlbase.h>
#include <atlsafe.h>
#else
#ifdef __cplusplus
    #define EXTERN_C       extern "C"
    #define EXTERN_C_START extern "C" {
    #define EXTERN_C_END   }
#else
    #define EXTERN_C       extern
    #define EXTERN_C_START
    #define EXTERN_C_END
#endif

typedef struct {
    unsigned long  Data1;
    unsigned short Data2;
    unsigned short Data3;
    unsigned char  Data4[ 8 ];
    } GUID;
typedef GUID IID;
typedef long HRESULT;
typedef long LONG;
typedef unsigned long ULONG;
typedef unsigned long DWORD;
typedef void *HANDLE;

#define REFIID const IID &
#define __stdcall

EXTERN_C const IID IID_IUnknown;
#define DECLSPEC_NOVTABLE
#define DECLSPEC_UUID(x)
#define MIDL_INTERFACE(x)   struct DECLSPEC_UUID(x) DECLSPEC_NOVTABLE
#define STDMETHODCALLTYPE   __stdcall
MIDL_INTERFACE("00000000-0000-0000-C000-000000000046")
IUnknown
    {
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid,void** r) = 0;
    virtual ULONG STDMETHODCALLTYPE AddRef()  = 0;
    virtual ULONG STDMETHODCALLTYPE Release() = 0;
    };

LONG InterlockedIncrement(LONG volatile *Addend);

#define ATLTRACE(...)
#define _ASSERT(...)
#endif
