using System;

namespace AottgBotLib.Commands
{
    /// <summary>
    /// Use this to create custom prefix for command instead of default one
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class PrefixAttribute : Attribute
    {
        public string Prefix { get; }

        public PrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }
    }
}
