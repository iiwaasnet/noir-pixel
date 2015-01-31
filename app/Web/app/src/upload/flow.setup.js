(function() {
    'use strict';

    angular.module('np')
        .config(configXhr);

    configXhr.$inject = ['flowFactoryProvider', 'TokenStorageProvider'];

    function configXhr(flowFactoryProvider, TokenStorageProvider) {
        flowFactoryProvider.defaults = {
            headers: getAuthHeader,
            testChunks: false,
            simultaneousUploads: 1
        };

        var tokenStorage = TokenStorageProvider.$get();

        function getAuthHeader() {
            return { 'Authorization': 'Bearer ' + tokenStorage.getToken() }
        }
    }
    }
)();