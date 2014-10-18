(function() {
    'use strict';

    angular.module('np.auth')
        .service('tokenStorage', tokenStorageService);

    tokenStorageService.$injector = ['localStorageService'];

    function tokenStorageService(localStorageService) {
        var service = this,
            tokenKey = 'auth-token',
            token = null;
        service.getToken = getToken;
        service.setToken = setToken;
        service.deleteToken = deleteToken;

        function getToken() {
            if (!token) {
                token = localStorageService.get(tokenKey);
            }
            return token;
        }

        function deleteToken() {
            token = null;
            localStorageService.remove(tokenKey);
        }

        function setToken(authToken) {
            token = authToken;
            localStorageService.set(tokenKey, authToken);
        }
    }
})();