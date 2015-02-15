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
        ctrl.fileUploadSuccess = fileUploadSuccess;
        ctrl.photoUpload = getPhotoUploadConfig();
        ctrl.loadProgress = undefined;
        ctrl.pendingPhotos = [];

        activate();

        function activate() {
            getPendingPhotos();
        }

        function getPendingPhotos() {
            
        }

        function updateProgress(loaded) {
            ctrl.loadProgress = loaded * 100;
        }

        function fileUploadSuccess(response) {
        }

        function uploadCompleted() {
            $interval(function () { ctrl.loadProgress = 0; }, progressClearDelay, 1);
        }

        function getPhotoUploadConfig() {
            var config = {
                endpoint: Url.buildUrl(Config.ApiUris.Photos.Upload)
            };
            return config;
        }
    }
})();