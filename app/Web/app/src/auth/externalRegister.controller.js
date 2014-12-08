(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalRegisterController', externalRegisterController);

    externalRegisterController.$inject = ['$location', '$scope', '$window', '$stateParams', 'Url', 'Signin', 'Validation', 'Errors', 'Messages'];

    function externalRegisterController($location, $scope, $window, $stateParams, Url, Signin, Validation, Errors, Messages) {
        var ctrl = this;
        ctrl.scope = $scope;
        ctrl.register = register;
        ctrl.userName = 'iiwaasnet';
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
            if (error.errors) {
                Validation.setValidationErrors(ctrl.scope.externalRegister, error.errors);
            }
            else {
                if (Validation.knownError(error.code)) {
                    ctrl.scope.externalRegister.UserName.$error[error.code] = true;
                }
                else {
                    //TODO: make error object simplier
                    Messages.error({ main: { code: Errors.Auth.RegistrationError } });
                }
            }
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