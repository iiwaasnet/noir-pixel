angular.module('npConfig', [])
    .constant('Config', {
        environment: '@@environment',
        loggingApiUri: '@@loggingApiUri',
        strings: {
            invalidationTimeout: '@@invalidationTimeout',
            versionsUri: '@@versionsUri',
            localizedUri: '@@localizedUri'
        }
    });