(function() {
    'use strict';

    angular.module('np.logging')
        .factory('ExceptionLogging', exceptionLoggingFactory);

    exceptionLoggingFactory.$injector = ['ApplicationLogging'];

    function exceptionLoggingFactory(ApplicationLogging) {
        return ApplicationLogging.error;
    }
})();