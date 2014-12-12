(function() {
    'use strict';

    var config = {
        environment: '@@environment',
        siteBaseUri: '@@siteBaseUri',
        loggingApiUri: '@@loggingApiUri',
        apiUris: {
            base: '@@apiUris.base',
            signin: '@@apiUris.signin',
            externalLogins: '@@apiUris.externalLogins',
            localAccessToken: '@@apiUris.localAccessToken',
            registerExternal: '@@apiUris.registerExternal',
            userExists: '@@apiUris.userExists'
        },
        strings: {
            invalidationTimeout: '@@strings.invalidationTimeout',
            versionsUri: '@@strings.versionsUri',
            localizedUri: '@@strings.localizedUri'
        },
        auth: {
            userNameValidationRegEx: '@@userNameValidationRegEx',
            minUserNameLength: '@@minUserNameLength',
            maxUserNameLength: '@@maxUserNameLength'
            }
    };

    angular.module('np.config')
        .constant('Config', config);
})();