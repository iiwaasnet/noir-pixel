(function() {
    'use strict';

    angular.module('np.auth')
        .controller('ExternalSignInController', externalSignInController);

    externalSignInController.$injector = ['$location', 'Auth'];

    function externalSignInController($location, Auth) {
        var ctrl = this;
            

        activate();

        function activate() {
            var accessToken = getTokenFromUrl();
            Auth.externalSignUp(accessToken);
        }

        function getTokenFromUrl() {
            var token = $location.hash();
            if (token && token !== '') {
                token = token.split('=');
                if (token.length > 0) {
                    return token[1].split('&')[0];
                }
            }
            return '';
        }
    }
})();