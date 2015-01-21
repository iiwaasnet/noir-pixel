(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePublicController', profilePublicController);

    profilePublicController.$inject = ['$rootScope', '$scope', '$filter', 'States', 'profileData', 'countries'];

    function profilePublicController($rootScope, $scope, $filter, States, profileData, countries) {
        var ctrl = this;
        ctrl.countries = $filter('orderBy')(countries.data, 'name');
        ctrl.country = undefined;
        ctrl.groupCountries = groupCountries;
        ctrl.save = save;

        var unsubscribe;

        activate();

        function save() {

        }

        function groupCountries(item) {
            return item.name[0].toUpperCase();
        }

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