(function() {
    'use strict';

    angular.module('np.auth')
        .service('Signin', signinService);

    signinService.$inject = ['$q', '$window', 'Auth', 'ngDialog', 'Progress', 'Messages', 'ApplicationLogging'];

    function signinService($q, $window, Auth, ngDialog, Progress, Messages, ApplicationLogging) {
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
                Progress.start();
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
            Progress.start();

            return Auth.registerExternal(externalLogin, userName)
                .then(registerExternalSuccess, registerExternalError);
        }

        function registerExternalError(error) {
            Progress.stop();

            return $q.reject(error.data);
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
            Progress.start();
            return Auth.getLocalToken(externalLogin)
                .then(getLocalTokenSuccess, getLocalTokenFailed);
        }

        function registerExternalSuccess(response) {
            Progress.stop();

            return response;
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

            Progress.stop();
        }

        function onUIClose() {
            ui = undefined;
        }

        function getAvailableLoginsError(error) {
            Progress.stop();
            Messages.error(error);
        }

        function getLocalTokenSuccess(response) {
            Progress.stop();

            return response;
        }

        function getLocalTokenFailed(error) {
            Progress.stop();
            ApplicationLogging.error(error);

            return $q.reject(error);
        }
    }
})();