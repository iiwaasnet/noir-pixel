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
        service.authenticated = authenticated;
        service.registerExternal = registerExternal;
        service.getLocalToken = getLocalToken;

        service.getUserInfo = getUserInfo;

        function authenticated() {
            return !!TokenStorage.getToken();
        }

        function googleSignIn() {
            var uri = '/account/external-login?provider=Google&response_type=token&client_id=self&redirect_uri=http%3A%2F%2Fnoir-pixel.com%2Fexternal-signin%2F';

            return Url.build([Config.apiUris.base, uri]);
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

        function getLocalToken(externalToken, provider) {
            var uri = 'account/local-access-token',
                url = Url.build([Config.apiUris.base, uri]),
                deferred = $q.defer();

            $http.post(url,
                {
                    provider: provider,
                    externalAccessToken: externalToken
                },
                { headers: { 'Authorization': 'Bearer ' + externalToken } })
                .success(function (response) { getLocalTokenSuccess(response, deferred); })
                .error(function (err, status) { getLocalTokenError(err, status, deferred); });

            return deferred.promise;
        }

        function getLocalTokenSuccess(response, deferred) {
            TokenStorage.setToken(response.access_token);
            deferred.resolve(response);
        }

        function getLocalTokenError(err, status, deferred) {
            debugger;
            deferred.reject(err);
        }

        function registerExternal(externalToken, provider) {
            var uri = 'account/register-external',
                url = Url.build([Config.apiUris.base, uri]),
                deferred = $q.defer();

            $http.post(url,
                 {
                     provider: provider,
                     externalAccessToken: externalToken
                 },
                { headers: { 'Authorization': 'Bearer ' + externalToken } })
                .success(function (response) { registerExternalSuccess(response, deferred); })
                .error(function (err, status) { registerExternalError(err, status, deferred); });

            return deferred.promise;
        }


        function registerExternalSuccess(response, deferred) {
            debugger;
            deferred.resolve(response.access_token);
        }

        function registerExternalError(err, status, deferred) {
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