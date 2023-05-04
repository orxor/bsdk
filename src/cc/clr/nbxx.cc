#include "hdrstop.h"
#include "omf.h"

#undef  DEFAULT
#define DEFAULT(E) \
CodeViewDirectoryFactory##E::CodeViewDirectoryFactory##E(const ObjectSource& ObjectSource): \
    OMFDirectoryFactory(ObjectSource) \
    { \
    }

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
