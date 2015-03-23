(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('EditPhotoController', editPhotoController);

    editPhotoController.$inject = ['Strings', 'photo'];

    function editPhotoController(Strings, photo) {
        var ctrl = this,
            labelPrefix = 'Label_Exif_';
        photo.exif = transformExifData(photo.exif);
        ctrl.photo = photo;

        function transformExifData(exifData) {
            var exif = [];
            Object.keys(exifData).forEach(function(key) {
                exif.push({
                    name: key,
                    label: getExifTagLabel(key),
                    value: exifData[key]
                });
            });
            
            return exif;
        }

        function getExifTagLabel(exifTag) {
            var labelName = labelPrefix + exifTag.capitalize();
            return Strings.getLocalizedString(labelName);
        }
    }
})();