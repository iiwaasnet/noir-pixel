(function() {
    'use strict';

    angular.module('np.auth')
        .controller('SignInController', signInController);

    function signInController(strings) {
        var ctrl = this;
        ctrl.signIn = signIn;
        ctrl.userName = '';
        ctrl.password = '';

        function signIn() {
            
        }
    }
})();