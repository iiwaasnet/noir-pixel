(function () {
    'use strict';

    angular.module('np.images')
        .directive('npImgCloak', npImgCloak);

    function npImgCloak() {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            if (element[0].tagName === 'IMG' && attrs.ngSrc) {
                element.css('display', 'none');
                var inMem = angular.element('<img/>')
                    .attr('src', attrs.ngSrc)
                    .on('load', function() {
                        inMem.remove();
                        element.css('display', 'block');
                    });
                attrs.ngSrc = '';
            }
        }
    }
})();