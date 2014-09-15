(function() {
    'use strict';

    angular.module('np.config')
        .constant('Config', {
            environment: 'DEV',
            loggingApiUri: 'api.logging.noir-pixel.com/log/add',
            strings: {
                invalidationTimeout: '00:10:00',
                versionsUri: '/strings/versions',
                localizedUri: '/strings/localized/'
            }
        });
})();