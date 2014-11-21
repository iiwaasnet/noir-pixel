(function() {
    'use strict';

    angular.module('np.auth')
        .service('Signin', signinService);

    signinService.$inject = ['$controller', '$rootScope', '$injector', '$http', 'Auth', 'ngDialog', 'Progress'];

    function signinService($controller, $rootScope, $injector, $http, Auth, ngDialog, Progress) {
        var srv = this;
        srv.signin = signin;

        function signin() {
            Progress.start();
            Auth.preCacheAvailableLogins().then(preCacheAvailableLoginsSuccess, preCacheAvailableLoginsError);
        }

        function preCacheAvailableLoginsSuccess() {
            ngDialog.open({
                template: 'app/src/auth/signin.html',
                appendTo: '#signin-menu-target',
                cache: true,
                className: 'dialog-theme-contextmenu',
                controller: 'SignInController as ctrl',
                showClose: false
            });

            Progress.stop();
        }

        function preCacheAvailableLoginsError() {
            Progress.stop();
        }
    }
})();