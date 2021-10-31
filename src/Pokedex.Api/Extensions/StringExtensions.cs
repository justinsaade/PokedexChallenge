using System.Text.RegularExpressions;

namespace Pokedex.Api.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceEscapeSequenceWithSpaces(this string str)
        {
            return Regex.Replace(str, @"\t|\n|\r|\f", " ");
        }
    }
}
