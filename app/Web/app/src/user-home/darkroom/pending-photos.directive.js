(function() {
    'use strict';

    angular.module('np.user-home')
        .directive('npPendingPhotos', pendingPhotos);

    function pendingPhotos() {
        var dir = {
            restrict: 'E',
            templateUrl: '/app/src/user-home/darkroom/pending-photos.html',
            scope: {
                photos: '='
            },
            controller: ['Overlay', 'Messages', 'Photos', controller],
            controllerAs: 'pendingPhotosCtrl'
        };

        return dir;

       function controller(Overlay, Messages, Photos) {
            var ctrl = this;
            ctrl.edit = edit;

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
        }
    }
})();