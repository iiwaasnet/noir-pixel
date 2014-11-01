(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalSignInController', externalSignInController);

    externalSignInController.$injector = ['$location', '$window', 'Auth', 'Url'];

    function externalSignInController($location, $window, Auth, Url) {
        var ctrl = this;
            
        activate();
       

        function activate() {
            var loginResult = getExternalLoginResult();
            if (loginResult.registered) {
                Auth.getLocalToken(loginResult.externalAccessToken, 'Google')
                .then(getLocalTokenSuccess, getLocalTokenError);
            } else {
                Auth.registerExternal(loginResult.externalAccessToken, 'Google')
                .then(registerExternalSuccess, registerExternalError);
            }
        }

        function registerExternalSuccess(externalToken) {
            Auth.getLocalToken(externalToken, 'Google')
            .then(getLocalTokenSuccess, getLocalTokenError);
        }

        function registerExternalError(err) {
            
        }

        function getExternalLoginResult() {
            var params = Url.parseQueryString($location.hash());

            return {
                externalAccessToken: params.external_access_token,
                registered: params.registered === 'true'
            };
        }

        function getLocalTokenSuccess() {
        }

        function getLocalTokenError() {}
    }
})();