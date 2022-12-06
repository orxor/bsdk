using System.Windows.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(FlowDocument))]
    internal class CloneFlowDocumentFactory : CloneFrameworkContentElementFactory<FlowDocument>
        {
        }
    }