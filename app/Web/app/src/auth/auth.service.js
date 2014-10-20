(function() {
    'use strict';

    angular.module('np.auth')
        .service('Auth', authService);

    authService.$injector = ['$http', '$q', 'Config', 'Url', 'tokenStorage', 'EventsHub'];

    function authService($http, $q, Config, Url, tokenStorage, EventsHub) {
        var service = this;
        service.signIn = signIn;
        service.signOut = signOut;
        service.extSignIn = extSignIn;
        service.authenticated = authenticated;
        service.getUserInfo = getUserInfo;
        
        function authenticated() {
            return !!tokenStorage.getToken();
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

        function extSignIn(provider) {
        }

        function signInSuccess(response, deferred) {
            tokenStorage.setToken(response.access_token);
            EventsHub.publishEvent(EventsHub.events.SignedIn);

            deferred.resolve(response);
        }

        function signInError(err, status, deferred) {
            tokenStorage.deleteToken();
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
            tokenStorage.deleteToken();
            EventsHub.publishEvent(EventsHub.events.SignedOut);
        }
    }
})();