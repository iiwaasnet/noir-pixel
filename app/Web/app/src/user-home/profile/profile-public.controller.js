(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePublicController', profilePublicController);

    profilePublicController.$inject = ['$rootScope', '$scope', '$filter', 'States', 'Url', 'Config', 'Strings', 'profileData', 'countries'];

    function profilePublicController($rootScope, $scope, $filter, States, Url, Config, Strings, profileData, countries) {
        var ctrl = this;
        ctrl.countries = $filter('orderBy')(countries.data, 'name');
        ctrl.country = undefined;
        ctrl.save = save;
        ctrl.upload = getUploadConfig();

        var unsubscribe;

        activate();

        function save() {

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

        function getUploadConfig() {
            var config = {
                endpoint: Url.build(Config.ApiUris.Base, Config.ApiUris.Profiles.UpdateProfileImage),
                description: Strings.getLocalizedString('ProfilePublic_ProfilePhotoDescription')
                    .format(Config.Profiles.Image.FullViewSize, Config.Profiles.Image.MaxFileSize)
            };
            return config;
        }
    }
})();