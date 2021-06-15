using System.Reflection;

namespace AottgBotLib.Commands.Internal
{
    internal sealed class Command
    {
        private string _cachedLowerName;

        internal CommandModule Owner { get; set; }

        public string Description { get; internal set; }

        public string LowerName
        {
            get
            {
                if (_cachedLowerName != null)
                {
                    return _cachedLowerName;
                }
                _cachedLowerName = Name.ToLower();
                return _cachedLowerName;
            }
        }

        public MethodInfo Method { get; internal set; }
        public string Name { get; internal set; }
        public bool IsMasterClientOnly { get; internal set; } = false;
        public string CustomPrefix { get; internal set; } = null;

    }
}