using System;
using System.Collections;

namespace BinaryStudio.Modeling.UnifiedModelingLanguage.Internal
    {
    public abstract class ENamedElement : EElement, NamedElement
        {
        public String Name { get;set; }
        public Namespace Namespace { get; }
        public String QualifiedName { get; }
        public Dependency[] ClientDependency { get; }
        public StringExpression NameExpression { get; }

        protected ENamedElement(String identifier)
            : base(identifier)
            {
            }

        VisibilityKind? NamedElement.Visibility { get;set; }

        }
    }