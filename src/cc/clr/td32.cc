#include "hdrstop.h"
#include "td32.h"

TD32DirectoryFB09::TD32DirectoryFB09(const ObjectSource& ObjectSource, LPBYTE BaseAddress, LPBYTE BegOfDebugData, LPBYTE EndOfDebugData):
    OMFDirectory(ObjectSource,BaseAddress,BegOfDebugData,EndOfDebugData)
    {
    }

TD32DirectoryFB0A::TD32DirectoryFB0A(const ObjectSource& ObjectSource, const LPBYTE BaseAddress, const LPBYTE BegOfDebugData, const LPBYTE EndOfDebugData):
    TD32DirectoryFB09(ObjectSource,BaseAddress,BegOfDebugData,EndOfDebugData)
    {
    }

