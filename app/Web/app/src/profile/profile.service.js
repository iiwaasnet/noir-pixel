(function() {
    'use strict';

    angular.module('np.profile')
        .service('Profile', profileService);

    profileService.$inject = ['$http', '$q', 'Config', 'Url', 'User'];

    function profileService($http, $q, Config, Url, User) {
        var srv = this;
        srv.getUserProfile = getUserProfile;
        srv.getOwnProfile = getOwnProfile;
        srv.deleteProfileImage = deleteProfileImage;
        srv.updatePublicInfo = updatePublicInfo;
        srv.updatePrivateInfo = updatePrivateInfo;


        function updatePrivateInfo(privateInfo) {
            var url = Url.buildApiUrl(Config.ApiUris.Profiles.UpdatePrivateInfo);
            return $http.post(url, {
                email: privateInfo.email
            });
        }


        function updatePublicInfo(publicInfo) {
            var url = Url.buildUrl(Config.ApiUris.Profiles.UpdatePublicInfo);
            return $http.post(url, {
                userFullName: publicInfo.userFullName,
                livesIn: {
                    countryCode: publicInfo.countryCode,
                    city: publicInfo.city
                }
            });
        }

        function deleteProfileImage() {
            var deferred = $q.defer();

            var url = Url.buildUrl(Config.ApiUris.Profiles.DeleteProfileImage);
            $http.delete(url).then(
                function(response) { deleteProfileImageSuccess(deferred); },
                function(reason) { deleteProfileImageError(reason, deferred); });

            return deferred.promise;
        }

        function deleteProfileImageSuccess(deferred) {
            User.updateUserData({
                thumbnail: ''
            });

            deferred.resolve();
        }

        function deleteProfileImageError(reason, deferred) {
            deferred.reject(reason);
        }

        function getUserProfile(userName) {
            if (userName) {
                var url = Url.buildUrl(Config.ApiUris.Profiles.Profile).formatNamed({ userName: userName });
                return $http.get(url);
            }

            return $q.reject();
        }

        function getOwnProfile() {
            var userData = User.getUserData() || { userName: '' };

            var deferred = $q.defer();

            getUserProfile(userData.userName).then(
                function(response) { getProfileSuccess(response, deferred); },
                function(reason) { getProfileError(reason, deferred); });

            return deferred.promise;
        }

        function getProfileSuccess(response, deferred) {
            User.updateUserData({
                thumbnail: response.data.publicInfo.profileImageThumbnail,
                fullName: response.data.publicInfo.user.fullName
            });

            deferred.resolve(response);
        }

        function getProfileError(reason, deferred) {
            deferred.reject(reason);
        }
    }
})();