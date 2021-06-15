using Photon.Realtime;

namespace AottgBotLib.Commands
{
    /// <summary>
    /// Represents command execution context
    /// </summary>
    public sealed class CommandContext
    {
        private string[] _cachedArguments;

        /// <summary>
        /// Arguments coming after command
        /// </summary>
        /// <remarks>Basically <seealso cref="MessageContent"/> but split with space</remarks>
        public string[] Arguments
        {
            get
            {
                if(MessageContent.Length == 0)
                {
                    _cachedArguments =  new string[0];
                }
                if(_cachedArguments != null)
                {
                    return _cachedArguments;
                }
                _cachedArguments = MessageContent.TrimStart().TrimEnd().Split(' ');
                return _cachedArguments;
            }
        }
        /// <summary>
        /// Client invoked Command
        /// </summary>
        public BotClient Client { get; internal set; }
        /// <summary>
        /// Content of message
        /// </summary>
        public string MessageContent { get; internal set; }
        /// <summary>
        /// Player who invoked the command
        /// </summary>
        public Player Sender { get; internal set; }

        internal CommandContext(BotClient client, string content, Player sender)
        {
            Client = client;
            MessageContent = content;
            Sender = sender;
        }
    }
}