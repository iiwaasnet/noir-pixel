(function() {
    'use strict';

    angular.module('np.auth')
        .controller('SignInController', signInController);

    signInController.$injector = ['$stateParams', '$location', '$http', '$window', 'Auth'];

    function signInController($stateParams, $location, $http, $window, Auth) {
        var ctrl = this,
            redirectTo = $stateParams.redirectTo || '';
        ctrl.signIn = signIn;
        ctrl.googleSignIn = googleSignIn;
        ctrl.getGoogleSignIn = getGoogleSignIn;
        ctrl.userName = '';
        ctrl.password = '';
        ctrl.signInUri = '';
        ctrl.signInAllowed = true;


        function boo() {
            var subscriptions = {};
            var evnt = 'auth';

            if (!~Object.keys(subscriptions).indexOf(evnt)) {
                subscriptions[evnt] = { signInSucceeded: signInSucceeded };
            }
        }


        function getGoogleSignIn() {
            //$http.get('http://api.noir-pixel.com/account/external-logins?returnUrl=%2F&generateState=false')
            $http.get('http://api.noir-pixel.com/account/external-logins?returnUrl=http%3A%2F%2Fnoir-pixel.com%2F&generateState=false')
                .success(function(response) {
                    debugger;
                    ctrl.signInUri = response;
            });
        }

        function googleSignIn() {
            $window.location.href = Auth.googleSignIn();
            //Auth.googleSignIn()
            //    .then(googleSignInSuccess, googleSignInError);
        }


        function googleSignInSuccess(data) {
            debugger;
        }

        function googleSignInError(err) {
            debugger;
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