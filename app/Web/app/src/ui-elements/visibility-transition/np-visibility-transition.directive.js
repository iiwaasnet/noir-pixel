(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npVisibilityTransition', visibilityTransition);

    function visibilityTransition() {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element) {
            element.on('click', click);
            var scroll = angular.element(element.parent());
            var parent = angular.element(scroll.parent());
            scope.$watch(function() { return scroll[0].offsetTop; }, changeOpacity);
            changeOpacity();

            function changeOpacity() {
                var rect = element[0].getBoundingClientRect(),
                    top = rect.top - parent[0].offsetTop,
                    visible = top >= 0
                        && (top + rect.height) <= parent[0].clientHeight;

                element.css('opacity', (visible ? 1 : 0.1));
            }

            function click() {
                var rect = element[0].getBoundingClientRect(),
                    top = rect.top - parent[0].offsetTop,
                    visible = top >= 0
                        && (top + rect.height) <= parent[0].clientHeight;
                alert((top + rect.height) / rect.height);
            }
        }
    }

})();