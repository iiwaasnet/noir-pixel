(function() {
    'use strict';

    angular.module('np.auth')
        .service('Signin', signinService);

    signinService.$inject = ['$window', 'Auth', 'ngDialog', 'Progress', 'Messages'];

    function signinService($window, Auth, ngDialog, Progress, Messages) {
        var srv = this,
            ui = undefined;
        srv.open = open;
        srv.close = close;
        srv.externalSignin = externalSignin;
        srv.registerExternal = registerExternal;

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

        function registerExternal(loginResult, userName) {
            Progress.start();
            //TODO: Probably move finalizeSigninError to externalSignin.controller
            if (loginResult.error) {
                finalizeSigninError(loginResult.error);
            } else {
                Auth.registerExternal(loginResult.externalAccessToken, loginResult.accessTokenSecret, loginResult.provider, userName)
                    .then(registerExternalSuccess, finalizeSigninError);
            }
        }

        function externalSignin(loginResult) {
            Progress.start();
            //TODO: Probably move finalizeSigninError to externalSignin.controller
            if (loginResult.error) {
                finalizeSigninError(loginResult.error);
            } else {
                getLocalToken(loginResult.externalAccessToken, loginResult.accessTokenSecret, loginResult.provider);
            }
        }

        function getLocalToken(externalAccessToken, accessTokenSecret, provider) {
            Auth.getLocalToken(externalAccessToken, accessTokenSecret, provider)
                .then(finalizeSigninSuccess, finalizeSigninError);
        }

        function registerExternalSuccess(response) {
            getLocalToken(response.data.access_token, response.data.access_token_secret, response.data.provider);
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