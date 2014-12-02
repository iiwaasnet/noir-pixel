(function() {
    'use strict';

    var config = {
        environment: 'DEV',
        siteBaseUri: 'noir-pixel.com',
        loggingApiUri: 'api.logging.noir-pixel.com/log/add',
        apiUris: {
            base: 'api.noir-pixel.com',
            signin: 'token',
            externalLogins: '/accounts/external-logins?returnUrl={0}',
            localAccessToken: 'accounts/local-access-token',
            registerExternal: 'accounts/register-external'
        },
        strings: {
            invalidationTimeout: '00:10:00',
            versionsUri: '/strings/versions',
            localizedUri: '/strings/localized/'
        }
    };

    angular.module('np.config')
        .constant('Config', config);
})();