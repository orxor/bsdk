using System;
using System.Linq;
using System.Windows;
using BinaryStudio.PlatformUI.Controls;

namespace BinaryStudio.PlatformUI
    {
    internal abstract class NameResolver
        {
        #region P:Name:String
        /// <summary>Gets or sets the name of the element to attempt to resolve.</summary>
        /// <value>The name to attempt to resolve.</value>
        public String Name {
            get { return name; }
            set
                {
                var o = Object;
                name = value;
                UpdateObjectFromName(o);
                }
            }
        private String name;
        #endregion

        /// <summary>Occurs when the resolved element has changed.</summary>
        public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;
        /// <summary>Gets or sets a value indicating whether the reference element load is pending.</summary>
        /// <value><c>True</c> if [pending reference element load]; otherwise, <c>False</c>.</value>
        /// <remarks>
        /// If the Host has not been loaded, the name will not be resolved.
        /// In that case, delay the resolution and track that fact with this property.
        /// </remarks>
        protected Boolean PendingReferenceElementLoad { get; set; }
        protected Boolean HasAttempedResolve { get; set; }
        protected DependencyObject ResolvedObject { get;set; }
        public abstract DependencyObject Object { get; }
        protected abstract void UpdateObjectFromName(DependencyObject oldObject);

        #region M:OnObjectChanged(DependencyObject,DependencyObject)
        protected void OnObjectChanged(DependencyObject o, DependencyObject n) {
            ResolvedElementChanged?.Invoke(this, new NameResolvedEventArgs(o, n));
            }
        #endregion

        public static NameResolver Create(DependencyObject source, String name) {
            if (source is FrameworkElement FrameworkElement) { return new NameResolverFE(FrameworkElement,name); }
            if (source is FrameworkContentElement FrameworkContentElement) { return new NameResolverFCE(FrameworkContentElement,name); }
            throw new ArgumentOutOfRangeException(nameof(source));
            }
        }

    internal abstract class NameResolver<T> : NameResolver
        where T: DependencyObject
        {
        private T nsre;
        protected abstract void OnNameScopeReferenceElementChanged(T o);
        protected abstract T GetActualNameScopeReference(T source);
        protected abstract Boolean IsNameScope(T source);

        #region P:NameScopeReferenceElement:FrameworkElement
        /// <summary>Gets or sets the reference element from which to perform the name resolution.</summary>
        /// <value>The reference element.</value>
        public T NameScopeReferenceElement
            {
            get { return nsre; }
            set
                {
                var o = NameScopeReferenceElement;
                nsre = value;
                OnNameScopeReferenceElementChanged(o);
                }
            }
        #endregion
        #region P:Object:DependencyObject
        /// <summary>
        /// The resolved object. Will return the reference element if TargetName is null or empty, or if a resolve has not been attempted.
        /// </summary>
        public override DependencyObject Object { get {
            return (String.IsNullOrEmpty(Name) && HasAttempedResolve)
                    ? NameScopeReferenceElement
                    : ResolvedObject;
            }}
        #endregion

        protected NameResolver(T source, String name)
            {
            Name = name;
            NameScopeReferenceElement = source;
            }
        }

    internal sealed class NameResolverFE : NameResolver<FrameworkElement>
        {
        public NameResolverFE(FrameworkElement source, String name)
            : base(source, name)
            {
            }

        #region P:ActualNameScopeReferenceElement:FrameworkElement
        private FrameworkElement ActualNameScopeReferenceElement { get {
            return (NameScopeReferenceElement == null || !NameScopeReferenceElement.IsLoaded)
                    ? null
                    : GetActualNameScopeReference(NameScopeReferenceElement);
            }}
        #endregion

        #region M:UpdateObjectFromName(DependencyObject)
        /// <summary>Attempts to update the resolved object from the name within the context of the namescope reference element.</summary>
        /// <param name="o">The old resolved object.</param>
        /// <remarks>
        /// Resets the existing target and attempts to resolve the current TargetName from the
        /// context of the current Host. If it cannot resolve from the context of the Host, it will
        /// continue up the visual tree until it resolves. If it has not resolved it when it reaches
        /// the root, it will set the Target to null and write a warning message to Debug output.
        /// </remarks>
        protected override void UpdateObjectFromName(DependencyObject o) {
            DependencyObject resolvedObject = null;
            ResolvedObject = null;
            if (NameScopeReferenceElement != null) {
                if (!NameScopeReferenceElement.IsLoaded) {
                    NameScopeReferenceElement.Loaded += OnNameScopeReferenceLoaded;
                    PendingReferenceElementLoad = true;
                    return;
                    }
                if (!String.IsNullOrEmpty(Name)) {
                    var e = ActualNameScopeReferenceElement;
                    if (e != null) {
                        resolvedObject = (e.FindName(Name) as DependencyObject);
                        }
                    }
                }
            HasAttempedResolve = true;
            ResolvedObject = resolvedObject;
            if (!Equals(o,Object)) {
                OnObjectChanged(o, Object);
                }
            }
        #endregion
        #region M:GetActualNameScopeReference(FrameworkElement):FrameworkElement
        protected override FrameworkElement GetActualNameScopeReference(FrameworkElement source) {
            var r = source;
            if (IsNameScope(source)) {
                r = ((source.Parent as FrameworkElement) ?? r);
                }
            return r;
            }
        #endregion
        #region M:IsNameScope(FrameworkElement):Boolean
        protected override Boolean IsNameScope(FrameworkElement source) {
            if (source.Parent is FrameworkElement e) {
                var r = e.FindName(Name);
                return r != null;
                }
            return false;
            }
        #endregion
        #region M:OnNameScopeReferenceLoaded(Object,RoutedEventArgs)
        private void OnNameScopeReferenceLoaded(Object sender, RoutedEventArgs e) {
            PendingReferenceElementLoad = false;
            NameScopeReferenceElement.Loaded -= OnNameScopeReferenceLoaded;
            UpdateObjectFromName(Object);
            }
        #endregion
        #region M:OnNameScopeReferenceElementChanged(FrameworkElement)
        protected override void OnNameScopeReferenceElementChanged(FrameworkElement o) {
            if (PendingReferenceElementLoad) {
                o.Loaded -= OnNameScopeReferenceLoaded;
                PendingReferenceElementLoad = false;
                }
            HasAttempedResolve = false;
            UpdateObjectFromName(Object);
            }
        #endregion
        }

    internal sealed class NameResolverFCE : NameResolver<FrameworkContentElement>
        {
        public NameResolverFCE(FrameworkContentElement source, String name)
            : base(source, name)
            {
            }

        #region P:ActualNameScopeReferenceElement:FrameworkContentElement
        private FrameworkContentElement ActualNameScopeReferenceElement { get {
            return (NameScopeReferenceElement != null)
                    ? GetActualNameScopeReference(NameScopeReferenceElement)
                    : null;
            }}
        #endregion

        #region M:OnNameScopeReferenceElementChanged(FrameworkContentElement)
        protected override void OnNameScopeReferenceElementChanged(FrameworkContentElement o) {
            HasAttempedResolve = false;
            UpdateObjectFromName(Object);
            }
        #endregion
        #region M:UpdateObjectFromName(DependencyObject)
        /// <summary>Attempts to update the resolved object from the name within the context of the namescope reference element.</summary>
        /// <param name="o">The old resolved object.</param>
        /// <remarks>
        /// Resets the existing target and attempts to resolve the current TargetName from the
        /// context of the current Host. If it cannot resolve from the context of the Host, it will
        /// continue up the visual tree until it resolves. If it has not resolved it when it reaches
        /// the root, it will set the Target to null and write a warning message to Debug output.
        /// </remarks>
        protected override void UpdateObjectFromName(DependencyObject o) {
            DependencyObject r = null;
            ResolvedObject = null;
            if (NameScopeReferenceElement != null) {
                if (!String.IsNullOrEmpty(Name)) {
                    var e = ActualNameScopeReferenceElement;
                    if (e != null) {
                        r = e.FindName(Name) as DependencyObject;
                        if (r == null) {
                            foreach (var i in e.LogicalDescendants().OfType<FrameworkContentElement>()) {
                                if (String.Equals(i.Name,Name)) {
                                    r = i;
                                    break;
                                    }
                                }
                            }
                        }
                    }
                }
            HasAttempedResolve = true;
            ResolvedObject = r;
            if (!Equals(o,Object)) {
                OnObjectChanged(o, Object);
                }
            }
        #endregion
        #region M:GetActualNameScopeReference(FrameworkContentElement):FrameworkContentElement
        protected override FrameworkContentElement GetActualNameScopeReference(FrameworkContentElement source) {
            var e = source;
            if (IsNameScope(source)) {
                e = ((source.Parent as FrameworkContentElement) ?? e);
                }
            return e;
            }
        #endregion
        #region M:IsNameScope(FrameworkContentElement):Boolean
        protected override Boolean IsNameScope(FrameworkContentElement source) {
            if (source.Parent is FrameworkContentElement e) {
                var r = e.FindName(Name);
                return r != null;
                }
            return false;
            }
        #endregion
        }
    }