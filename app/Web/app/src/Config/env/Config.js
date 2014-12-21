(function() {
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
            Users: {
                Home: '@@ApiUris.Users.Home'
            },
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