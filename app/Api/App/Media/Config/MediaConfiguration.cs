using System.IO;

namespace Api.App.Media.Config
{
    public class MediaConfiguration
    {
        private string rootMediaFolder;
        private string uploadFolder;
        private string profileImageFolderTemplate;
        private string photosFolderTemplate;

        private void InitProperties()
        {
            uploadFolder = PrefixedOrSelf(uploadFolder, rootMediaFolder);
            profileImageFolderTemplate = PrefixedOrSelf(profileImageFolderTemplate, rootMediaFolder);
            photosFolderTemplate = PrefixedOrSelf(photosFolderTemplate, rootMediaFolder);
        }

        private string PrefixedOrSelf(string self, string prefix)
        {
            return (string.IsNullOrWhiteSpace(prefix) || string.IsNullOrWhiteSpace(self))
                       ? self
                       : Path.Combine(prefix, self);
        }

        public string RootMediaFolder
        {
            get { return rootMediaFolder; }
            set
            {
                rootMediaFolder = value;
                InitProperties();
            }
        }

        public string UploadFolder
        {
            get { return uploadFolder; }
            set { uploadFolder = PrefixedOrSelf(value, rootMediaFolder); }
        }

        public string ProfileImageFolderTemplate
        {
            get { return profileImageFolderTemplate; }
            set { profileImageFolderTemplate = PrefixedOrSelf(value, rootMediaFolder); }
        }

        public string PhotosFolderTemplate
        {
            get { return photosFolderTemplate; }
            set { photosFolderTemplate = PrefixedOrSelf(value, rootMediaFolder); }
        }
    }
}