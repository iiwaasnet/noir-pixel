(function() {
    'use strict';

    angular.module('np.auth')
        .service('SignIn', signInService);

    signInService.$injector = ['$state', 'localStorageService'];

    function signInService($state, localStorageService) {
        var service = this,
            signInState = 'signIn';

        service.signIn = signIn;

        function signIn() {
            saveCurrentLocation();
            $state.go(signInState);
        }

        function saveCurrentLocation() {
            var currentState = $state.current.name;
            if (currentState !== signInState) {
                localStorageService.set('loginRedirectState', $state.current.name);
            }
        }
    }
})();