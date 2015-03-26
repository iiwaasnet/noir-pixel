(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('DarkroomController', darkroomController);

    darkroomController.$inject = ['Url', 'Config', 'Photos', 'Overlay', 'Strings', 'Messages'];

    function darkroomController(Url, Config, Photos, Overlay, Strings, Messages) {
        var ctrl = this,
            EAPI_Image_Unknown = 'EAPI_Image_Unknown',
            currentlyUploading = [];
        ctrl.updateProgress = updateProgress;
        ctrl.uploadCompleted = uploadCompleted;
        ctrl.fileUploadSuccess = fileUploadSuccess;
        ctrl.fileUploadError = fileUploadError;
        ctrl.filesAdded = filesAdded;
        ctrl.edit = edit;
        ctrl.photoUpload = getPhotoUploadConfig();
        ctrl.pendingPhotos = [];

        activate();

        function activate() {
            getPendingPhotos();
        }

        function edit(photo) {
            Photos.getPhotoForEdit(photo.id)
                .then(getPhotoForEditSuccess, getPhotoForEditError);
        }

        function getPhotoForEditSuccess(response) {
            Overlay.open('app/src/user-home/darkroom/edit-photo.html',
                'EditPhotoController as ctrl',
                { photo: response.data },
                { closeByEscape: false });
        }

        function getPhotoForEditError(error) {
            Messages.error({ main: { code: error } });
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