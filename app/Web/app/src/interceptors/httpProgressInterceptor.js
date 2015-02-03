(function() {
    'use strict';

    angular.module('np')
        .config(configHttp);

    configHttp.$inject = ['$httpProvider'];

    function configHttp($httpProvider) {
        $httpProvider.interceptors.push(httpProgressProvider);
    }

    httpProgressProvider.$inject = ['$q', 'Progress'];

    function httpProgressProvider($q, Progress) {

        return {
            responseError: responseError,
            request: request,
            response: response
        };

        function response(resp) {
            try {
                Progress.stop();
            } catch (e) {
            }

            return resp;
        }

        function request(config) {
            try {
                Progress.start();
            } catch (e) {
            }

            return config;
        }

        function responseError(resp) {
            try {
                Progress.stop();
            } catch (e) {
            }

            return $q.reject(resp);
        }
    }
})();