(function() {
    'use strict';

    angular.module('np.auth')
        .controller('SignInController', signInController);

    signInController.$injector = ['Auth'];

    function signInController(Auth) {
        var ctrl = this;
        ctrl.signIn = signIn;
        ctrl.userName = '';
        ctrl.password = '';
        ctrl.signInAllowed = true;


        function boo() {
            var subscriptions = {};
            var evnt = 'auth';

            if (!~Object.keys(subscriptions).indexOf(evnt)) {
                subscriptions[evnt] = {signInSucceeded: signInSucceeded};
            }
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
        }

        function signInFailed(err) {
            alert(err);
        }

        function enableSignIn() {
            ctrl.signInAllowed = true;
        }
    }
})();