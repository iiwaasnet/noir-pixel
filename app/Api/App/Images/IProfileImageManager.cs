using System;
using System.Drawing;

namespace Api.App.Images
{
    public interface IProfileImageManager
    {
        Size AvatarSize();

        Size ThumbnailSize();

        Uri DefaultAvatarUri();

        Uri DefaultThumbnailUri();
    }
}