(function() {
    'use strict';

    angular.module('np.auth')
        .service('Auth', authService);

    authService.$injector = ['$http', '$q', 'Config', 'Url'];

    function authService($http, $q, Config, Url) {
        var service = this;
        service.signIn = signIn;
        service.extSignIn = extSignIn;
        service.authenticated = authenticated;
        service.getUserInfo = getUserInfo;
        service.token = null;

        function authenticated() {
            return !!service.token;
        }

        function signIn(userName, pwd) {
            var url = Url.build([Config.apiUris.base, Config.apiUris.signin]),
                data = "grant_type=password&username=" + userName + "&password=" + pwd,
                deferred = $q.defer();

            $http.post(url,
                    data,
                    {
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded'

                        }
                    })
                .success(function(response) { signInSuccess(response, deferred); })
                .error(function(err, status) { signInError(err, status, deferred); });

            return deferred.promise;
        }

        function extSignIn(provider) {
        }

        function signInSuccess(response, deferred) {
            service.token = response.access_token;

            deferred.resolve(response);
        }

        function signInError(err, status, deferred) {
            service.token = null;

            deferred.reject(err);
        }

        function getUserInfo() {
            var url = Url.build([Config.apiUris.base, 'account/user-info']);

            var deferred = $q.defer();

            $http.get(url)
                .then(
                    function(response) { deferred.resolve(response.data); },
                    function(error) { deferred.reject(error); });

            return deferred.promise;
        }
    }
})();