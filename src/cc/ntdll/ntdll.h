#pragma once

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

EXTERN_C LONG InterlockedIncrement(LONG volatile *Addend);