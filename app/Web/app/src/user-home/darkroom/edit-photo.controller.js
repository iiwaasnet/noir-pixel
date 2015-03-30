﻿(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('EditPhotoController', editPhotoController);

    editPhotoController.$inject = ['$scope', 'Moment', 'Strings', 'photo'];

    function editPhotoController($scope, Moment, Strings, photo) {
        var ctrl = this,
            labelPrefix = 'Label_Exif_',
            formatPrefix = 'Format_Exif_',
            photoDate = 'dateTimeTaken',
            placeholderPrefix = 'Hint_Exif_';
        photo.exif = transformExifData(photo.exif);
        ctrl.photo = photo;
       

        function transformExifData(exifData) {
            var exifTags = [];
            Object.keys(exifData).forEach(function(key) {
                var exif = {
                    name: key,
                    label: getExifTagLabel(key),
                    displayValue: getExifDisplayValue(key, exifData[key]),
                    editValue: getExifTagEditValue(key, exifData[key]),
                    placeholder: getExifTagPlaceholder(key)
                };

                exifTags.push(exif);
                $scope.$watch(function() { return exif.editValue; }, function(v) { setDisplayValue(exif, key, v); });
            });

            return exifTags;
        }

        function setDisplayValue(tag, tagName, value) {
            tag.displayValue = getExifDisplayValue(tagName, value);
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
                var date = Moment(tagValue);

                return (date.isValid())
                    ? date.format(formatString || 'YYYY-MM-DD')
                    : undefined;
            }
            return tagValue;
        }

        function getExifDisplayValue(tagName, tagValue) {
            if (tagValue) {
                var formatString = getExifTagFormat(tagName);

                if (tagName === photoDate) {
                    var date = Moment(tagValue);

                    return (date.isValid())
                        ? date.format(formatString || 'YYYY-MM-DD')
                        : undefined;

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