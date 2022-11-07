#pragma once
#include "omf.h"

class TD32DirectoryFactoryFB09 : public OMFDirectoryFactory
    {
public:
    TD32DirectoryFactoryFB09(const ObjectSource& ObjectSource);
    };

class TD32DirectoryFactoryFB0A : public TD32DirectoryFactoryFB09
    {
public:
    TD32DirectoryFactoryFB0A(const ObjectSource& ObjectSource);
    };
