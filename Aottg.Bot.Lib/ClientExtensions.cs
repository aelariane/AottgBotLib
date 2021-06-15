using System;

using AottgBotLib.Commands;
using Photon.Realtime;

namespace AottgBotLib
{
    /// <summary>
    /// Set of extensions for <seealso cref="BotClient"/>
    /// </summary>
    public static class ClientExtensions
    {
        /// <summary>
        /// Sends Chat message
        /// </summary>
        /// <param name="client">Instance that sends the message</param>
        /// <param name="content">Content to send</param>
        /// <param name="target">If null, sends to ll</param>
        /// <returns>If sending was succeed</returns>
        public static bool SendChatMessage(this BotClient client, string content, Player target = null)
        {
            bool result;

            if(target == null)
            {
                result = client.SendRPC(2, "Chat", new object[] { content, client.PlayerName }, PhotonTargets.All);
            }
            else
            {
                result = client.SendRPC(2, "Chat", new object[] { content, client.PlayerName }, target.ActorNumber);
            }

            return result;
        }

        /// <summary>
        /// Initialized command handler for given Bot instance
        /// </summary>
        /// <param name="client">Bot instance</param>
        /// <param name="config">Configuration of <seealso cref="CommandHandler"/></param>
        /// <returns>Command handler of <paramref name="client"/></returns>
        /// <exception cref="InvalidOperationException">Throws on attemption to use the method with BotClient that already has initialized <seealso cref="CommandHandler"/></exception>
        public static CommandHandler UseCommands(this BotClient client, Action<CommandHandlerConfiguration> config = null)
        {
            if(client.CommandHandler != null)
            {
                throw new InvalidOperationException("Cannot re-attach CommandHandler of BotClient");
            }
            var configuration = new CommandHandlerConfiguration();
            if(config != null)
            {
                config?.Invoke(configuration);
            }

            var commandHandler = new CommandHandler(client);
            commandHandler.Prefix = configuration.Prefix;

            client.CommandHandler = commandHandler;
            return client.CommandHandler;
        }
    }
}
