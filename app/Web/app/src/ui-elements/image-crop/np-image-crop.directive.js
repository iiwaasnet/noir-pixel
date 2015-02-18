(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npImageCrop', imageCrop);

    function imageCrop() {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            debugger;
            if (element[0].tagName !== 'IMG') {
                return;
            }

            var inMem = angular.element('<img/>');
            var imageSrc = attrs.ngSrc || attrs.src;
            if (!imageSrc) {
                attrs.$observe('src', loadImage);
            } else {
                loadImage(imageSrc);
            }

            function loadImage(src) {
                debugger;
                inMem.on('load', cropImage);
                inMem.attr('src', src);
            }

            function cropImage() {
                debugger;
                var cropMarginWidth = 5,
                    canvas = angular
                        .element('<canvas/>')
                        .attr({
                            width: inMem.width - 2 * cropMarginWidth,
                            height: inMem.height - 2 * cropMarginWidth
                        })
                        .css('display', 'none')
                        .appendTo('body'),
                    ctx = canvas.get(0).getContext('2d'),
                    cropCoords = {
                        topLeft: {
                            x: cropMarginWidth,
                            y: cropMarginWidth
                        },
                        bottomRight: {
                            x: inMem.width - cropMarginWidth,
                            y: inMem.height - cropMarginWidth
                        }
                    };

                ctx.drawImage(inMem, cropCoords.topLeft.x, cropCoords.topLeft.y, cropCoords.bottomRight.x, cropCoords.bottomRight.y, 0, 0, img.width, img.height);
                var base64ImageData = canvas.get(0).toDataURL();
                element.attr('href', base64ImageData);

                canvas.remove();
            }
        }
    }
})();