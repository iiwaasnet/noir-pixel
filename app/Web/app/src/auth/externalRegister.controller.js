(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalRegisterController', externalRegisterController);

    externalRegisterController.$inject = ['$location', '$scope', '$window', '$stateParams', 'Url', 'Signin', 'Validation', 'Messages', 'Config'];

    function externalRegisterController($location, $scope, $window, $stateParams, Url, Signin, Validation, Messages, Config) {
        var ctrl = this,
            EAPI_Auth_RegistrationError = 'EAPI_Auth_RegistrationError';
        ctrl.scope = $scope;
        ctrl.register = register;
        ctrl.close = close;
        ctrl.userName = '';
        ctrl.userNameValidation = {
            regEx: new RegExp(('/{0}/').format(Config.Auth.UserNameValidationRegEx).slice(1, -1), 'i'),
            minLength: Config.Auth.MinUserNameLength,
            maxLength: Config.Auth.MaxUserNameLength
        };

        activate();

        function activate() {
            var externalLogin = getExternalLoginData();

            if (externalLogin.error) {
                Signin.finalizeSigninError(externalLogin.error);
            }

            ctrl.externalLogin = externalLogin;
        }

        function close() {
            $window.close();
        }

        function register() {
            Signin.registerExternal(ctrl.externalLogin, ctrl.userName)
                .then(function() {}, registerExternalError);
        }

        function registerExternalError(error) {
            var parsed = Validation.tryParseError(error);

            Messages.error({ main: { code: parsed.errorCode } }, parsed.placeholders, EAPI_Auth_RegistrationError);
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