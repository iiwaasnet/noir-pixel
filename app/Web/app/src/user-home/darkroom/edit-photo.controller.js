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
            placeholderPrefix = 'Hint_Exif_';
        photo.exif = transformExifData(photo.exif);
        ctrl.photo = photo;

        function transformExifData(exifData) {
            var exifTags = [];
            Object.keys(exifData).forEach(function (key) {
                var exif = {
                    name: key,
                    label: getExifTagLabel(key),
                    displayValue: getExifDisplayValue(key, exifData[key]),
                    editValue: getExifTagEditValue(key, exifData[key]),
                    placeholder: getExifTagPlaceholder(key)
                };

                exifTags.push(exif);
            });

            return exifTags;
        }

        function getExifTagPlaceholder(tagName) {
            return getExifString(placeholderPrefix, tagName);
        }

        function getExifTagLabel(exifTag) {
            return getExifString(labelPrefix, exifTag);
        }

        function getExifTagFormat(exifTag) {
            return getExifString(formatPrefix, exifTag);
        }


        function getExifTagEditValue(tagName, tagValue) {
            if (tagValue && tagName === photoDate) {
                var formatString = getExifTagFormat(tagName);

                return Moment(tagValue).format(formatString || 'YYYY-MM-DD');
            }
            return tagValue;
        }

        function getExifDisplayValue(tagName, tagValue) {
            if (tagValue) {
                var formatString = getExifTagFormat(tagName);

                if (tagName === photoDate) {
                    return Moment(tagValue).format(formatString || 'YYYY-MM-DD');
                }
                if (formatString) {
                    return formatString.format(tagValue);
                }
            }
            tagValue = tagValue || getExifTagPlaceholder(tagName);
            return tagValue;
        }

        function getExifString(prefix, exifTag) {
            var labelName = prefix + exifTag.capitalize();
            return Strings.getLocalizedString(labelName);
        }
    }
})();