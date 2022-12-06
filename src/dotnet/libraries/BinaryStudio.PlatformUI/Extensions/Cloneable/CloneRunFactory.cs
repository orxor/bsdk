using System.Windows;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Run))]
    internal class CloneRunFactory : CloneInlineFactory<Run>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(Run Source, Run Target)
            {
            if (Source == null) { return; }
            Target.Text = Source.Text;
            base.CopyTo(Source, Target);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            CopyTo(Source,Target,Run.TextProperty);
            }
        }
    }