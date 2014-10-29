(function() {
    'use strict';

    angular.module('np.auth')
        .service('Auth', authService);

    authService.$injector = ['$http', '$q', 'Config', 'Url', 'TokenStorage', 'EventsHub'];

    function authService($http, $q, Config, Url, TokenStorage, EventsHub) {
        var service = this;
        service.signIn = signIn;
        service.googleSignIn = googleSignIn;
        service.signOut = signOut;
        service.externalSignUp = externalSignUp;
        service.authenticated = authenticated;
        service.getUserInfo = getUserInfo;

        function authenticated() {
            return !!TokenStorage.getToken();
        }

        function googleSignIn() {
            var extLogin = '/account/external-login?provider=Google&response_type=token&client_id=self&redirect_uri=http%3A%2F%2Fnoir-pixel.com%2Fexternal-signin%2F',
                url = Url.build([Config.apiUris.base, extLogin]),
                deferred = $q.defer();

            return url;

            $http.get(url)
                .success(function(response) { googleSignInSuccess(response, deferred); })
                .error(function (err, status) { googleSignInError(err, status, deferred); });

            return deferred.promise;
        }

        function signIn(userName, pwd) {
            var url = Url.build([Config.apiUris.base, Config.apiUris.signin]),
                data = "grant_type=password&username=" + userName + "&password=" + pwd,
                deferred = $q.defer();

            $http.post(url,
                    data,
                    {
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                    })
                .success(function(response) { signInSuccess(response, deferred); })
                .error(function(err, status) { signInError(err, status, deferred); });

            return deferred.promise;
        }

        function googleSignInSuccess(response, deferred) {
            deferred.resolve(response);
        }

        function googleSignInError(err, status, deferred) {
            deferred.reject(err);
        }

        function externalSignUp(externalToken) {
            var externalLogin = 'account/register-external',
                url = Url.build([Config.apiUris.base, externalLogin]),
                deferred = $q.defer();

            $http.post(url,
                { email: 'me-google@google.com' },
                { headers: { 'Authorization': 'Bearer ' + externalToken } })
                .success(function(response) { externalSignUpSuccess(response, deferred); })
                .error(function(err, status) { externalSignUpError(err, status, deferred); });

            return deferred.promise;
        }


        function externalSignUpSuccess(response, deferred) {
            debugger;
            deferred.resolve();
        }

        function externalSignUpError(err, status, deferred) {
            debugger;
            deferred.reject();
        }

        function signInSuccess(response, deferred) {
            TokenStorage.setToken(response.access_token);
            EventsHub.publishEvent(EventsHub.events.SignedIn);

            deferred.resolve(response);
        }

        function signInError(err, status, deferred) {
            TokenStorage.deleteToken();
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

        function signOut() {
            TokenStorage.deleteToken();
            EventsHub.publishEvent(EventsHub.events.SignedOut);
        }
    }
})();