(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalSignInController', externalSignInController);

    externalSignInController.$injector = ['$location', '$window', 'Auth'];

    function externalSignInController($location, $window, Auth) {
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
            var params = parseQueryString($location.hash());

            return {
                externalAccessToken: params.external_access_token,
                registered: params.registered === 'true'
            };
        }

        function getLocalTokenSuccess() {
        }

        function getLocalTokenError() {}

        function parseQueryString (queryString) {
            var parsed = {};

            var queryParams = queryString.split("&");
            angular.forEach(queryParams, function(param) { addTupple(param, parsed); });
            

            return parsed;
        };

        function addTupple(param, parsed) {
            var temp = param.split('=');
            if (temp.length === 2) {
                parsed[temp[0]] = temp[1];
            }
        }

    }
})();