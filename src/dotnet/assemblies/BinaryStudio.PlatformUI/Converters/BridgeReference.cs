using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace BinaryStudio.PlatformUI
    {
    public class BridgeReference : FrameworkElement
        {
        public BridgeReference()
            {
            //Controls.Extensions.GetActualWidth(this);
            }

        #region P:Source:Object
        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(Object), typeof(BridgeReference), new FrameworkPropertyMetadata(default(Object),OnSourceChanged) {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });
        public static void SetSource(DependencyObject element, Object value)
            {
            element.SetValue(SourceProperty, value);
            }

        public static Object GetSource(DependencyObject element)
            {
            return (Object)element.GetValue(SourceProperty);
            }

        private static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            Debug.Print($"{{{sender.GetType().Name}}}.Source:{{{e.NewValue}}}");
            if (sender is FrameworkElement source) {
                var binding = source.GetBindingExpression(TargetProperty);
                if (binding != null) {
                    sender.SetCurrentValue(TargetProperty,e.NewValue);
                    binding.UpdateSource();
                    return;
                    }
                }
            }

        public Object Source {
            get { return (Object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
            }
        #endregion
        #region P:Target:Object
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached("Target", typeof(Object), typeof(BridgeReference), new FrameworkPropertyMetadata(default(Object), OnTargetChanged){
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            
            });
        private static void OnTargetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            Debug.Print($"{{{sender.GetType().Name}}}.Target:{{{e.NewValue}}}");
            (sender as BridgeReference)?.OnTargetChanged();
            }

        protected virtual void OnTargetChanged() {
            return;
            }

        public Object Target {
            get { return (Object)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
            }

        public static void SetTarget(DependencyObject element, Object value)
            {
            element.SetValue(TargetProperty, value);
            }

        public static Object GetTarget(DependencyObject element)
            {
            return (Object)element.GetValue(TargetProperty);
            }
        #endregion
        }
    }