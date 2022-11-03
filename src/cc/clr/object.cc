#include "hdrstop.h"
#include "object.h"

ULONG STDMETHODCALLTYPE Object<IUnknown>::AddRef()
    {
    _ASSERT(this != nullptr);
    _ASSERT(!Disposed);
    const auto r = InterlockedIncrement(&ReferenceCount);
    ATLTRACE(L"%S::AddRef{%p}:%i\n", TypeName().c_str(), this, r);
    return r;
    }
