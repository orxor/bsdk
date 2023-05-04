using System;
using System.Windows.Input;

namespace BinaryStudio.PlatformUI.Extensions
    {
    public class DelegateCommand : ICommand
        {
        private readonly Action<Object> Execute;
        private readonly Func<Object,Boolean> CanExecute;

        public DelegateCommand(Action<Object> Execute, Func<Object,Boolean> CanExecute = null) {
            this.CanExecute = CanExecute;
            this.Execute = Execute;
            }

        void ICommand.Execute(Object parameter) {
            if (Execute != null) {
                Execute(parameter);
                }
            }

        Boolean ICommand.CanExecute(Object parameter) {
            return (CanExecute != null)
                ? CanExecute(parameter)
                : true;
            }
    
        public event EventHandler CanExecuteChanged;
        }
    }