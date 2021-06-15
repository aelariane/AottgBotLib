using System;

namespace AottgBotLib.Commands
{
    /// <summary>
    /// Description of command, for displaying it via help command
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// Description of commands
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc"></param>
        public DescriptionAttribute(string desc)
        {
            Description = desc;
        }
    }
}
