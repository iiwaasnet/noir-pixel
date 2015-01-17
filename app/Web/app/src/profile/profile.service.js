(function() {
    'use strict';

    angular.module('np.profile')
        .service('Profile', profileService);

    profileService.$inject = ['$http', '$q', 'Config', 'Url', 'User'];

    function profileService($http, $q, Config, Url, User) {
        var srv = this;
        srv.getUserProfile = getUserProfile;
        srv.getOwnProfile = getOwnProfile;
        srv.getCountries = getCountries;

        function getUserProfile(userName) {
            if (userName) {
                var url = Url.build(Config.ApiUris.Base, Config.ApiUris.Profiles.Profile).formatNamed({ userName: userName });
                return $http.get(url);
            }

            return $q.reject();
        }

        function getCountries() {
            var url = Url.build(Config.ApiUris.Base, Config.ApiUris.Profiles.Countries);
            return $http.get(url);
        }

        function getOwnProfile() {
            var userData = User.getUserData() || {userName: ''};

            var deferred = $q.defer();

            getUserProfile(userData.userName).then(
                function (response) { getProfileSuccess(response, deferred); },
                function(reason) { getProfileError(reason, deferred); });

            return deferred.promise;
        }

        function getProfileSuccess(response, deferred) {
            User.updateUserData({
                thumbnail: response.data.publicInfo.thumbnail,
                fullName: response.data.user.fullName
            });

            deferred.resolve(response);
        }

        function getProfileError(reason, deferred) {
            deferred.reject(reason);
        }
    }
})();