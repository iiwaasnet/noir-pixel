(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npImgQuasiCrop', imgQausiCrop);

    function imgQausiCrop() {
        var dir = {
            restrict: 'E',
            link: link,
            transclude: true,
            templateUrl: '/app/src/ui-elements/img-quasi-crop/img-quasi-crop.html',
            scope: {
                size: '='
            }
        };

        return dir;

        function link(scope, element, attrs) {
            var image = angular.element(element[0].getElementsByTagName('img'));
            if (image) {
                var inMem = angular.element('<img/>');
                var imageSrc = image.attr('src');
                if (!imageSrc) {
                    scope.$watch(function() { return image.attr('src'); }, loadImage);
                } else {
                    loadImage(imageSrc);
                }
            }

            function loadImage(src) {
                if (src) {
                    inMem.on('load', cropImage);
                    inMem.attr('src', src);
                }
            }

            function cropImage() {
                var size = scope.size + 'px';
                if (inMem[0].width > inMem[0].height) {
                    image.css('max-height', size);
                } else {
                    image.css('max-width', size);
                }
            }
        }
    }
})();