using System;

namespace BinaryStudio.PlatformUI
    {
    internal class InvokableFunction<TResult> : InvokableBase
        {
        private Func<TResult> fn;

        public TResult Result { get; private set; }

        public InvokableFunction(Func<TResult> fn)
            {
            this.fn = fn;
            }

        protected override void InvokeMethod()
            {
            Result = fn();
            }
        }
    }