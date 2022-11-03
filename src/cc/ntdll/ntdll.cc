#include "ntdll.h"

EXTERN_C LONG InterlockedIncrement(LONG volatile *Addend)
    {
    return ++*Addend;
    }
