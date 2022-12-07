using System.Windows.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(LineBreak))]
    internal class TransferLineBreakFactory : TransferInlineFactory<LineBreak>
        {
        }
    }