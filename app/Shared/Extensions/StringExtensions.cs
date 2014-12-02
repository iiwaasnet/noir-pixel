namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static string AppendFormatting(this string str)
        {
            return str + ": {0}";
        }
    }
}