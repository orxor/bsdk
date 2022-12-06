using System.Windows;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    internal interface ICloneFactory
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        void CopyTo(DependencyObject Source,DependencyObject Target);
        }
    }