(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npFillHeight', fillHeight);

    fillHeight.$inject = ['$window'];

    function fillHeight($window) {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            var scrollBarHeight = 25,
                bottomMargin = Math.max(0, parseInt(attrs.bottomMargin, 10)),
                height = Math.max($window.innerHeight
                    - element[0].offsetTop
                    - scrollBarHeight
                    - bottomMargin,
                    Math.max(0, parseInt(attrs.minHeight, 10)));
            element.css({
                height: height + 'px',
                overflow: 'hidden'
            });
        }
    }
})();