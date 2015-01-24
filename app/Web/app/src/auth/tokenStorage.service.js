(function() {
    'use strict';

    var tokenStorageProvider = {
        $get: ['Storage', tokenStorageService]
};

    angular.module('np.auth')
        .provider('TokenStorage', tokenStorageProvider);


    angular.module('np.auth')
        .service('TokenStorage', tokenStorageService);

    tokenStorageService.$inject = ['Storage'];

    function tokenStorageService(Storage) {
        var service = this,
            tokenKey = 'auth-token',
            token = null;
        service.getToken = getToken;
        service.setToken = setToken;
        service.deleteToken = deleteToken;

        function getToken() {
            if (!token) {
                token = Storage.get(tokenKey);
            }
            return token;
        }

        function deleteToken() {
            token = null;
            Storage.remove(tokenKey);
        }

        function setToken(authToken) {
            token = authToken;
            Storage.set(tokenKey, authToken);
        }
    }
})();