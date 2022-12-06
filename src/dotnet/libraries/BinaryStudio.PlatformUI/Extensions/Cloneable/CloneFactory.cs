using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Interactivity;
using BinaryStudio.PlatformUI.Controls;
using BinaryStudio.PlatformUI.Documents;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    using TriggerBase=System.Windows.Interactivity.TriggerBase;
    public abstract class CloneFactory
        {
        private static void CopyTo(ContextMenu Source,ContextMenu Target,FrameworkContentElement Host)
            {
            }

        private static void ApplyBindings(DependencyObject source, DependencyProperty property) {
            if ((source != null) && (property != null)) {
                var B = BindingOperations.GetBindingBase(source,property);
                if (B != null) {
                    var E = BindingOperations.GetBindingExpressionBase(source,property);
                    if (E != null) {
                        E.UpdateTarget();
                        }
                    }
                }
            }

        private static void ApplyBindings(BlockUIContainer Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(List Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(Figure Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(Floater Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(InlineUIContainer Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(LineBreak Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(Run Source) {
            if (Source == null) { return; }
            ApplyBindings(Source,Run.TextProperty);
            }

        private static void ApplyBindings(Span Source) {
            if (Source == null) { return; }
            }

        private static void ApplyBindings(Inline Source) {
            if (Source == null) { return; }
            ApplyBindings(Source as Figure);
            ApplyBindings(Source as Floater);
            ApplyBindings(Source as InlineUIContainer);
            ApplyBindings(Source as LineBreak);
            ApplyBindings(Source as Run);
            ApplyBindings(Source as Span);
            }

        private static void ApplyBindings(Paragraph Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var Inline in Source.Inlines.ToArray()) {
                Inline.DataContext = Inline.DataContext ?? DataContext;
                ApplyBindings(Inline);
                }
            }

        private static void ApplyBindings(Section Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var Block in Source.Blocks.ToArray()) {
                Block.DataContext = Block.DataContext ?? DataContext;
                ApplyBindings(Block);
                }
            }

        private static void ApplyBindings(TableRowGroup Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var SourceRow in Source.Rows.ToArray()) {
                SourceRow.DataContext = SourceRow.DataContext ?? DataContext;
                ApplyBindings(SourceRow);
                }
            }

        private static void ApplyBindings(Table Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var SourceRowGroup in Source.RowGroups.ToArray()) {
                SourceRowGroup.DataContext = SourceRowGroup.DataContext ?? DataContext;
                ApplyBindings(SourceRowGroup);
                }
            }

        private static void ApplyBindings(Block Source) {
            if (Source == null) { return; }
            ApplyBindings(Source as BlockUIContainer);
            ApplyBindings(Source as List);
            ApplyBindings(Source as Paragraph);
            ApplyBindings(Source as Section);
            ApplyBindings(Source as Table);
            }

        private static void ApplyBindings(TableCell Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var Block in Source.Blocks.ToArray()) {
                Block.DataContext = Block.DataContext ?? DataContext;
                ApplyBindings(Block);
                }
            }

        public static void ApplyBindings(TableRow Source) {
            if (Source == null) { return; }
            var DataContext = Source.DataContext;
            foreach (var Cell in Source.Cells.ToArray()) {
                Cell.DataContext = Cell.DataContext ?? DataContext;
                ApplyBindings(Cell);
                }
            }

        private static BindingBase Clone(BindingBase Source) {
            //if (Source != null) {
            //    if (Source is Binding Binding) {
            //        var mi = typeof(BindingBase).GetMethod("Clone",BindingFlags.Instance|BindingFlags.NonPublic);
            //        return (BindingBase)mi.Invoke(Source,new Object[]{ Binding.Mode });
            //        }
            //    }
            return Source;
            }

        #region M:CopyTo(DependencyObject,DependencyObject,DependencyProperty)
        public static void CopyTo(DependencyObject Source,DependencyObject Target,DependencyProperty Property)
            {
            if (Source == null) { return; }
            var TargetValue = Target.GetValue(Property);
            var SourceValue = Source.GetValue(Property);
            var TargetMetadata = Property.GetMetadata(Target);
            var e = BindingOperations.GetBindingBase(Source,Property);
            if (e != null) {
                BindingOperations.SetBinding(Target,Property,Clone(e));
                }
            else
                {
                Target.SetValue(Property,SourceValue);
                }
            }
        #endregion
        #region M:CopyTriggers(DependencyObject,DependencyObject)
        protected static void CopyTriggers(DependencyObject Source,DependencyObject Target) {
            if (Source == null) { return; }
            var SourceTriggers = Interaction.GetTriggers(Source);
            var TargetTriggers = Interaction.GetTriggers(Target);
            TargetTriggers.Clear();
            foreach (var SourceTrigger in SourceTriggers) {
                var TargetTrigger = (TriggerBase)SourceTrigger.Clone();
                TargetTriggers.Add(TargetTrigger);
                }
            }
        #endregion

        private static UIElement Clone(UIElement Source, FrameworkContentElement Host)
            {
            throw new NotImplementedException();
            }

        //private static T Load<T>(FrameworkContentElement Source, T Target)
        //    where T: FrameworkContentElement
        //    {
        //    if (Target != null) {
        //        if (!Target.IsLoaded) {
        //            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
        //            Target.RaiseEvent(new RoutedEventArgs(FrameworkContentElement.LoadedEvent));
        //            }
        //        }
        //    return Target;
        //    }

        private static void ApplyStyle<T>(T Target,FrameworkContentElement Host)
            where T: FrameworkContentElement
            {
            if (Host != null) {
                if (Host.TryFindResource(typeof(T)) is Style Style) {
                    Target.Style = Style;
                    return;
                    }
                if (Host.TryFindResource(Target.GetType()) is Style Style2) {
                    Target.Style = Style2;
                    return;
                    }
                }
            }

        #region M:GetFactory(Type):ICloneFactory
        internal static ICloneFactory GetFactory(Type Type) {
            if (Type != null) {
                if (!Factories.TryGetValue(Type, out var Factory)) {
                    foreach (var i in Factories) {
                        if (Type.IsSubclassOf(i.Key)) {
                            Factory = i.Value;
                            break;
                            }
                        }
                    }
                Debug.Print($"Factory[{{{Type.Name}}}]:{{{Factory.GetType().Name}}}");
                return Factory;
                }
            return null;
            }
        #endregion

        public T Clone<T>(T Source)
            where T : DependencyObject
            {
            if (Source != null) {
                var Target = (T)Activator.CreateInstance(Source.GetType());
                var Factory = GetFactory(Source.GetType());
                if (Factory == null) { throw new NotSupportedException(); }
                Factory.CopyTo(Source,Target);
                return Target;
                }
            return null;
            }

        private static readonly IDictionary<Type,ICloneFactory> Factories = new Dictionary<Type, ICloneFactory>();
        static CloneFactory()
            {
            foreach (var Type in typeof(CloneFactory).Assembly.GetTypes()) {
                foreach (var Attribute in Type.GetCustomAttributes(false).OfType<CloneFactoryAttribute>()) {
                    Factories.Add(Attribute.Type,(ICloneFactory)Activator.CreateInstance(Type));
                    }
                }
            }
        }

    internal abstract class CloneFactory<T> : CloneFactory,ICloneFactory
        where T : DependencyObject
        {
        #region M:CopyTo(T,T)
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected abstract void CopyTo(T Source,T Target);
        #endregion
        #region M:CopyTo(DependencyObject,DependencyObject,DependencyProperty)
        public static void CopyTo(DependencyObject Source,DependencyObject Target,DependencyProperty Property)
            {
            if (Source == null) { return; }
            var TargetValue = Target.GetValue(Property);
            var SourceValue = Source.GetValue(Property);
            var TargetMetadata = Property.GetMetadata(Target);
            var e = BindingOperations.GetBindingBase(Source,Property);
            if (e != null) {
                BindingOperations.SetBinding(Target,Property,e);
                }
            else
                {
                Target.SetValue(Property,SourceValue);
                }
            }
        #endregion

        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        void ICloneFactory.CopyTo(DependencyObject Source,DependencyObject Target)
            {
            CopyTo((T)Source,(T)Target);
            }
        }
    }