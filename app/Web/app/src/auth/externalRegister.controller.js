(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalRegisterController', externalRegisterController);

    externalRegisterController.$inject = ['$location', '$scope', '$window', '$stateParams', 'Url', 'Signin', 'Validation', 'Errors', 'Messages'];

    function externalRegisterController($location, $scope, $window, $stateParams, Url, Signin, Validation, Errors, Messages) {
        var ctrl = this;
        ctrl.scope = $scope;
        ctrl.register = register;
        ctrl.close = close;
        ctrl.userName = '';

        activate();

        function activate() {
            var externalLogin = getExternalLoginData();

            if (externalLogin.error) {
                Signin.finalizeSigninError(externalLogin.error);
            }

            ctrl.externalLogin = externalLogin;
        }

        function close() {
            Signin.finalizeSigninSuccess();
        }

        function register() {
            Signin.registerExternal(ctrl.externalLogin, ctrl.userName)
                .then(function() {}, registerExternalError);
        }

        function registerExternalError(error) {
            var errorCode = error.code;
            var placeholders = error.placeholderValues;

            if (!Validation.knownError(errorCode)) {
                errorCode = Errors.Auth.RegistrationError;
            }
            if (error.errors && error.errors.length > 0) {
                errorCode = error.errors[0].code;
                placeholders = error.errors[0].code.placeholderValues;
            }

            //TODO: Provide PlacehodelValues and do formatting in Messages.error()
            Messages.error({ main: { code: errorCode } }, placeholders);
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