(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('ProfilePublicController', profilePublicController);

    profilePublicController.$inject = ['$rootScope', '$scope', '$filter', 'States', 'Url', 'Config', 'Strings', 'Profile', 'Progress', 'profileData', 'countries'];

    function profilePublicController($rootScope, $scope, $filter, States, Url, Config, Strings, Profile, Progress, profileData, countries) {
        var ctrl = this;
        ctrl.countries = $filter('orderBy')(countries.data, 'name');
        ctrl.livesIn = getLivesIn();
        ctrl.save = save;
        ctrl.imageUpload = getImageUploadConfig();
        ctrl.profileData = profileData.data.publicInfo;
        ctrl.uploadProfileImage = uploadProfileImage;
        ctrl.uploadCompleted = uploadCompleted;
        ctrl.deleteProfileImage = deleteProfileImage;
        var unsubscribe;

        activate();

        function deleteProfileImage() {
            Profile.deleteProfileImage().then(deleteProfileImageSuccess);
        }

        function deleteProfileImageSuccess() {
            ctrl.profileData.profileImage = '';
        }

        function save() {
            Profile.updatePublicInfo({
                    userFullName: ctrl.profileData.user.fullName,
                    countryCode: ctrl.livesIn.country.code,
                    city: ctrl.livesIn.city
                })
                .then(updatePublicInfoSuccess);
        }

        function updatePublicInfoSuccess() {
            $scope.profilePublic.$setPristine();
        }

        function uploadProfileImage() {
            Progress.start();
        }

        function uploadCompleted() {
            Progress.stop();
            Profile.getOwnProfile().then(getProfileSuccess);
        }

        function getProfileSuccess(response) {
            ctrl.profileData.profileImage = response.data.publicInfo.profileImage;
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
                    .format(Config.Profiles.Image.FullViewSize, Config.Profiles.Image.MaxFileSizeMB)
            };
            return config;
        }

        function getLivesIn() {
            var livesIn = {
                city: undefined,
                country: {
                    code: undefined,
                    name: undefined
                }
            };
            if (profileData.data.publicInfo.livesIn) {
                livesIn.city = profileData.data.publicInfo.livesIn.city;
                livesIn.country.code = profileData.data.publicInfo.livesIn.countryCode;
                livesIn.country.name = profileData.data.publicInfo.livesIn.country;
            }
            return livesIn;
        }

        function activate() {
            $scope.$on('$destroy', cleanup);
            unsubscribe = $rootScope.$on('$stateChangeStart', stateChangeStart);
        }

    }
})();