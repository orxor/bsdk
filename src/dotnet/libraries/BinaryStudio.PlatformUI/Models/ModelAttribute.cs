using System;

namespace BinaryStudio.PlatformUI.Models
    {
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
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