(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('DarkroomController', darkroomController);

    darkroomController.$inject = ['$interval', 'Url', 'Config'];

    function darkroomController($interval ,Url, Config) {
        var ctrl = this,
            progressClearDelay = 1000;
        ctrl.updateProgress = updateProgress;
        ctrl.uploadCompleted = uploadCompleted;
        ctrl.imageUpload = getImageUploadConfig();
        ctrl.loadProgress = undefined;

        function updateProgress(loaded) {
            ctrl.loadProgress = loaded * 100;
        }

        function uploadCompleted() {
            $interval(function () { ctrl.loadProgress = 0; }, progressClearDelay, 1);
        }

        function getImageUploadConfig() {
            var config = {
                endpoint: Url.build(Config.ApiUris.Base, Config.ApiUris.Profiles.UpdateProfileImage)
            };
            return config;
        }
    }
})();