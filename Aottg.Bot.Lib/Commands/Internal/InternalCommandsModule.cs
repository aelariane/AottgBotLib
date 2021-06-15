using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace AottgBotLib.Commands.Internal
{
    internal class InternalCommandsModule : CommandModule
    {
        [Command("help")]
        [Description("Gets descriptions and list of commands")]
        public void HelpCommand(CommandContext ctx)
        {
            if(ctx.Arguments.Length == 0)
            {
                StringBuilder bld = new StringBuilder();

                bld.Append($"Type {ctx.Client.CommandHandler.Prefix}help *command* to get description of command.\nList of commands:");

                foreach(var cmd in ctx.Client.CommandHandler.AllCommands)
                {
                    bld.Append($"\n{cmd.LowerName}");
                }

                ctx.Client.SendChatMessage(bld.ToString(), ctx.Sender);
            }
            else
            {
                var cmd = ctx.Client.CommandHandler.AllCommands.FirstOrDefault(c => c.LowerName == ctx.Arguments[0].ToLower());

                object[] args = new object[] { string.Empty, ctx.Arguments[0].ToLower() };

                if(cmd == null)
                {
                    args[0] = "No such command found";
                }
                else if(cmd.Description == null)
                {
                    args[0] = "There is no description yet ;(";
                }
                else
                {
                    args[0] = cmd.Description;
                }
                ctx.Client.SendRPC(2, "Chat", args, ctx.Sender.ActorNumber);
            }
        }
    }
}
