using System;

namespace BinaryStudio.PlatformUI.Models
    {
    public class ModelAttribute : Attribute
        {
        public Type Type { get; }
        public ModelAttribute(Type type)
            {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            Type = type;
            }
        }
    }