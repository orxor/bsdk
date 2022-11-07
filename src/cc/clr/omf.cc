#include "hdrstop.h"
#include "omf.h"

OMFDirectory::OMFDirectory(const ObjectSource& ObjectSource, LPBYTE BaseAddress, LPBYTE BegOfDebugData, LPBYTE EndOfDebugData):
    Object<IUnknown>(ObjectSource)
    {
    }
