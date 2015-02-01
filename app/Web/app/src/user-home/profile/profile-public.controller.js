(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePublicController', profilePublicController);

    profilePublicController.$inject = ['$rootScope', '$scope', '$filter', 'States', 'Url', 'Config', 'Strings', 'Profile', 'profileData', 'countries'];

    function profilePublicController($rootScope, $scope, $filter, States, Url, Config, Strings, Profile, profileData, countries) {
        var ctrl = this;
        ctrl.countries = $filter('orderBy')(countries.data, 'name');
        ctrl.country = undefined;
        ctrl.save = save;
        ctrl.imageUpload = getImageUploadConfig();
        ctrl.profileData = profileData.data;
        ctrl.refershProfileImage = refershProfileImage;
        ctrl.deleteProfileImage = deleteProfileImage;
        var unsubscribe;

        activate();

        function deleteProfileImage() {
            Profile.deleteProfileImage().then(deleteProfileImageSuccess);
        }

        function deleteProfileImageSuccess() {
            ctrl.profileData.publicInfo.profileImage = '';
        }

        function save() {

        }

        function refershProfileImage() {
            Profile.getOwnProfile().then(getProfileSuccess);
        }

        function getProfileSuccess(response) {
            ctrl.profileData.publicInfo.profileImage = response.data.publicInfo.profileImage;
        }

        function stateChangeStart(event, toState, toParams, fromState, fromParams) {
            if (fromState.name === States.UserHome.Profile.Public.Name) {
            }
        }

        function cleanup() {
            unsubscribe && unsubscribe();
        }

        function getImageUploadConfig() {
            var config = {
                endpoint: Url.build(Config.ApiUris.Base, Config.ApiUris.Profiles.UpdateProfileImage),
                description: Strings.getLocalizedString('ProfilePublic_ProfilePhotoDescription')
                    .format(Config.Profiles.Image.FullViewSize, Config.Profiles.Image.MaxFileSize)
            };
            return config;
        }

        function activate() {
            $scope.$on('$destroy', cleanup);
            unsubscribe = $rootScope.$on('$stateChangeStart', stateChangeStart);
        }

    }
})();