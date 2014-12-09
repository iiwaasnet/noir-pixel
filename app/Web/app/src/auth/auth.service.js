﻿(function() {
    'use strict';

    angular.module('np.auth')
        .service('Auth', authService);

    authService.$inject = ['$http', '$q', '$state', 'Storage', 'Config', 'Url', 'TokenStorage', 'EventsHub'];

    function authService($http, $q, $state, Storage, Config, Url, TokenStorage, EventsHub) {
        var service = this,
            signInState = 'signIn',
            loginRedirectStorageKey = 'loginRedirectState',
            availableLogins = [];
        service.signOut = signOut;
        service.authenticated = authenticated;
        service.registerExternal = registerExternal;
        service.getLocalToken = getLocalToken;
        service.getAvailableLogins = getAvailableLogins;
        service.userExists = userExists;

        function authenticated() {
            return !!TokenStorage.getToken();
        }

        function getAvailableLogins() {
            if (availableLogins.length > 0) {
                var deferred = $q.defer();
                deferred.resolve(availableLogins);
                return deferred.promise;
            }

            return getLoginsFromServer();
        }

        function getLoginsFromServer() {
            var externalSignIn = $state.get('externalSignIn').url.split('?')[0],
                redirectUrl = Url.build(Config.siteBaseUri, externalSignIn);
            var url = Url.build(Config.apiUris.base, Config.apiUris.externalLogins.format(encodeURIComponent(redirectUrl)));

            var deferred = $q.defer();
            $http.get(url).then(
                function(response) { getAvailableLoginsSuccess(response, deferred); },
                function(error) { getAvailableLoginsError(error, deferred); }
            );

            return deferred.promise;
        }

        function getAvailableLoginsSuccess(response, deferred) {
            availableLogins = [];

            angular.forEach(response.data, function(login) {
                availableLogins.push({
                    provider: login.name,
                    url: Url.build(Config.apiUris.base, login.url)
                });
            });

            deferred.resolve(availableLogins);
        }

        function getAvailableLoginsError(error, deferred) {
            deferred.reject(error);
        }

        function getLocalToken(externalLogin) {
            var url = Url.build(Config.apiUris.base, Config.apiUris.localAccessToken),
                deferred = $q.defer();

            $http.post(url,
                {
                    provider: externalLogin.provider,
                    externalAccessToken: externalLogin.externalAccessToken,
                    accessTokenSecret: externalLogin.accessTokenSecret,
                },
                { headers: { 'Authorization': 'Bearer ' + externalLogin.externalAccessToken } })
                .success(function(response) { getLocalTokenSuccess(response, deferred); })
                .error(function(err, status) { getLocalTokenError(err, status, deferred); });

            return deferred.promise;
        }

        function getLocalTokenSuccess(response, deferred) {
            TokenStorage.setToken(response.access_token);
            deferred.resolve(response);
        }

        function getLocalTokenError(err, status, deferred) {
            deferred.reject(err);
        }

        function registerExternal(externalLogin, userName) {
            var url = Url.build(Config.apiUris.base, Config.apiUris.registerExternal);

            return $http.post(url,
            {
                provider: externalLogin.provider,
                externalAccessToken: externalLogin.externalAccessToken,
                accessTokenSecret: externalLogin.accessTokenSecret,
                userName: userName
            },
            { headers: { 'Authorization': 'Bearer ' + externalLogin.externalAccessToken } });
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

        function userExists(userName) {
            var url = Url.build(Config.apiUris.base, Config.apiUris.userExists).formatNamed({userName: userName});

            return $http.get(url);
        }

        function signOut() {
            TokenStorage.deleteToken();
            EventsHub.publishEvent(EventsHub.events.SignedOut);
        }

        //function saveLoginRedirectState(redirectState) {
        //    if (redirectState !== signInState) {
        //        Storage.set(loginRedirectStorageKey, redirectState);
        //    }
        //}

        //function getLoginRedirectState() {
        //    var redirectState = Storage.get(loginRedirectStorageKey);

        //    return redirectState || 'home';
        //}
    }
})();