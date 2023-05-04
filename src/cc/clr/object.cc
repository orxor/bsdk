#include "hdrstop.h"
#include "object.h"

unordered_set<Object<IUnknown>*> Object<IUnknown>::Instances;
HANDLE Object<IUnknown>::Mutex = nullptr;

ObjectSource::ObjectSource(const ObjectSource& Other):
    FileName(Other.FileName),Line(Other.Line)
    {
    }

ObjectSource::ObjectSource(const string& FileName, const int Line):
    FileName(FileName),Line(Line)
    {
    }

string Object<IUnknown>::TypeName() const { return "Object<IUnknown>"; }

ULONG STDMETHODCALLTYPE Object<IUnknown>::AddRef()
    {
    _ASSERT(this != nullptr);
    _ASSERT(!Disposed);
    const auto r = InterlockedIncrement(&ReferenceCount);
    ATLTRACE(L"%S::AddRef{%p}:%i\n", TypeName().c_str(), this, r);
    return r;
    }

ULONG STDMETHODCALLTYPE Object<IUnknown>::Release() {
    _ASSERT(this != nullptr);
    _ASSERT(!Disposed);
    #ifdef FEATURE_FREE_THREADED_MARSHALER
    if (Disposing) { return 0; }
    #endif
    if (ReferenceCount != 0) {
        const auto r = InterlockedDecrement(&ReferenceCount);
        #ifdef TRACE_OBJECT_RELEASE
        TRACE(L"%S::Release{%p}:%i\n", TypeName().c_str(), this, r);
        #endif
        if (r == 0) {
            Disposing = true;
            try
                {
                #ifdef FEATURE_FREE_THREADED_MARSHALER
                Marshaler = nullptr;
                #endif
                delete this;
                }
            catch (...)
                {
                }
            return 0;
            }
        return r;
        }
    return 0;
    }

HRESULT STDMETHODCALLTYPE Object<IUnknown>::QueryInterface(REFIID riid, _COM_Outptr_ void __RPC_FAR *__RPC_FAR *r) {
    _ASSERT(this != nullptr);
    _ASSERT(!Disposed);
    if (r == nullptr) { return E_INVALIDARG; }
    *r = nullptr;
    if (riid == IID_IUnknown) {
        const auto i = static_cast<IUnknown*>(this);
        *r = i;
        AddRef();
        return S_OK;
        };
    #ifdef FEATURE_FREE_THREADED_MARSHALER
    if (riid == IID_IMarshal) {
        if (EnsureMarshaler() == S_OK) {
            Marshaler->QueryInterface(riid, r);
            return S_OK;
            }
        };
    #endif
    return E_NOINTERFACE;
    }

Object<IUnknown>::Object(const ObjectSource& Source):
    Source(Source),Disposing(false),Disposed(false)
    {
    MutexLock lock(Mutex);
    Instances.insert(this);
    }

Object<IUnknown>::~Object()
    {
    MutexLock lock(Mutex);
    ReferenceCount = 0;
    Disposed = true;
    Instances.erase(this);
    #ifdef TRACE_OBJECT_FINALIZE
    TRACE(L"Object<IUnknown>::Finalize{%p}\n", this);
    #endif
    }
