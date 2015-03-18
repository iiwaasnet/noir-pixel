(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('EditPhotoController', editPhotoController);

    editPhotoController.$inject = ['photo'];

    function editPhotoController(photo) {
        var ctrl = this;
        ctrl.photo = photo;
    }
})();