(function() {
    'use strict';

    angular.module('np.auth')
        .controller('SignInController', signInController);

    signInController.$inject = ['$stateParams', '$location', '$http', '$window', '$scope', 'Auth'];

    function signInController($stateParams, $location, $http, $window, $scope, Auth) {
        var ctrl = this,
            redirectTo = $stateParams.redirectTo || '';
        ctrl.availableLogins = [];
        ctrl.signIn = signIn;
        ctrl.googleSignIn = googleSignIn;
        ctrl.getGoogleSignIn = getGoogleSignIn;
        $scope.alert = alert;
        ctrl.userName = '';
        ctrl.password = '';
        ctrl.signInUri = '';
        ctrl.signInAllowed = true;

        activate();

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

        function alert(msg) {
            alert(msg);
        }

        function boo() {
            var subscriptions = {};
            var evnt = 'auth';

            if (!~Object.keys(subscriptions).indexOf(evnt)) {
                subscriptions[evnt] = { signInSucceeded: signInSucceeded };
            }
        }


        function getGoogleSignIn() {
            $http.get('http://api.noir-pixel.com/account/external-logins?returnUrl=http%3A%2F%2Fnoir-pixel.com%2F&generateState=false')
                .success(function(response) {
                    ctrl.signInUri = response;
                });
        }

        function googleSignIn() {
            $window.location.href = Auth.googleSignIn('home');
        }


        function googleSignInSuccess(data) {
        }

        function googleSignInError(err) {
        }

        function signIn(valid) {
            boo();

            if (valid) {
                ctrl.signInAllowed = false;
                Auth.signIn(ctrl.userName, ctrl.password)
                    .then(signInSucceeded, signInFailed)
                    .then(enableSignIn);
            } else {
                alert('Correct the errors and try again!');
            }
        }

        function signInSucceeded(data) {
            alert('Welcome!');
            if (redirectTo) {
                $location.url(redirectTo);
            }
        }

        function signInFailed(err) {
            alert(err);
        }

        function enableSignIn() {
            ctrl.signInAllowed = true;
        }
    }
})();