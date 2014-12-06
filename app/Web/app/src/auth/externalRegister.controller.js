﻿(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalRegisterController', externalRegisterController);

    externalRegisterController.$inject = ['$location', '$window', '$stateParams', 'Url', 'Signin'];

    function externalRegisterController($location, $window, $stateParams, Url, Signin) {
        var ctrl = this;
        ctrl.register = register;
        ctrl.userName = '';

        activate();

        function activate() {
            ctrl.externalProviderToken = getExternalProviderToken();
        }

        function register() {
            Signin.registerExternal(ctrl.externalProviderToken, ctrl.userName);
        }

        function getExternalProviderToken() {
            return {
                externalAccessToken: $stateParams.external_access_token,
                accessTokenSecret: $stateParams.access_token_secret,
                provider: $stateParams.provider
            };
        }
    }
})();