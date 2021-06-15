using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AottgBotLib
{
    public static class StringExtensions
    {
        private const string RemoveAllPattern = @"((\[([0-9a-f]{6})\])|(<(\/|)(color(?(?=\=).*?)>))|(<size=(\\w*)?>?|<\/size>?)|(<\/?[bi]>))";

        /// <summary>
        /// Removes all tags, etc from string
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string RemoveAll(this string x)
        {
            return Regex.Replace(x, @"((\[([0-9a-f]{6})\])|(<(\/|)(color(?(?=\=).*?)>))|(<size=(\\w*)?>?|<\/size>?)|(<\/?[bi]>))", "", RegexOptions.IgnoreCase);
        }
    }
}
