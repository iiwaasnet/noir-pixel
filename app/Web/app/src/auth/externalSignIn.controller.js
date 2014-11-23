(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalSignInController', externalSignInController);

    externalSignInController.$inject = ['$location', '$window', 'Url', 'Signin'];

    function externalSignInController($location, $window, Url, Signin) {
        activate();
       
        function activate() {
            var loginResult = getExternalLoginResult();
            Signin.externalSignin(loginResult);
            
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
    }
})();