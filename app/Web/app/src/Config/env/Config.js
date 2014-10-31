(function() {
    'use strict';

    var config = {
        environment: '@@environment',
        siteBaseUri: '@@siteBaseUri',
        loggingApiUri: '@@loggingApiUri',
        apiUris: {
            base: '@@base',
            signin: '@@signin',
            externalLogin: '@@externalLogin'
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