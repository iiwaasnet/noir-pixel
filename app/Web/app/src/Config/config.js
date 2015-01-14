(function() {
    'use strict';

    var config = {
        Environment: 'DEV',
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
                Profile: 'profiles/{userName}'
            },
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