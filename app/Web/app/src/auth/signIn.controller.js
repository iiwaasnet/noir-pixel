(function() {
    'use strict';

    angular.module('np.auth')
        .controller('SignInController', signInController);

    signInController.$inject = ['$stateParams', '$location', '$http', '$window', '$scope', '$state', 'Storage', 'Messages', 'loginOptions'];

    function signInController($stateParams, $location, $http, $window, $scope, $state, Storage, Messages, loginOptions) {
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
            if (result.succeeded) {
                //$stateParams.go(getLoginRedirectState());
            } else {
                Messages.error({ main: { id: result.error } });
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
                case 'GooglePlus':
                    login.displayOrder = 1;
                default:
            }
        }

        function signin(url) {
            saveLoginRedirectState(redirectTo);
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