(function () {
    'use strict';

    angular.module('np.auth')
        .service('Signin', signinService);

    signinService.$inject = ['Auth'];

    function signinService(Auth) {
        var srv = this,
            loginOverlay = angular.element('#signin');
        srv.signin = signin;

        function signin() {

        }
    }
})();