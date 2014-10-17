(function() {
    'use strict';

    var httpProviderConfig = {
        interceptStatuses: [500],
        maxDataLength: 200
    };

    angular.module('np')
        .constant('HttpProviderConfig', httpProviderConfig)
        .config(config);

    config.$injector = ['$httpProvider'];

    function config($httpProvider) {
        $httpProvider.interceptors.push(httpProvider);
    }

    httpProvider.$injector = ['$q', 'ApplicationLogging', 'HttpProviderConfig'];

    function httpProvider($q, ApplicationLogging, HttpProviderConfig) {

        return { responseError: responseError };

        function responseError(response) {
            if (response && ~HttpProviderConfig.interceptStatuses.indexOf(response.status)) {
                ApplicationLogging.error(createError(response));
            }

            return $q.reject(response);
        }

        function createError(response) {
            var error = {
                method: response.config.method,
                url: response.config.url,
                message: (response.data && response.data.toString()) || '',
                status: response.status,
                toString: toString
            };

            return error;

            function toString() {
                return 'method: ' + error.method.toUpperCase()
                    + ' url: ' + error.url
                    + ' status: ' + error.status
                    + ' data: ' + error.message.substring(0, HttpProviderConfig.maxDataLength);
            }
        }
    }
})();