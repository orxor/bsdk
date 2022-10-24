using System;

namespace BinaryStudio.VSShellServices
    {
    public class ProvideEditorWindowAttribute : Attribute
        {
        public Type EditorWindowType { get;set; }
        public ProvideEditorWindowAttribute(Type type)
            {
            EditorWindowType = type;
            }
        }
    }