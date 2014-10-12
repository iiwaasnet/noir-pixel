(function() {
    'use strict';

    angular.module('np.auth')
        .controller('SignInController', signInController);

    function signInController() {
        var ctrl = this;
        ctrl.signIn = signIn;
        ctrl.userName = '';
        ctrl.password = '';

        function signIn() {
            
        }
    }
})();