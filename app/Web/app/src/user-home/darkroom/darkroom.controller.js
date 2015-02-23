(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('DarkroomController', darkroomController);

    darkroomController.$inject = ['$interval', 'DelayedToggle', 'Url', 'Config', 'Photos', 'Overlay'];

    function darkroomController($interval, DelayedToggle, Url, Config, Photos, Overlay) {
        var ctrl = this,
            progressClearDelay = 1000,
            currentlyUploading = [];
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
            currentlyUploading = [];
            if (files.length) {
                currentlyUploading = getCurrentlyUploadingFiles(files);
                Overlay.open('app/src/user-home/darkroom/photos-upload.html',
                    'PhotosUploadController as ctrl',
                    { photos: currentlyUploading });
            }
            //ctrl.currentlyLoading = ctrl.currentlyLoading.concat(files);
        }

        function getCurrentlyUploadingFiles(files) {
            var tmp = [];
            angular.forEach(files, function (file) { tmp.push(wrapFileForUpload(file)); });

            return tmp;
        }

        function wrapFileForUpload(file) {
            return {
                file: file,
                isCompleted: false
            };
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
            var item = currentlyUploading.first(function (f) {
                return f.file === file;
            });
            if (item) {
                item.error = message || 'bla';
            }
        }

        function fileUploadSuccess(file, response) {
            ctrl.pendingPhotos.splice(0, 0, angular.fromJson(response));
            markFileCompleted(file);
        }

        function markFileCompleted(file) {
            var item = currentlyUploading.first(function(f) {
                return f.file === file;
            });
            if (item) {
                item.isCompleted = true;
            }
            clearFileFromHistory(file);
        }

        function clearFileFromHistory(file) {
            file.cancel();
        }

        function uploadCompleted() {
            if (!currentlyUploading.any(function(f) { return f.error; })) {
                Overlay.close();
            }
            //$interval(function() { ctrl.loadProgress = 0; }, progressClearDelay, 1);
        }

        function getPhotoUploadConfig() {
            var config = {
                endpoint: Url.buildApiUrl(Config.ApiUris.Photos.Upload)
            };
            return config;
        }
    }
})();