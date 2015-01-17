(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePublicController', profilePublicController);

    profilePublicController.$inject = ['$rootScope', '$scope', 'States', 'Profile', 'profileData'];

    function profilePublicController($rootScope, $scope, States, Profile, profileData) {
        var ctrl = this;
        ctrl.countries = [];

        var unsubscribe;

        activate();

        function activate() {
            $scope.$on('$destroy', cleanup);
            unsubscribe = $rootScope.$on('$stateChangeStart', stateChangeStart);

            populateCountries();
        }

        function populateCountries() {
            ctrl.countries = [
                { code: 'AX', name: '&Aring;land Islands', selected: false },
                { code: 'UA', name: 'Ukraine', selected: false },
                { code: 'UK', name: 'United Kingdom', selected: false }
            ];
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