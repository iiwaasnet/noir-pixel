(function() {
    'use strict';

    var httpProviderConfig = {
        InterceptStatuses: [500],
        MaxDataLength: 200
    };

    angular.module('np')
        .constant('HttpProviderConfig', httpProviderConfig)
        .config(configHttp);

    configHttp.$inject = ['$httpProvider'];

    function configHttp($httpProvider) {
        $httpProvider.interceptors.push(httpProvider);
    }

    httpProvider.$inject = ['$q', 'ApplicationLogging', 'HttpProviderConfig', 'TokenStorage'];

    function httpProvider($q, ApplicationLogging, HttpProviderConfig, TokenStorage) {

        return {
            responseError: responseError,
            request: request,
            response: response
        };

        function response(resp) {
            return resp;
        }

        function request(config) {
            var token = TokenStorage.getToken();
            if (!!token) {
                config.headers.Authorization = 'Bearer ' + token;
            }

            return config;
        }

        function responseError(resp) {
            if (resp && ~HttpProviderConfig.InterceptStatuses.indexOf(resp.status)) {
                ApplicationLogging.error(createError(resp));
            }

            return $q.reject(resp);
        }

        function createError(resp) {
            var error = {
                method: resp.config.method,
                url: resp.config.url,
                message: (resp.data && resp.data.toString()) || '',
                status: resp.status,
                toString: toString
            };

            return error;

            function toString() {
                return 'method: ' + error.method.toUpperCase()
                    + ' url: ' + error.url
                    + ' status: ' + error.status
                    + ' data: ' + error.message.substring(0, HttpProviderConfig.MaxDataLength);
            }
        }
    }
})();