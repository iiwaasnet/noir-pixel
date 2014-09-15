(function() {
    'use strict';

    angular.module('np.config')
        .constant('Config', {
            environment: '@@environment',
            loggingApiUri: '@@loggingApiUri',
            strings: {
                invalidationTimeout: '@@invalidationTimeout',
                versionsUri: '@@versionsUri',
                localizedUri: '@@localizedUri'
            }
        });
})();