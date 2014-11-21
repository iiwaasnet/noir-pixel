(function() {
    'use strict';

    angular.module('np.logging')
        .factory('ExceptionLogging', exceptionLoggingFactory);

    exceptionLoggingFactory.$inject = ['ApplicationLogging'];

    function exceptionLoggingFactory(ApplicationLogging) {
        return ApplicationLogging.error;
    }
})();