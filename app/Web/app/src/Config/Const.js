angular.module('npConfig', [])
    .constant('Const', {
        environment: 'DEV',
        loggingApiUri: 'api.logging.noir-pixel.com/log/add',
        configApiUri: 'configuration/all'
    });