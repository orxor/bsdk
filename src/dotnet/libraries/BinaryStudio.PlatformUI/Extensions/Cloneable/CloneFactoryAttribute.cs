using System;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    internal class CloneFactoryAttribute : Attribute
        {
        public Type Type { get; }
        public CloneFactoryAttribute(Type type)
            {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            Type = type;
            }
        }
    }