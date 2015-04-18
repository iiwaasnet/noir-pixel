(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('DarkroomController', darkroomController);

    darkroomController.$inject = ['$scope', 'Url', 'Config', 'Photos', 'Overlay', 'Strings', 'Messages', 'DarkroomModes'];

    function darkroomController($scope, Url, Config, Photos, Overlay, Strings, Messages, DarkroomModes) {
        var ctrl = this,
            EAPI_Image_Unknown = 'EAPI_Image_Unknown',
            currentlyUploading = [];
        ctrl.mode = DarkroomModes.Edit;
        ctrl.updateProgress = updateProgress;
        ctrl.uploadCompleted = uploadCompleted;
        ctrl.fileUploadSuccess = fileUploadSuccess;
        ctrl.fileUploadError = fileUploadError;
        ctrl.filesAdded = filesAdded;
        ctrl.action = action;
        ctrl.photoUpload = getPhotoUploadConfig();
        ctrl.pendingPhotos = [];
        ctrl.addPhotosMode = addPhotosMode;

        activate();

        function addPhotosMode(on) {
            switchMode(on, DarkroomModes.AddToPhotos);
        }

        function switchMode(on, toMode) {
            $scope.$evalAsync(function() {
                ctrl.mode = on ? toMode : DarkroomModes.Edit;
            });
        }

        function activate() {
            getPendingPhotos();
        }

        function action(photo) {
            switch (mode) {
            case DarkroomModes.Edit:
                edit(photo);
                return;
            default:
            }
        }

        function filesAdded(files) {
            currentlyUploading = [];
            if (files.length) {
                currentlyUploading = getCurrentlyUploadingFiles(files);
                Overlay.open('app/src/user-home/darkroom/photos-upload.html',
                    'PhotosUploadController as ctrl',
                    { photos: currentlyUploading });
            }
        }

        function getCurrentlyUploadingFiles(files) {
            var tmp = [];
            angular.forEach(files, function(file) { tmp.push(wrapFileForUpload(file)); });

            return tmp;
        }

        function wrapFileForUpload(file) {
            return {
                file: file,
                isCompleted: false
            };
        }

        function getPendingPhotos() {
            Photos.getPendingPhotos()
                .then(getPendingPhotosSuccess, getPendingPhotosError);
        }

        function getPendingPhotosSuccess(response) {
            ctrl.pendingPhotos = response.data.photos;
        }

        function getPendingPhotosError(error) {
        }

        function updateProgress(loaded, files) {
        }

        function fileUploadError(file, message) {
            clearFileFromHistory(file);

            var item = currentlyUploading.first(function(f) {
                return f.file === file;
            });
            if (item) {
                item.error = Strings.getLocalizedMessage(angular.fromJson(message), null, EAPI_Image_Unknown);
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
        }

        function getPhotoUploadConfig() {
            var config = {
                endpoint: Url.buildApiUrl(Config.ApiUris.Photos.Upload)
            };
            return config;
        }
    }
})();