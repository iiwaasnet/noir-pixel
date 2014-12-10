(function() {
    'use strict';

    angular.module('np.auth')
        .controller('SignInController', signInController);

    signInController.$inject = ['$stateParams', '$window', '$scope', '$state', 'Storage', 'Messages', 'Signin', 'loginOptions'];

    function signInController($stateParams, $window, $scope, $state, Storage, Messages, Signin, loginOptions) {
        var ctrl = this,
            redirectTo = $stateParams.redirectTo || '',
            signInState = 'signIn',
            loginRedirectStorageKey = 'loginRedirectState';
        ctrl.availableLogins = [];
        ctrl.signin = signin;
        $scope.finalizeLogin = finalizeLogin;
        ctrl.userName = '';
        ctrl.password = '';
        ctrl.signInUri = '';
        ctrl.signInAllowed = true;
        ctrl.show = false;

        activate();

        function activate() {
            ctrl.availableLogins = orderLogins(loginOptions);
        }

        function finalizeLogin(result) {
            try {
                if (result.succeeded) {
                    Signin.close();
                    if (result.newRegistration) {
                        Messages.message({main: {message: 'Welcome, my dear friend!'} });
                    }
                    //TODO: Refresh current view?
                    // registerExternal.controller has a close method, which means, user didn't register
                    // Refreshing current view might not make sense. Think of providing TRUE/FALSE to distinguish...
                    //$state.go(getLoginRedirectState());
                } else {
                    Messages.error({ main: { code: result.error } });
                }
            } catch (e) {
            }
        }

        function orderLogins(logins) {
            angular.forEach(logins, assignOrder);
            return logins;
        }

        function assignOrder(login) {
            switch (login.provider) {
            case 'Facebook':
                login.displayOrder = 0;
                break;
            case 'GooglePlus':
                login.displayOrder = 1;
                break;
            case 'Twitter':
                login.displayOrder = 2;
                break;
            case 'Yahoo':
                login.displayOrder = 3;
                break;
            default:
            }
        }

        function signin(url) {
            //saveLoginRedirectState(redirectTo);
            $window.$scope = $scope;
            $window.open(url, "Signin", 'width=800, height=600');
        }

        function enableSignIn() {
            ctrl.signInAllowed = true;
        }

        function saveLoginRedirectState(redirectState) {
            if (redirectState !== signInState) {
                Storage.set(loginRedirectStorageKey, redirectState);
            }
        }

        function getLoginRedirectState() {
            var redirectState = Storage.get(loginRedirectStorageKey);

            return redirectState || 'home';
        }
    }
})();