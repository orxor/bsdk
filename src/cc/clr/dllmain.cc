
#include "hdrstop.h"
#include "module.h"

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

