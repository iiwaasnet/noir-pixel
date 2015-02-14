using System.IO;

namespace Api.App.Media.Extensions
{
    public static class FileNameExtensions
    {
        public static string GetFileExtension(this string fileName)
        {
            return Path.GetExtension(fileName).TrimStart('.');
        }
    }
}