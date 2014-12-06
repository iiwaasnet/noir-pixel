using System.Linq;

namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static string AddFormatting(this string str)
        {
            return str.AddFormatting(1);
        }
        
        public static string AddFormatting(this string str, int count, string separator = ":")
        {
            var format = string.Join(separator, Enumerable.Range(0, count).Select(i => "{" + i + "}"));
            return str + ": " + format;
        }
    }
}