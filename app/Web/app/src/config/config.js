(function () {
    'use strict';

    var config = {
        Environment: '@@Environment',
        SiteBaseUri: 'noir-pixel.com',
        LoggingApiUri: 'api.logging.noir-pixel.com/log/add',
        ApiUris: {
            Base: 'api.noir-pixel.com',
            Accounts: {
                Signin: 'token',
                ExternalLogins: '/accounts/external-logins?returnUrl={0}',
                LocalAccessToken: 'accounts/local-access-token',
                RegisterExternal: 'accounts/register-external',
                UserExists: 'accounts/exists/{userName}'
            },
            Profiles: {
                Profile: 'profiles/{userName}',
                Countries: 'profiles/countries',
                UpdateProfileImage: 'profiles/update-profile-image',
                DeleteProfileImage: 'profiles/delete-profile-image',
                UpdatePublicInfo: 'profiles/update-profile/public-info',
                UpdatePrivateInfo: 'profiles/update-profile/private-info'
            },
            Geo: {
                Countries: 'geo/countries'
            },
            Photos: {
                Upload: 'photos/upload',
                GetPending: 'photos/pending',
                GetPhotoForEdit: 'photos/{shortId}/edit'
}
        },
        Profiles: {
            Image: {
                MaxFileSizeMB: '1',
                FullViewSize: '130',
                ThumbnailSize: '32'
            }
        },
        Photos: {
            PreviewSize: '200',
            ThumbnailSize: '60'
        },
        Strings: {
            InvalidationTimeout: '00:10:00',
            VersionsUri: '/strings/versions',
            LocalizedUri: '/strings/localized/'
        },
        Auth: {
            UserNameValidationRegEx: '((^\\B|^\\b)[a-z0-9_\\-]+(\\B$|\\b$))',
            MinUserNameLength: '2',
            MaxUserNameLength: '20'
        }
    };

    angular.module('np.config')
        .constant('Config', config);
})();