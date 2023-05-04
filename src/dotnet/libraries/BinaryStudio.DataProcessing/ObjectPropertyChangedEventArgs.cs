using System;
#if UseWPF
using System.Windows;
#endif

namespace BinaryStudio.DataProcessing
    {
    public class ObjectPropertyChangedEventArgs<T> : EventArgs
        {
        public T OldValue { get; }
        public T NewValue { get; }
        public ObjectPropertyChangedEventArgs(T o, T n) {
            OldValue = o;
            NewValue = n;
            }

        #if UseWPF
        public ObjectPropertyChangedEventArgs(DependencyPropertyChangedEventArgs e) {
            OldValue = (T)e.OldValue;
            NewValue = (T)e.NewValue;
            }
        #endif
        }
    }