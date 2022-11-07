#pragma once
#include "object.h"

EXTERN_C const IID IID_IMetadataObject;
MIDL_INTERFACE("c37594e2-bbc1-4e28-8ca8-aada25f74634")
IMetadataObject : IUnknown
    {
    };

EXTERN_C const IID IID_ICommonObjectFileSource;
MIDL_INTERFACE("2f857d78-03a7-40fe-9500-cf2d2f235b4c")
ICommonObjectFileSource : IUnknown
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

class CommonObjectFileSource : public MetadataObject<ICommonObjectFileSource>
    {
public:
    explicit CommonObjectFileSource(const ObjectSource& ObjectSource);
    };
