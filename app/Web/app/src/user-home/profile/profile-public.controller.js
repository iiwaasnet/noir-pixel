(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePublicController', profilePublicController);

    profilePublicController.$inject = ['$rootScope', '$scope', 'States', 'Profile', 'profileData'];

    function profilePublicController($rootScope, $scope, States, Profile, profileData) {
        var ctrl = this;
        ctrl.countries = [
            { code: 'UA', name: 'Ukraine' },
            { code: 'DE', name: 'Germany' },
            { code: 'US', name: 'United States' }
        ];
        ctrl.country = [{ code: null }];

        var unsubscribe;

        activate();

        function activate() {
            $scope.$on('$destroy', cleanup);
            unsubscribe = $rootScope.$on('$stateChangeStart', stateChangeStart);
        }

        function stateChangeStart(event, toState, toParams, fromState, fromParams) {
            if (fromState.name === States.UserHome.Profile.Public.Name) {
            }
        }

        function cleanup() {
            unsubscribe && unsubscribe();
        }
    }
})();