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
        srv.externalSignin = externalSignin;
        srv.registerExternal = registerExternal;
        srv.finalizeSigninSuccess = finalizeSigninSuccess;
        srv.finalizeSigninError = finalizeSigninError;

        function open() {
            if (!ui) {
                Progress.start();
                Auth.getAvailableLogins().then(getAvailableLoginsSuccess, getAvailableLoginsError);
            }
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

        function externalSignin(externalLogin) {
            Progress.start();
            getLocalToken(externalLogin);
        }

        function getLocalToken(externalLogin) {
            Auth.getLocalToken(externalLogin)
                .then(finalizeSigninSuccess, finalizeSigninError);
        }

        function registerExternalSuccess(response) {
            getLocalToken({
                externalAccessToken: response.data.access_token,
                accessTokenSecret: response.data.access_token_secret,
                provider: response.data.provider
            });
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

        function finalizeSigninError(error) {
            $window.opener.$scope.finalizeLogin({
                succeeded: false,
                error: error
            });
            ApplicationLogging.error(error);

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