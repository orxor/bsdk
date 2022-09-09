using System;

namespace BinaryStudio.Modeling.VSShellPackage.Models
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