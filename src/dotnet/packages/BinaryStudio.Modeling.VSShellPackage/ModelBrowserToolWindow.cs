using System.Runtime.InteropServices;
using BinaryStudio.VSShellServices;

namespace BinaryStudio.Modeling.VSShellPackage
    {
    [Guid("57f64175-f95b-4e5a-b588-8c212db72594")]
    [ToolWindowCommand(CommandSet = "{fdc929a3-8dfd-4def-b927-43ab6a8b7f8b}", CommandId = 0x0100)]
    public sealed class ModelBrowserToolWindow : ToolWindow<ModelBrowserControl>
        {
        }
    }