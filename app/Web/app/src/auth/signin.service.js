(function() {
    'use strict';

    angular.module('np.auth')
        .service('Signin', signinService);

    signinService.$inject = ['$controller', '$rootScope', '$injector', '$http', '$window', 'Auth', 'ngDialog', 'Progress', 'Messages'];

    function signinService($controller, $rootScope, $injector, $http, $window, Auth, ngDialog, Progress, Messages) {
        var srv = this;
        srv.signin = signin;
        srv.externalSignin = externalSignin;

        function signin() {
            Progress.start();
            Auth.getAvailableLogins().then(getAvailableLoginsSuccess, getAvailableLoginsError);
        }

        function externalSignin(loginResult) {
            Progress.start();
            if (loginResult.error) {
                finalizeSigninError(loginResult.error);
            } else {
                if (loginResult.registered) {
                    Auth.getLocalToken(loginResult.externalAccessToken, loginResult.provider)
                        .then(finalizeSigninSuccess, finalizeSigninError);
                } else {
                    Auth.registerExternal(loginResult.externalAccessToken, loginResult.provider)
                        .then(registerExternalSuccess, finalizeSigninError);
                }
            }
        }

        function registerExternalSuccess(externalToken) {
            Auth.getLocalToken(externalToken, 'GooglePlus')
                .then(finalizeSigninSuccess, finalizeSigninError);
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
            Messages.error({
                main: { id: 'Err_Auth_Unknown' }
            });
        }

        function finalizeSigninError(error) {
            $window.opener.$scope.finalizeLogin({
                succeeded: false,
                error: error
            });
            $window.close();
        }

        function finalizeSigninSuccess() {
            $window.opener.$scope.finalizeLogin({
                succeeded: true
            });
            $window.close();
        }
    }
})();