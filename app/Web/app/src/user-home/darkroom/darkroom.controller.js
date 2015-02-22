(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('DarkroomController', darkroomController);

    darkroomController.$inject = ['$interval', 'DelayedToggle', 'Url', 'Config', 'Photos', 'Overlay'];

    function darkroomController($interval, DelayedToggle, Url, Config, Photos, Overlay) {
        var ctrl = this,
            progressClearDelay = 1000;
        ctrl.loading = false;
        ctrl.updateProgress = updateProgress;
        ctrl.uploadCompleted = uploadCompleted;
        ctrl.fileUploadSuccess = fileUploadSuccess;
        ctrl.fileUploadError = fileUploadError;
        ctrl.filesAdded = filesAdded;
        ctrl.photoUpload = getPhotoUploadConfig();
        ctrl.loadProgress = undefined;
        ctrl.pendingPhotos = [];
        ctrl.currentlyLoading = [];

        activate();

        function activate() {
            getPendingPhotos();
        }

        function filesAdded(files) {
            if (files.length) {
                Overlay.open('app/src/user-home/darkroom/photos-upload.html',
                    'PhotosUploadController as ctrl',
                    { photos: files });
            }
            //ctrl.currentlyLoading = ctrl.currentlyLoading.concat(files);
        }

        function getPendingPhotos() {
            var toggle = DelayedToggle.on(ctrl, 'loading');
            Photos.getPendingPhotos()
                .then(getPendingPhotosSuccess, getPendingPhotosError)
                .then(function() { toggle.off(); });
        }

        function getPendingPhotosSuccess(response) {
            ctrl.pendingPhotos = response.data.photos;
        }

        function getPendingPhotosError(error) {
        }

        function updateProgress(loaded, files) {
            ctrl.loadProgress = loaded;
        }

        function fileUploadError(file, message) {
            debugger;
        }

        function fileUploadSuccess(file, response) {
            ctrl.pendingPhotos.splice(0, 0, angular.fromJson(response));
            clearFileFromHistory(file);
        }

        function clearFileFromHistory(file) {
            file.cancel();
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