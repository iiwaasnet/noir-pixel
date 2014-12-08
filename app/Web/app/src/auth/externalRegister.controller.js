(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalRegisterController', externalRegisterController);

    externalRegisterController.$inject = ['$location', '$window', '$stateParams', 'Url', 'Signin'];

    function externalRegisterController($location, $window, $stateParams, Url, Signin) {
        var ctrl = this;
        ctrl.register = register;
        ctrl.userName = '';
        ctrl.errors = {};

        activate();

        function activate() {
            var externalLogin = getExternalLoginData();

            if(externalLogin.error){
                Signin.finalizeSigninError(externalLogin.error);
            }

            ctrl.externalLogin = externalLogin;
        }

        function register() {
            Signin.registerExternal(ctrl.externalLogin, ctrl.userName)
            .then(function() {}, registerExternalError);
        }

        function registerExternalError(error) {
            ctrl.errors[error.code] = true;
        }

        function getExternalLoginData() {
            if (!$stateParams.external_access_token || !$stateParams.provider) {
                return {
                    error: 'Missing external_access_token or provider'
                };
            }

            return {
                externalAccessToken: $stateParams.external_access_token,
                accessTokenSecret: $stateParams.access_token_secret,
                provider: $stateParams.provider
            };
        }
    }
})();