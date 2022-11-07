#pragma once

#include "object.h"

class Module final : public Object<IUnknown>
    {
private:
    explicit Module(const ObjectSource& ObjectSource):
        Object<IUnknown>(ObjectSource)
        {
        }
public:
    static BOOL ProcessAttach(HMODULE Module);
    static BOOL ProcessDetach(HMODULE Module);
    static BOOL ThreadAttach(HMODULE Module);
    static BOOL ThreadDetach(HMODULE Module);
    };