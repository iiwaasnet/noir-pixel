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
        ctrl.allowSignIn = true;

        function signIn() {
            ctrl.allowSignIn = false;
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
            ctrl.allowSignIn = true;
        }
    }
})();