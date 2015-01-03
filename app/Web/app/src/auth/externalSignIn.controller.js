(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalSignInController', externalSignInController);

    externalSignInController.$inject = ['$location', '$window', '$state', 'Url', 'Signin'];

    function externalSignInController($location, $window, $state, Url, Signin) {
        activate();

        function activate() {
            var loginResult = getExternalLoginResult();

            if (loginResult.error) {
                Signin.finalizeSigninFailed(loginResult.error);
            }

            if (loginResult.registered) {
                Signin.finalizeSignin(loginResult);
            } else {
                $state.go('externalRegister',
                {
                    //TODO: Get additionaly userName with server response, to suggest it as a username??
                    external_access_token: loginResult.externalAccessToken,
                    access_token_secret: loginResult.accessTokenSecret,
                    provider: loginResult.provider
                });
            }
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
                accessTokenSecret: params.access_token_secret,
                registered: params.registered === 'true',
                provider: params.provider
            };
        }
    }
})();