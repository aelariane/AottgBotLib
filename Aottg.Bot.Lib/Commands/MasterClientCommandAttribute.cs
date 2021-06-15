using System;

namespace AottgBotLib.Commands
{
    /// <summary>
    /// Indicates that command is only accessible when <seealso cref="BotClient"/> is MasterClient
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MasterClientCommandAttribute : Attribute
    {
    }
}
