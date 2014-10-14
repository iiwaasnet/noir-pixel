(function() {
    'use strict';

    var config = {
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
    };

    angular.module('np.config')
        .constant('Config', config);
})();