(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('DarkroomController', darkroomController);

    darkroomController.$inject = ['$interval', 'Url', 'Config', 'Photos'];

    function darkroomController($interval, Url, Config, Photos) {
        var ctrl = this,
            progressClearDelay = 1000;
        ctrl.loading = false;
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
            DelayedToggle.on(function () { ctrl.loading = true; });
            DelayedToggle.on(ctrl, 'loading');

            ctrl.loading = true;
            Photos.getPendingPhotos()
                .then(getPendingPhotosSuccess, getPendingPhotosError)
                .then(function() { ctrl.loading = false; });
        }

        function getPendingPhotosSuccess(response) {
            ctrl.pendingPhotos = response.data.photos;
        }

        function getPendingPhotosError(error) {
        }

        function updateProgress(loaded) {
            ctrl.loadProgress = loaded * 100;
        }

        function fileUploadSuccess(response) {
            ctrl.pendingPhotos.splice(0, 0, angular.fromJson(response));
        }

        function uploadCompleted() {
            $interval(function() { ctrl.loadProgress = 0; }, progressClearDelay, 1);
        }

        function getPhotoUploadConfig() {
            var config = {
                endpoint: Url.buildApiUrl(Config.ApiUris.Photos.Upload)
            };
            return config;
        }
    }
})();