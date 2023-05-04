
#include "hdrstop.h"
#include "module.h"
#include "object.h"
#include "codeview.h"

#if defined(_WIN32) || defined(_WIN64)
BOOL APIENTRY DllMain(HMODULE Module,DWORD ul_reason_for_call,LPVOID) {
    switch (ul_reason_for_call) {
        case DLL_PROCESS_ATTACH: return Module::ProcessAttach(Module);
        case DLL_PROCESS_DETACH: return Module::ProcessDetach(Module);
        case DLL_THREAD_ATTACH:  return Module::ThreadAttach(Module);
        case DLL_THREAD_DETACH:  return Module::ThreadDetach(Module);
        }
    return TRUE;
    }
#endif

static const GUID IID_ICommonObjectFileSource = __uuidof(ICommonObjectFileSource);

#pragma warning(push)
#pragma warning(disable: 28252)
STDAPI DllGetClassObject(_In_ REFCLSID rclsid, _In_ REFIID riid, _Outptr_ LPVOID* r)
    {
    if (r == nullptr) { return E_INVALIDARG; }
    if (rclsid == IID_ICommonObjectFileSource) { return ComPtrM<ICommonObjectFileSource,CommonObjectFileSource>(__EFILESRC__)->QueryInterface(riid,r); }
    return E_NOINTERFACE;
    }
#pragma warning(pop)
