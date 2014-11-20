(function() {
    'use strict';

    angular.module('np.auth')
        .controller('SignInController', signInController);

    signInController.$inject = ['$stateParams', '$location', '$http', '$window', '$scope', '$state', 'Storage', 'Auth', 'Signin'];

    function signInController($stateParams, $location, $http, $window, $scope, $state, Storage, Auth) {
        debugger;
        var ctrl = this,
            redirectTo = $stateParams.redirectTo || '',
            signInState = 'signIn',
            loginRedirectStorageKey = 'loginRedirectState';
        ctrl.availableLogins = [];
        ctrl.signin = signin;
        ctrl.googleSignIn = googleSignIn;
        $scope.finalizeLogin = finalizeLogin;
        ctrl.userName = '';
        ctrl.password = '';
        ctrl.signInUri = '';
        ctrl.signInAllowed = true;
        ctrl.show = false;

        activate();

        function finalizeLogin() {
        }

        function activate() {
            Auth.getAvailableLogins().then(getAvailableLoginsSuccess, getAvailableLoginsError);
        }

        function getAvailableLoginsSuccess(data) {
            ctrl.availableLogins = orderLogins(data);
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

        function getAvailableLoginsError(error) {

        }

        function googleSignIn() {

        }


        function googleSignInSuccess(data) {
        }

        function googleSignInError(err) {
        }

        function signin(url) {
            saveLoginRedirectState(redirectTo);
            $window.$scope = $scope;
            $window.open(url, "Signin", 'width=800, height=600');
        }

        function signInSucceeded(data) {
            $stateParams.go(getLoginRedirectState());
        }

        function signInFailed(err) {
            alert(err);
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