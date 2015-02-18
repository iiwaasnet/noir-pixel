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
            var imageSrc = attrs.ngSrc || attrs.src;
            var inMem = angular.element('<img/>');
            inMem.on('load', imageLoaded);
            inMem.attr('src', imageSrc);

            function imageLoaded() {
                var cropMarginWidth = 5,
                    canvas = angular.element('<canvas/>')
                        .attr({
                            width: inMem.width - 2 * cropMarginWidth,
                            height: inMem.height - 2 * cropMarginWidth
                        })
                        .hide()
                        .appendTo('body'),
                    ctx = canvas.get(0).getContext('2d'),
                    a = $('<a download="cropped-image" title="click to download the image" />'),
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

                //a
                //    .attr('href', base64ImageData)
                //    .text('cropped image')
                //    .appendTo('body');

                //a
                //    .clone()
                //    .attr('href', img.src)
                //    .text('original image')
                //    .attr('download', 'original-image')
                //    .appendTo('body');

                canvas.remove();
            }
        }
    }
})();