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
            Auth.getAvailableLogins().then(getAvailableLoginsSuccess, getAvailableLoginsError);
        }

        function getAvailableLoginsSuccess(response) {
            ngDialog.open({
                template: 'app/src/auth/signin.html',
                cache: true,
                className: 'dialog-theme-contextmenu',
                controller: 'SignInController as ctrl',
                showClose: false,
                locals: {
                    loginOptions: response
                }
            });

            Progress.stop();
        }

        function getAvailableLoginsError() {
            Progress.stop();
        }
    }
})();