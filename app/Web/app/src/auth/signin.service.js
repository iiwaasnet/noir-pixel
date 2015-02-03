(function() {
    'use strict';

    angular.module('np.auth')
        .service('Signin', signinService);

    signinService.$inject = ['$q', '$window', 'Auth', 'ngDialog', 'Messages', 'ApplicationLogging'];

    function signinService($q, $window, Auth, ngDialog, Messages, ApplicationLogging) {
        var srv = this,
            ui = undefined;
        srv.open = open;
        srv.close = close;
        srv.registerExternal = registerExternal;
        srv.finalizeSignin = finalizeSignin;
        srv.finalizeSigninFailed = finalizeSigninFailed;
        srv.signin = signin;

        function open() {
            if (!ui) {
                Auth.getAvailableLogins().then(getAvailableLoginsSuccess, getAvailableLoginsError);
            }
        }

        function signin(externalLogin) {
            return getLocalToken(externalLogin);
        }

        function close() {
            if (ui) {
                ui.close();
                ui = undefined;
            }
        }

        function registerExternal(externalLogin, userName) {
            return Auth.registerExternal(externalLogin, userName);
        }

        function finalizeSigninFailed(error) {
            $window.opener.$scope.finalizeLogin({
                succeeded: false,
                error: error
            });
            ApplicationLogging.error(error);
            $window.close();
        }

        function finalizeSignin(externalLogin) {
            externalLogin.succeeded = true;
            $window.opener.$scope.finalizeLogin(externalLogin);
            $window.close();
        }

        function getLocalToken(externalLogin) {
            return Auth.getLocalToken(externalLogin)
                .then(getLocalTokenSuccess, getLocalTokenFailed);
        }

        function getAvailableLoginsSuccess(response) {
            ui = ngDialog.open({
                template: 'app/src/auth/signin.html',
                cache: true,
                className: 'dialog-theme-contextmenu',
                controller: 'SignInController as ctrl',
                showClose: false,
                preCloseCallback: onUIClose,
                locals: {
                    loginOptions: response
                }
            });
        }

        function onUIClose() {
            ui = undefined;
        }

        function getAvailableLoginsError(error) {
            Messages.error(error);
        }

        function getLocalTokenSuccess(response) {
            return response;
        }

        function getLocalTokenFailed(error) {
            ApplicationLogging.error(error);

            return $q.reject(error);
        }
    }
})();