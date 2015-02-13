(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('DarkroomController', darkroomController);

    darkroomController.$inject = ['$scope', 'Url', 'Config'];

    function darkroomController($scope ,Url, Config) {
        var ctrl = this;
        ctrl.progress = progress;
        ctrl.imageUpload = getImageUploadConfig();
        ctrl.loadProgress = 0;

        function progress(loaded) {
            ctrl.loadProgress = loaded * 100;
        }

        function getImageUploadConfig() {
            var config = {
                endpoint: Url.build(Config.ApiUris.Base, Config.ApiUris.Profiles.UpdateProfileImage)
            };
            return config;
        }
    }
})();