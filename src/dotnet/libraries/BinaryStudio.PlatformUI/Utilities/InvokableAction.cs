using System;

namespace BinaryStudio.PlatformUI
    {
    internal class InvokableAction : InvokableBase
        {
        private Action a;

        public InvokableAction(Action a, Boolean captureContext = false)
            {
            this.a = a;
            if (!captureContext)
                return;
            }

        protected override void InvokeMethod()
            {
            a();
            }
        }
    }