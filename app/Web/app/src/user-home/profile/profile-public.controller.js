(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePublicController', profilePublicController);

    profilePublicController.$inject = ['$rootScope', '$scope', 'States', 'profileData', 'countries'];

    function profilePublicController($rootScope, $scope, States, profileData, countries) {
        var ctrl = this;
        ctrl.countries = countries.data;
        ctrl.country = undefined;

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