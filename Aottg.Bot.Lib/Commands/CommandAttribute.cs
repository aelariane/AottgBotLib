using System;

namespace AottgBotLib.Commands
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CommandAttribute : Attribute
    {
        public string CommandName { get; }

        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}
