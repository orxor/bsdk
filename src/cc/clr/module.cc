#include "hdrstop.h"
#include "module.h" 

BOOL Module::ProcessAttach(const HMODULE Module)
    {
    if (Mutex == nullptr) {
        Mutex = CreateMutex(nullptr,FALSE,nullptr);
        }
    ThreadAttach(Module);
    return TRUE;
    }

BOOL Module::ProcessDetach(const HMODULE Module)
    {
    ThreadDetach(Module);
    CloseHandle(Mutex);
    Mutex = nullptr;
    return TRUE;
    }

BOOL Module::ThreadAttach(const HMODULE Module)
    {
    return TRUE;
    }

BOOL Module::ThreadDetach(const HMODULE Module)
    {
    return TRUE;
    }
