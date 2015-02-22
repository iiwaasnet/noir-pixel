(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('PhotosUploadController', photosUploadController);

    photosUploadController.$inject = ['photos'];

    function photosUploadController(photos) {
        var ctrl = this;
        ctrl.currentlyLoading = photos;
    }
})();