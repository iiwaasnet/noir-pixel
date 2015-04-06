(function() {
    'use strict';

    angular.module('np.user-home')
        .controller('EditPhotoController', editPhotoController);

    editPhotoController.$inject = ['$scope', '$filter', 'Moment', 'Strings', 'Photos', 'photo'];

    function editPhotoController($scope, $filter, Moment, Strings, Photos, photo) {
        var ctrl = this,
            labelPrefix = 'Label_Exif_',
            formatPrefix = 'Format_Exif_',
            photoDate = 'dateTimeTaken',
            placeholderPrefix = 'Hint_Exif_';
        photo.description.exif = transformExifData(photo.description.exif);
        photo.description.tags = transformTags(photo.description.tags);
        ctrl.photo = photo;
        ctrl.genres = [];
        ctrl.save = save;
        ctrl.close = close;

        activate();

        function activate() {
            $scope.$watch(trackTagsChanges, invalidateForm);
            Photos.getPhotoGenres().then(assignGenres);
        }

        function close() {
            $scope.closeThisDialog();
        }

        function save() {
            Photos.updatePhoto(ctrl.photo.image.id, createUpdateObject())
                .then(updatePhotoSuccess, updatePhotoError);
        }

        function updatePhotoSuccess() {
            ctrl.close();
        }

        function updatePhotoError() {
            debugger;
        }

        function createUpdateObject() {
            return {
                title: ctrl.photo.description.title,
                story: ctrl.photo.description.story,
                genre: ctrl.photo.description.genre,
                exif: getExif(),
                tags: getTags()
            };
        }

        function getTags() {
            ctrl.photo.description.tags.filter(function(t) { return t.selected; });
        }

        function getExif() {
            var exifData = {};
            ctrl.photo.description.exif.forEach(function (exifTag) {
                exifData[exifTag.name] = exifTag.editValue;
            });
        }

        function assignGenres(response) {
            ctrl.genres = (response && response.data)
                ? $filter('orderBy')(response.data, 'name')
                : ctrl.genres;
        }

        function trackTagsChanges() {
            return ctrl.photo.description.tags.filter(function(t) { return t.selected; }).length;
        }

        function invalidateForm() {
            if ($scope.editPhoto) {
                $scope.editPhoto.$setDirty();
            }
        }

        function transformTags(photoTags) {
            var tags = [];
            (photoTags || []).forEach(function (tag) {
                tags.push({
                    tag: tag,
                    selected: true
                });
            });

            return tags;
        }

        function transformExifData(exifData) {
            var exifTags = [];
            Object.keys(exifData || {}).forEach(function(key) {
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