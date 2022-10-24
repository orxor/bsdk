using System;
using System.Windows;

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

        public ObjectPropertyChangedEventArgs(DependencyPropertyChangedEventArgs e) {
            OldValue = (T)e.OldValue;
            NewValue = (T)e.NewValue;
            }
        }
    }