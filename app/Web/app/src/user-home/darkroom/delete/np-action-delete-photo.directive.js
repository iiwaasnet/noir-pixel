(function() {
    'use strict';

    angular.module('np.user-home')
        .directive('npActionDeletePhoto', actionDeletePhoto);

    function actionDeletePhoto() {
        var dir = {
            restrict: 'E',
            templateUrl: '/app/src/user-home/darkroom/delete/action-delete-photo.html',
            scope: {
                photo: '='
            },
            controller: ['Overlay', 'Messages', 'Photos', controller],
            controllerAs: 'editPhotoCtrl'
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
                Overlay.open('app/src/user-home/darkroom/edit/edit-photo.html',
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