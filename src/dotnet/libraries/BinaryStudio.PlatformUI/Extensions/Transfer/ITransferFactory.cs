using System;
using System.Windows;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    internal interface ITransferFactory
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        void CopyTo(DependencyObject Source,DependencyObject Target);

        /// <summary>Transfers data context.</summary>
        /// <param name="Target">Target object.</param>
        /// <param name="DataContext">Data context.</param>
        void TransferDataContext(DependencyObject Target,Object DataContext);
        }
    }