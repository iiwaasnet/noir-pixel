(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalSignInController', externalSignInController);

    externalSignInController.$inject = ['$location', '$window', 'Auth', 'Url'];

    function externalSignInController($location, $window, Auth, Url) {
        var ctrl = this;
            
        activate();
       

        function activate() {
            var loginResult = getExternalLoginResult();
            if (loginResult.error) {
                alert(loginResult.error);
            } else {
                if (loginResult.registered) {
                    Auth.getLocalToken(loginResult.externalAccessToken, loginResult.provider)
                        .then(getLocalTokenSuccess, getLocalTokenError);
                } else {
                    Auth.registerExternal(loginResult.externalAccessToken, loginResult.provider)
                        .then(registerExternalSuccess, registerExternalError);
                }
            }
        }

        function registerExternalSuccess(externalToken) {
            Auth.getLocalToken(externalToken, 'GooglePlus')
            .then(getLocalTokenSuccess, getLocalTokenError);
        }

        function registerExternalError(err) {
            
        }

        function getExternalLoginResult() {
            var params = Url.parseQueryString($location.hash());

            if (params.error) {
                return {
                    error: params.error
                };
            }

            return {
                externalAccessToken: params.external_access_token,
                registered: params.registered === 'true',
                provider: params.provider
            };
        }

        function getLocalTokenSuccess() {
        }

        function getLocalTokenError() {}
    }
})();