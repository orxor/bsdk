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
    STDMETHOD(QueryInterface)(REFIID riid,void** r) override;
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

template<class... T> class Object<IUnknown,T...>:
    public Object<IUnknown>,
    public virtual T...
    {
protected:
    Object(const ObjectSource& ObjectSource):
        Object<IUnknown>(ObjectSource)
        {
        }
private:
    template<class I, class... T> struct QueryInterfaceT;
    template<class I> struct QueryInterfaceT<I>
        {
        template<class SELF>
        static HRESULT QueryInterface(SELF self,REFIID riid,void** r) {
            if (riid == __uuidof(I)) {
                auto i = static_cast<I*>(self);
                *r = i;
                i->AddRef();
                return S_OK;
                }
            return E_NOINTERFACE;
            }
        };
    template<class I, class... T> struct QueryInterfaceT
        {
        template<class SELF>
        static HRESULT QueryInterface(SELF self,REFIID riid,void** r) {
            return (QueryInterfaceT<I>::QueryInterface(self,riid,r) != S_OK)
                ? QueryInterfaceT<T...>::QueryInterface(self,riid,r)
                : S_OK;
            }
        };
public:
    STDMETHOD(QueryInterface)(REFIID riid,void** r) override {
        if (r == nullptr) { return E_INVALIDARG; }
        *r = nullptr;
        return (Object<IUnknown>::QueryInterface(riid,r) != S_OK)
            ? QueryInterfaceT<T...>::QueryInterface(this,riid,r)
            : S_OK;
        }
    ULONG STDMETHODCALLTYPE AddRef()  override { return Object<IUnknown>::AddRef();  }
    ULONG STDMETHODCALLTYPE Release() override { return Object<IUnknown>::Release(); }
protected:
    Object(const Object&) = default;
    Object(Object&&)      = default;
    Object& operator=(const Object&) = default;
    Object& operator=(Object&&)      = default;
    };

template<class T>
class ComPtr
    {
public:
    ComPtr():
        r(nullptr)
        {
        }
    ComPtr(nullptr_t):
        r(nullptr)
        {
        }
    ComPtr(const T* o):
        r(const_cast<T*>(o))
        {
        AddRef();
        }
    ComPtr(T* o):
        r(o)
        {
        AddRef();
        }
    ComPtr(ComPtr<T>& o):
        r(o.r)
        {
        AddRef();
        }
    ComPtr(ComPtr<T>&& o):
        r(o.r)
        {
        AddRef();
        }
    ~ComPtr()
        {
        Release();
        }
public:
    bool operator == (T* o) const { return r == o; }
    bool operator != (T* o) const { return r != o; }
    T** operator &()       { return &r; }
    T&  operator *() const { return *r; }
    T*  operator->() const { return  r; }
    operator T*()    const { return  r; }
public:
    ComPtr<T>& operator=(nullptr_t) {
        Release();
        r = nullptr;
        return *this;
        }
    ComPtr<T>& operator=(T* o) {
        Release();
        r = o;
        AddRef();
        return *this;
        }
    ComPtr<T>& operator=(const ComPtr<T>& o) {
        Release();
        r = o.r;
        AddRef();
        return *this;
        }
    ComPtr<T>& operator=(ComPtr<T>&& o) {
        Release();
        r = o.r;
        AddRef();
        return *this;
        }
private:
    ULONG AddRef()
        {
        return (r)
            ? r->AddRef()
            : 0;
        }
    ULONG Release()
        {
        if (r)
            {
            const auto o = r;
            r = nullptr;
            return o->Release();
            }
        return 0;
        }
public:
    T* Detach()
        {
        const auto o = r;
        r = nullptr;
        return o;
        }
    void Attach(T* o)
        {
        Release();
        r = o;
        }
public:
    template <class I>
    HRESULT QueryInterface(I** o) const
        {
        return r->QueryInterface(__uuidof(I),(void**)o);
        }
public:
    T* r;
    };

template<class T, class... P> inline ComPtr<T> ComPtrM(P&&... args) {
    auto o = new T(args...);
    auto r = ComPtr<T>(o);
    ((T*)r)->Release();
    return r;
    }

template<class I, class T, class... P> inline ComPtr<I> ComPtrM(P&&... args) {
    auto o = new T(args...);
    auto r = ComPtr<I>(o);
    ((I*)r)->Release();
    return r;
    }

template<class T,class P> inline ComPtr<T> ComPtrA(P& arg) {
    ComPtr<T> r;
    r.Attach(arg);
    return r;
    }

class MutexLock
    {
public:
    explicit MutexLock(HANDLE MutexObject);
    MutexLock(const MutexLock&) = delete;
    ~MutexLock();
private:
private:
    HANDLE MutexObject;
    };


#define COR_E_NULLREFERENCE         0x80004003
#define COR_E_ARGUMENT              0x80070057
#define COR_E_ARGUMENTOUTOFRANGE    0x80131502
#define COR_E_NOTSUPPORTED          0x80131515
#define COR_E_INVALIDOPERATION      0x80131509
#define CORSEC_E_CRYPTO             0x80131430
#define CORSEC_E_CRYPTO_UNEX_OPER   0x80131431
#define CORSEC_E_POLICY_EXCEPTION   0x80131416
#define CORSEC_E_INVALID_STRONGNAME 0x80131415
#define CORSEC_E_XMLSYNTAX          0x80131419
#define COR_E_EXCEPTION             0x80131500
#define COR_E_OVERFLOW              0x80131516

#define __SHORTFILE__   __FILE__
#define __EFUNCSIG__    __FUNCSIG__
#define __EFILESRC__ (::ObjectSource(__SHORTFILE__,__LINE__))
