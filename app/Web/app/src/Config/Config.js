(function() {
    'use strict';

    var config = {
        environment: 'DEV',
        loggingApiUri: 'api.logging.noir-pixel.com/log/add',
        apiUris: {
            base: 'api.noir-pixel.com',
            signin: 'token'
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