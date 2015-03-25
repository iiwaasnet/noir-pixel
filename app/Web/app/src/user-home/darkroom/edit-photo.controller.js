(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('EditPhotoController', editPhotoController);

    editPhotoController.$inject = ['Moment', 'Strings', 'photo'];

    function editPhotoController(Moment, Strings, photo) {
        var ctrl = this,
            labelPrefix = 'Label_Exif_',
            formatPrefix = 'Format_Exif_',
            photoDate = 'dateTimeTaken',
            emptyExifValue = 'Exif_Empty';
        photo.exif = transformExifData(photo.exif);
        ctrl.photo = photo;

        function transformExifData(exifData) {
            var exif = [];
            Object.keys(exifData).forEach(function(key) {
                exif.push({
                    name: key,
                    label: getExifTagLabel(key),
                    value: format(key, exifData[key]),
                    empty: !exifData[key]
                });
            });

            return exif;
        }

        function getExifTagLabel(exifTag) {
            return getExifString(labelPrefix, exifTag);
        }

        function getExifTagFormat(exifTag) {
            return getExifString(formatPrefix, exifTag);
        }

        function getExifString(prefix, exifTag) {
            var labelName = prefix + exifTag.capitalize();
            return Strings.getLocalizedString(labelName);
        }

        function format(tagName, tagValue) {
            if (tagValue) {
                var formatString = getExifTagFormat(tagName);

                if (tagName === photoDate) {
                    return Moment(tagValue).format(formatString || 'YYYY-MM-DD');
                }
                if (formatString) {
                    return formatString.format(tagValue);
                }
            }
            tagValue = tagValue || Strings.getLocalizedString(emptyExifValue);
            return tagValue;
        }
    }
})();