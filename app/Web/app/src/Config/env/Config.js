(function() {
    'use strict';

    var config = {
        environment: '@@environment',
        siteBaseUri: '@@siteBaseUri',
        loggingApiUri: '@@loggingApiUri',
        apiUris: {
            base: '@@base',
            signin: '@@signin',
            externalLogin: '@@externalLogin',
            localAccessToken: '@@localAccessToken',
            registerExternal: '@@registerExternal'
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