namespace AottgBotLib.Commands
{
    /// <summary>
    /// Provides configuration for <seealso cref="CommandHandler"/>
    /// </summary>
    public class CommandHandlerConfiguration
    {
        /// <summary>
        /// Sets <see cref="CommandHandler.Prefix"/>
        /// </summary>
        public string Prefix { get; set; } = "!";

        internal CommandHandlerConfiguration()
        {
            Prefix = "!";
        }
    }
}
