using System.Text.RegularExpressions;

namespace Api.App.Auth.ExternalUserInfo.GPlus
{
    public static class GoogleProfileImageUrlBuilder
    {
        private static readonly Regex sizeMatch;

        static GoogleProfileImageUrlBuilder()
        {
            sizeMatch = new Regex(@"([\?|&]sz=[0-9]+)", RegexOptions.IgnoreCase);
        }

        public static string AvatarUrl(this string imageUrl, int size)
        {
            return ImageUrl(imageUrl, size);
        }
        
        public static string ThumbnailUrl(this string imageUrl, int size)
        {
            return ImageUrl(imageUrl, size);
        }

        private static string ImageUrl(string imageUrl, int size)
        {
            var match = sizeMatch.Match(imageUrl);
            if (match.Success)
            {
                return imageUrl.Replace(match.Value.Substring(1), string.Format("sz={0}", size));
            }

            return imageUrl;
        }
    }
}