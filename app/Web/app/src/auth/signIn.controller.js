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

        function signIn() {
            ctrl.signInAllowed = false;
            Auth.signIn(ctrl.userName, ctrl.password)
                .then(signInSucceeded, signInFailed)
                .then(enableSignIn);
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