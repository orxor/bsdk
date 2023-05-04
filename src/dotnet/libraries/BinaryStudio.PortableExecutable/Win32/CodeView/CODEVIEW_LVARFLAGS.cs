using System;

namespace BinaryStudio.PortableExecutable.Win32
    {
    [Flags]
    public enum CODEVIEW_LVARFLAGS : short
        {
        IsParam        = 0x0001, // variable is a parameter.
        AddrTaken      = 0x0002, // address is taken.
        CompGenx       = 0x0004, // variable is compiler generated.
        IsAggregate    = 0x0008, // the symbol is splitted in temporaries, which are treated by compiler as independent entities.
        IsAggregated   = 0x0010, // Counterpart of IsAggregate - tells that it is a part of a IsAggregate symbol.
        IsAliased      = 0x0020, // variable has multiple simultaneous lifetimes.
        IsAlias        = 0x0400, // represents one of the multiple simultaneous lifetimes.
        IsRetValue     = 0x0080, // represents a function return value.
        IsOptimizedOut = 0x0100, // variable has no lifetimes.
        IsEnregGlob    = 0x0200, // variable is an enregistered global.
        IsEnregStat    = 0x0400  // variable is an enregistered static.
        }
    }