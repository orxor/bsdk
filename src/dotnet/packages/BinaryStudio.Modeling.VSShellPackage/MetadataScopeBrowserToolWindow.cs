﻿using System.Runtime.InteropServices;
using BinaryStudio.VSShellServices;
using Microsoft.VisualStudio.Shell;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    [Guid("b48b966f-ca8f-4f44-9c4e-495aa8993905")]
    [ToolWindowCommand(CommandSet = "{fdc929a3-8dfd-4def-b927-43ab6a8b7f8b}", CommandId = 0x0100)]
    public class MetadataScopeBrowserToolWindow : ToolWindow<ModelBrowserControl>
        {
        }
    }