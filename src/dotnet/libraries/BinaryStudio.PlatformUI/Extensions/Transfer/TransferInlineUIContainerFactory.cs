﻿using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(InlineUIContainer))]
    internal class TransferInlineUIContainerFactory : TransferInlineFactory<InlineUIContainer>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(InlineUIContainer Source, InlineUIContainer Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Target.Child = Clone(Source.Child);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                }
            }
        }
    }