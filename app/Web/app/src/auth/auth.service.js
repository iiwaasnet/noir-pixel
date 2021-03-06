﻿(function() {
    'use strict';

    angular.module('np.auth')
        .service('Auth', authService);

    authService.$inject = ['$http', '$q', '$state', 'Config', 'Url', 'Storage', 'TokenStorage', 'EventsHub', 'User'];

    function authService($http, $q, $state, Config, Url, Storage, TokenStorage, EventsHub, User) {
        var srv = this,
            availableLogins = [];
        srv.signOut = signOut;
        srv.authenticated = authenticated;
        srv.registerExternal = registerExternal;
        srv.getLocalToken = getLocalToken;
        srv.getAvailableLogins = getAvailableLogins;
        srv.userExists = userExists;

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
                redirectUrl = Url.buildUrl(Config.SiteBaseUri, externalSignIn);
            var url = Url.buildApiUrl(Config.ApiUris.Accounts.ExternalLogins.format(encodeURIComponent(redirectUrl)));
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
                    url: Url.buildApiUrl(login.url)
                });
            });

            deferred.resolve(availableLogins);
        }

        function getAvailableLoginsError(error, deferred) {
            deferred.reject(error);
        }

        function getLocalToken(externalLogin) {
            var url = Url.buildApiUrl(Config.ApiUris.Accounts.LocalAccessToken),
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
            User.saveUserData({
                userName: response.userName,
                roles: response.roles
            });

            EventsHub.publishEvent(EventsHub.events.Auth.SignedIn);

            deferred.resolve(response);
        }

        function getLocalTokenError(err, status, deferred) {
            deferred.reject(err);
        }

        function registerExternal(externalLogin, userName) {
            var url = Url.buildApiUrl(Config.ApiUris.Accounts.RegisterExternal);

            return $http.post(url,
            {
                provider: externalLogin.provider,
                externalAccessToken: externalLogin.externalAccessToken,
                accessTokenSecret: externalLogin.accessTokenSecret,
                userName: userName
            },
            { headers: { 'Authorization': 'Bearer ' + externalLogin.externalAccessToken } });
        }

        function userExists(userName) {
            var url = Url.buildApiUrl(Config.ApiUris.Accounts.UserExists).formatNamed({ userName: userName });

            return $http.get(url);
        }

        function signOut() {
            TokenStorage.deleteToken();
            EventsHub.publishEvent(EventsHub.events.Auth.SignedOut);
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