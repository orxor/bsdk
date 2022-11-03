#pragma once

#include <string>
#include <unordered_set>

using namespace std;

class ObjectSource
    {
public:
    ObjectSource(const string& FileName, int Line);
    ObjectSource(const ObjectSource& Other);
public:
    string FileName;
    int Line;
    };

template<class... T> class Object;
template<> class Object<IUnknown> :
    public virtual IUnknown
    {
protected:
    Object(const ObjectSource& ObjectSource);
public:
    virtual ~Object();
public:
    HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid,void** r) override;
    ULONG STDMETHODCALLTYPE AddRef()  override;
    ULONG STDMETHODCALLTYPE Release() override;
protected:
    Object(const Object&) = default;
    Object(Object&&)      = default;
    Object& operator=(const Object&) = default;
    Object& operator=(Object&&)      = default;
public:
    ObjectSource Source;
    bool Disposing,Disposed;
    LONG ReferenceCount;
public:
    virtual string TypeName() const;
protected:
    #ifdef FEATURE_FREE_THREADED_MARSHALER
    ComPtr<IUnknown> Marshaler;
    virtual HRESULT EnsureMarshaler() {
        _ASSERT(this != nullptr);
        _ASSERT(!Disposed);
        if (Marshaler == nullptr) {
            HRESULT hr;
            if ((hr = CoCreateFreeThreadedMarshaler(this, &Marshaler)) != S_OK) { return hr; }
            }
        return S_OK;
        }
    #endif
public:
    static HANDLE Mutex;
    static unordered_set<Object<IUnknown>*> Instances;
public:
    };

