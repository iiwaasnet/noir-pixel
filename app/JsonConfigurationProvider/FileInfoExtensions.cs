using System.IO;

namespace JsonConfigurationProvider
{
    public static class FileInfoExtensions
    {
        public static string NameWithoutExtension(this FileInfo fileInfo)
        {
            return fileInfo.Name.Split('.')[0];
        }
    }
}