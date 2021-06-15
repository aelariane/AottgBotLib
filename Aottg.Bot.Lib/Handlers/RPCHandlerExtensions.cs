using AottgBotLib;

namespace AottgBotLib.Handlers
{
    /// <summary>
    /// Set of extensions related to RPCs
    /// </summary>
    public static class RPCHandlerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">ID of sender in the room</param>
        /// <param name="sender">Sender name</param>
        /// <param name="message">Message Content</param>
        public delegate void ChatCallback(int Id, string sender, string message);

        /// <summary>
        /// Adds callback for Chat method
        /// </summary>
        /// <param name="handler">Handler to attach callback</param>
        /// <param name="callback">Callback</param>
        /// <returns>Handler to continue chatin calls</returns>
        public static RPCHandler AddChatCallback(this RPCHandler handler, ChatCallback callback)
        {
            handler.AddCallback(SupportedRPC.Chat, (args) =>
            {
                if(args.CallInfo.ViewID == 2 && args.Arguments.Length == 2)
                {
                    string sender = args.Arguments[1] as string ?? string.Empty;
                    string content = args.Arguments[0] as string ?? string.Empty;
                    callback(args.CallInfo.Sender.ActorNumber, sender, content);
                }
            });

            return handler;
        }
    }
}
