(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfileController', profileController);

    profileController.$inject = ['$rootScope', '$scope', 'States'];

    function profileController($rootScope, $scope, States) {
        var unsubscribe;

        activate();

        function activate() {
            $scope.$on('$destroy', cleanup);
            unsubscribe = $rootScope.$on('$stateChangeStart', stateChangeStart);
        }

        function stateChangeStart(event, toState, toParams, fromState, fromParams) {
            if (fromState.name === States.UserHome.Profile.Name) {
                event.preventDefault();
            }
        }

        function cleanup() {
            unsubscribe && unsubscribe();
        }
    }
})();
