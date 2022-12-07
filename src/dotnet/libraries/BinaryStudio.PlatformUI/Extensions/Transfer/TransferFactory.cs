using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    using TriggerBase = System.Windows.Interactivity.TriggerBase;
    public abstract class TransferFactory
        {
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

        protected static void ApplyStyle<T>(T Target,FrameworkContentElement Host)
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

        protected static void ApplyStyle<T>(T Target,FrameworkElement Host)
            where T: DependencyObject
            {
            if (Host != null) {
                if (Host.TryFindResource(Target.GetType()) is Style Style) {
                    Target.SetValue(FrameworkElement.StyleProperty,Style);
                    }
                }
            }

        #region M:GetFactory(Type):ITransferFactory
        internal static ITransferFactory GetFactory(Type Type) {
            if (Type != null) {
                if (!Factories.TryGetValue(Type, out var Factory)) {
                    foreach (var i in Factories) {
                        if (Type.IsSubclassOf(i.Key)) {
                            Factory = i.Value;
                            break;
                            }
                        }
                    }
                #if DEBUG_CLONE
                Debug.Print($"Factory[{{{Type.Name}}}]:{{{Factory.GetType().Name}}}");
                #endif
                return Factory;
                }
            return null;
            }
        #endregion
        #region M:GetFactory(Object):ITransferFactory
        internal static ITransferFactory GetFactory(Object Source) {
            return (Source != null)
                ? GetFactory(Source.GetType())
                : null;
            }
        #endregion
        #region M:Clone<T>(T):T
        public static T Clone<T>(T Source)
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
        #endregion
        #region M:Update(DependencyObject,DependencyProperty)
        public static void Update(DependencyObject source,DependencyProperty property) {
            if ((source != null) && (property != null)) {
                var binding = BindingOperations.GetBindingBase(source,property);
                if (binding != null) {
                    var expression = BindingOperations.GetBindingExpressionBase(source,property);
                    if (expression != null) {
                        expression.UpdateTarget();
                        }
                    }
                }
            }
        #endregion

        /// <summary>Transfers data context.</summary>
        /// <param name="Target">Target object.</param>
        /// <param name="DataContext">Data context.</param>
        public static void TransferDataContext(DependencyObject Target, Object DataContext) {
            var Factory = GetFactory(Target.GetType());
            if (Factory == null) { throw new NotSupportedException(); }
            Factory.TransferDataContext(Target,DataContext);
            }

        private static readonly IDictionary<Type,ITransferFactory> Factories = new Dictionary<Type, ITransferFactory>();
        static TransferFactory()
            {
            foreach (var Type in typeof(TransferFactory).Assembly.GetTypes()) {
                foreach (var Attribute in Type.GetCustomAttributes(false).OfType<CloneFactoryAttribute>()) {
                    Factories.Add(Attribute.Type,(ITransferFactory)Activator.CreateInstance(Type));
                    }
                }
            }
        }

    internal abstract class TransferFactory<T> : TransferFactory,ITransferFactory
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
            var SourceValue = Source.GetValue(Property);
            var binding = BindingOperations.GetBindingBase(Source,Property);
            if (binding != null) {
                BindingOperations.SetBinding(Target,Property,binding);
                var e = BindingOperations.GetBindingExpressionBase(Target,Property);
                if (e != null) {
                    e.UpdateTarget();
                    //if (e.Status != BindingStatus.Active) {
                    //    Task.Factory.StartNew(()=>{
                    //        for (;;) {
                    //            var status = e.Status;
                    //            switch (status) {
                    //                case BindingStatus.UpdateTargetError : { return; }
                    //                case BindingStatus.Active:
                    //                    {
                    //                    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.DataBind,new Action(()=>{
                    //                        e.UpdateTarget();
                    //                        }));
                    //                    return;
                    //                    }
                    //                }
                    //            Debug.Print($"Binding:{Target.GetType().Name}.{Property}:{status}");
                    //            Thread.Yield();
                    //            Thread.Sleep(1000);
                    //            }
                    //        });
                    //    }
                    //else
                    //    {
                    //    return;
                    //    }
                    }
                }
            else
                {
                Target.SetValue(Property,SourceValue);
                }
            }
        #endregion

        /// <summary>Transfers data context.</summary>
        /// <param name="Target">Target object.</param>
        /// <param name="DataContext">Data context.</param>
        protected abstract void TransferDataContext(T Target, Object DataContext);

        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        void ITransferFactory.CopyTo(DependencyObject Source,DependencyObject Target) {
            #if DEBUG
            if ((Source != null) && (Target != null)) {
                Debug.Print("Transfer:{{{1}}}->{{{3}}}:{{{0}}}->{{{2}}}",
                    Source.GetType().Name, Diagnostics.GetKey(Source),
                    Target.GetType().Name, Diagnostics.GetKey(Target));
                }
            #endif
            using (new DebugScope()) {
                CopyTo((T)Source,(T)Target);
                }
            }

        /// <summary>Transfers data context.</summary>
        /// <param name="Target">Target object.</param>
        /// <param name="DataContext">Data context.</param>
        void ITransferFactory.TransferDataContext(DependencyObject Target, Object DataContext) {
            using (new DebugScope()) {
                TransferDataContext((T)Target,DataContext);
                }
            }
        }
    }