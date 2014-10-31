(function() {
    'use strict';

    var config = {
        environment: 'DEV',
        siteBaseUri: 'noir-pixel.com',
        loggingApiUri: 'api.logging.noir-pixel.com/log/add',
        apiUris: {
            base: 'api.noir-pixel.com',
            signin: 'token',
            externalLogin: '/account/external-login?provider={0}&response_type=token&client_id=self&redirect_uri={1}'
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