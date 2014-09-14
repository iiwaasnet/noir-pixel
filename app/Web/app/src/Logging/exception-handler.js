(function() {
    'use strict';

    var exceptionHandlerProvider = {
        $get: ['ExceptionLogging', exceptionHandler]
    };

    angular.module('np.logging')
        .provider('$exceptionHandler', exceptionHandlerProvider);

    function exceptionHandler(ExceptionLogging) {
        return ExceptionLogging;
    }
})();