using System.IO;

namespace Api.App.Media.Config
{
    public class MediaConfiguration
    {
        private string rootMediaFolder;
        private string uploadFolder;
        private string storageFolder;
        private string profileImagesFolderTemplate;
        private string photosFolderTemplate;

        private void InitProperties()
        {
            uploadFolder = PrefixedOrSelf(uploadFolder, rootMediaFolder);
            storageFolder = PrefixedOrSelf(storageFolder, rootMediaFolder);
            profileImagesFolderTemplate = PrefixedOrSelf(profileImagesFolderTemplate, rootMediaFolder);
            photosFolderTemplate = PrefixedOrSelf(photosFolderTemplate, rootMediaFolder);
        }

        private string PrefixedOrSelf(string self, string prefix)
        {
            return (string.IsNullOrWhiteSpace(prefix) || string.IsNullOrWhiteSpace(self))
                       ? self
                       : Path.Combine(prefix, self);
        }

        public string RootMediaFolder {
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

        public string StorageFolder
        {
            get { return storageFolder; }
            set { storageFolder = PrefixedOrSelf(value, rootMediaFolder); }
        }

        public string ProfileImagesFolderTemplate
        {
            get { return profileImagesFolderTemplate; }
            set { profileImagesFolderTemplate = PrefixedOrSelf(value, rootMediaFolder); }
        }

        public string PhotosFolderTemplate
        {
            get { return photosFolderTemplate; }
            set { photosFolderTemplate = PrefixedOrSelf(value, rootMediaFolder); }
        }
    }
}