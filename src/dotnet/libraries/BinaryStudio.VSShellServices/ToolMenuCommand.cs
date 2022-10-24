using System;
using System.ComponentModel.Design;

namespace BinaryStudio.VSShellServices
    {
    public class ToolMenuCommand : MenuCommand
        {
        public Type ToolWindowType { get; }
        public ToolMenuCommand(EventHandler handler, CommandID command, Type toolWindowType)
            : base(handler, command)
            {
            ToolWindowType = toolWindowType;
            }
        }
    }
