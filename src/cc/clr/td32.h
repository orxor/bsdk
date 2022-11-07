#pragma once
#include "omf.h"

class TD32DirectoryFB09 : public OMFDirectory
    {
public:
    TD32DirectoryFB09(const ObjectSource& ObjectSource,LPBYTE BaseAddress,LPBYTE BegOfDebugData,LPBYTE EndOfDebugData);
    };

class TD32DirectoryFB0A : public TD32DirectoryFB09
    {
public:
    TD32DirectoryFB0A(const ObjectSource& ObjectSource,LPBYTE BaseAddress,LPBYTE BegOfDebugData,LPBYTE EndOfDebugData);
    };
