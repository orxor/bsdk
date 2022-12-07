using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Run))]
    internal class TransferRunFactory : TransferInlineFactory<Run>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(Run Source, Run Target)
            {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            using (new DebugScope()) {
                CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
                CopyTo(Source,Target,Run.TextProperty);
                //Debug.Print("Target{{{1}}}.Text:{0}", (Target.Text != null) ? $@"""{Target.Text}""" : "null", Diagnostics.GetKey(Target));
                }
            }
        }
    }