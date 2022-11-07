#include "hdrstop.h"
#include "object.h"

MutexLock::MutexLock(HANDLE MutexObject):
    MutexObject(MutexObject)
    {
    if (MutexObject != nullptr) {
        WaitForSingleObject(MutexObject,INFINITE);
        }
    }
MutexLock::~MutexLock() {
    if (MutexObject != nullptr) {
        ReleaseMutex(MutexObject);
        MutexObject = nullptr;
        }
    }
