using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AottgBotLib.Commands.Internal;

namespace AottgBotLib.Commands
{
    /// <summary>
    /// Base class for Commands module
    /// </summary>
    public abstract class CommandModule
    {

        internal IReadOnlyCollection<Command> GetCommands()
        {
            MethodInfo[] methods = GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            List<Command> result = new List<Command>();

            foreach(MethodInfo method in methods)
            {
                CommandAttribute[] cmdAttributes = method.GetCustomAttributes<CommandAttribute>().ToArray();

                if(cmdAttributes.Length < 1)
                {
                    continue;
                }

                DescriptionAttribute description = method.GetCustomAttribute<DescriptionAttribute>();
                MasterClientCommandAttribute isMc = method.GetCustomAttribute<MasterClientCommandAttribute>();
                PrefixAttribute customPrefix = method.GetCustomAttribute<PrefixAttribute>();

                foreach(CommandAttribute cmd in cmdAttributes)
                {
                    Command newCommand = new Command()
                    {
                        Method = method,
                        Name = cmd.CommandName,
                        Owner = this
                    };

                    if(description != null)
                    {
                        newCommand.Description = description.Description;
                    }

                    if(isMc != null)
                    {
                        newCommand.IsMasterClientOnly = true;
                    }

                    if(customPrefix != null)
                    {
                        newCommand.CustomPrefix = customPrefix.Prefix;
                    }

                    result.Add(newCommand);
                }
            }

            return result;
        }
    }
}
