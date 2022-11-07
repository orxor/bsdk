#pragma once
#include "object.h"

enum class OMFDirectorySignature
    {
    NB00 = 0x3030424e,
    NB01 = 0x3130424e,
    NB02 = 0x3230424e,
    NB03 = 0x3330424e,
    NB04 = 0x3430424e,
    NB05 = 0x3530424e,
    NB06 = 0x3630424e,
    NB07 = 0x3730424e,
    NB08 = 0x3830424e,
    NB09 = 0x3930424e,
    NB10 = 0x3031424e,
    FB09 = 0x39304246,
    FB0A = 0x41304246,
    RSDS = 0x53445352
    };

#pragma pack(push)
#pragma pack(1)
struct OMFDirectorySignatureHeader
    {
    OMFDirectorySignature Signature;
    LONG32 Offset;
    };
#pragma pack(pop)

class OMFDirectory : public Object<IUnknown>
    {
protected:
    OMFDirectory(const ObjectSource& ObjectSource,LPBYTE BaseAddress, LPBYTE BegOfDebugData,LPBYTE EndOfDebugData);
    };

#define DEFAULT(E) \
class CodeViewDirectory##E : public OMFDirectory \
    { \
public: \
    CodeViewDirectory##E(const ObjectSource& ObjectSource,LPBYTE BaseAddress, LPBYTE BegOfDebugData,LPBYTE EndOfDebugData); \
    };

DEFAULT(NB00)
DEFAULT(NB01)
DEFAULT(NB02)
DEFAULT(NB03)
DEFAULT(NB04)
DEFAULT(NB05)
DEFAULT(NB06)
DEFAULT(NB07)
DEFAULT(NB08)
DEFAULT(NB09)
DEFAULT(NB10)
DEFAULT(RSDS)