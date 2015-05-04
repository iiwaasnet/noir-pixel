(function() {
    'use strict';

    angular.module('np.user-home')
        .directive('npActionDeletePhoto', actionDeletePhoto);

    function actionDeletePhoto() {
        var dir = {
            restrict: 'E',
            templateUrl: '/app/src/user-home/darkroom/delete/action-delete-photo.html',
            scope: {
                photo: '=',
                photos: '='
            },
            controller: ['$scope', 'Messages', 'Photos', controller],
            controllerAs: 'deletePhotoCtrl'
        };

        return dir;

        function controller($scope, Messages, Photos) {
            var ctrl = this;
            ctrl.deletePhoto = deletePhoto;

            function deletePhoto() {
                Photos.deletePhoto($scope.photo.id)
                    .then(deletePhotoSuccess, deletePhotoError);
            }

            function deletePhotoSuccess(response) {
                removeDeletedPhotoFromCollection();
            }

            function deletePhotoError(error) {
                Messages.error({ main: { code: error } });
            }

            function removeDeletedPhotoFromCollection(shortId) {
                $scope.photos.forEach(function (el, index) {
                    if (el.id === $scope.photo.id) {
                        $scope.photos.splice(index, 1);

                        return;
                    }
                });
            }
        }
    }
})();