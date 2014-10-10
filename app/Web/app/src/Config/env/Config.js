(function() {
    'use strict';

    angular.module('np.config')
        .constant('Config', {
            environment: '@@environment',
            loggingApiUri: '@@loggingApiUri',
            apiUris: {
                base: '@@base',
                signin: '@@signin'
            },
            strings: {
                invalidationTimeout: '@@invalidationTimeout',
                versionsUri: '@@versionsUri',
                localizedUri: '@@localizedUri'
            }
        });
})();