namespace AottgBotLib.Handlers
{
    /// <summary>
    /// Arguments of RPC call
    /// </summary>
    public sealed class RPCArguments
    {
        /// <summary>
        /// Received method arguments
        /// </summary>
        public object[] Arguments { get; }
        /// <summary>
        /// Meta information
        /// </summary>
        public RPCCallInfo CallInfo { get; }
        /// <summary>
        /// Current client instance
        /// </summary>
        public BotClient Client { get; }

        internal RPCArguments(BotClient client, object[] args, RPCCallInfo callInfo)
        {
            CallInfo = callInfo;
            Client = client;
            Arguments = args;
        }
    }
}