(function () {
    'use strict';

    var config = {
        Environment: '@@Environment',
        SiteBaseUri: '@@SiteBaseUri',
        LoggingApiUri: '@@LoggingApiUri',
        ApiUris: {
            Base: '@@ApiUris.Base',
            Accounts: {
                Signin: '@@ApiUris.Accounts.Signin',
                ExternalLogins: '@@ApiUris.Accounts.ExternalLogins',
                LocalAccessToken: '@@ApiUris.Accounts.LocalAccessToken',
                RegisterExternal: '@@ApiUris.Accounts.RegisterExternal',
                UserExists: '@@ApiUris.Accounts.UserExists'
            },
            Profiles: {
                Profile: '@@ApiUris.Profiles.Profile',
                Countries: '@@ApiUris.Profiles.Countries',
                UpdateProfileImage: '@@ApiUris.Profiles.UpdateProfileImage',
                DeleteProfileImage: '@@ApiUris.Profiles.DeleteProfileImage',
                UpdatePublicInfo: '@@ApiUris.Profiles.UpdatePublicInfo',
                UpdatePrivateInfo: '@@ApiUris.Profiles.UpdatePrivateInfo'
            },
            Geo: {
                Countries: '@@ApiUris.Geo.Countries'
            },
            Photos: {
                Upload: '@@ApiUris.Photos.Upload',
                GetPending: '@@ApiUris.Photos.GetPending',
                GetPhotoForEdit: '@@ApiUris.Photos.GetPhotoForEdit'
}
        },
        Profiles: {
            Image: {
                MaxFileSizeMB: '@@Images.ProfileImages.MaxFileSizeMB',
                FullViewSize: '@@Images.ProfileImages.FullViewSize',
                ThumbnailSize: '@@Images.ProfileImages.ThumbnailSize'
            }
        },
        Photos: {
            PreviewSize: '@@Images.Photos.PreviewSize',
            ThumbnailSize: '@@Images.Photos.ThumbnailSize'
        },
        Strings: {
            InvalidationTimeout: '@@Strings.InvalidationTimeout',
            VersionsUri: '@@Strings.VersionsUri',
            LocalizedUri: '@@Strings.LocalizedUri'
        },
        Auth: {
            UserNameValidationRegEx: '@@Auth.UserNameValidationRegEx',
            MinUserNameLength: '@@Auth.MinUserNameLength',
            MaxUserNameLength: '@@Auth.MaxUserNameLength'
        }
    };

    angular.module('np.config')
        .constant('Config', config);
})();