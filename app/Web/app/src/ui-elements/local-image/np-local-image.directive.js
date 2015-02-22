(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npLocalImage', localImage);

    function localImage() {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            if (element[0].tagName === 'IMG') {
                if (attrs.npLocalImage) {
                    scope.$watch(attrs.npLocalImage, readLocalImage);
                }
            }

            function readLocalImage(file) {
                if (file) {
                    var fileReader = new FileReader();
                    fileReader.onload = setTargetImage;
                    fileReader.readAsDataURL(file);
                }
            }

            function setTargetImage(event) {
                scope.$evalAsync(function () {
                    attrs.$set('src', event.target.result);
                });
            }
        }
    }
})();