using System;
using System.Collections.Generic;
using System.Text;

using AottgBotLib.Commands.Internal;
using Photon.Realtime;

namespace AottgBotLib.Commands
{
    public sealed class CommandHandler
    {
        public BotClient Client { get; }

        internal CommandHandler(BotClient client)
        {
            Client = client;
            RegisterModule<InternalCommandsModule>();
        }

        internal List<Command> AllCommands { get; } = new List<Command>();

        /// <summary>
        /// Prefix to indicate if Command was triggered
        /// </summary>
        public string Prefix { get; internal set; } = "!";

        /// <summary>
        /// Registers module as Command handle module
        /// </summary>
        /// <typeparam name="T">Module to add</typeparam>
        /// <returns>CommandHandler to make chain calls</returns>
        public CommandHandler RegisterModule<T>() where T : CommandModule, new()
        {
            CommandModule module = Activator.CreateInstance(typeof(T)) as CommandModule;

            IReadOnlyCollection<Command> moduleCommands = module.GetCommands();
            if(moduleCommands.Count > 0)
            {
                AllCommands.AddRange(moduleCommands);
            }

            return this;
        }

        internal void TryExecuteCommand(string fullMessage, Player sender)
        {
            fullMessage = fullMessage.RemoveAll();

            string lowerMessage = fullMessage.ToLower();
            foreach(var cmd in AllCommands)
            {
                string check = (cmd.CustomPrefix ?? Prefix) + cmd.LowerName;
                int index = lowerMessage.IndexOf(check);
                if(index < 0)
                {
                    continue;
                }

                if(cmd.IsMasterClientOnly && Client.LocalPlayer.IsMasterClient == false)
                {
                    //TODO: Add response?
                    break;
                }

                string cut = fullMessage.Substring(index + check.Length).TrimStart().TrimEnd();
                CommandContext ctx = new CommandContext(Client, cut, sender);

                try
                {
                    cmd.Method.Invoke(cmd.Owner, new object[] { ctx });
                    Log.LogInfo($"Executed command ({cmd.Name}) by Player " + sender.ActorNumber + " " + sender.CustomProperties["name"]);
                    break;
                }
                catch(Exception ex)
                {
                    Log.LogError($"Failed exectution of command {cmd} invoked by player {sender.ActorNumber} {sender.CustomProperties["name"]}. \nMessage: {fullMessage}\nException: {ex.Message}\nTrace: {ex.StackTrace}");
                }
            }
        }
    }
}
