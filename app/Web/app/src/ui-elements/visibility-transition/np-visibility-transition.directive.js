(function() {
    "use strict";

    angular.module('np.ui-elements')
        .directive('npVisibilityTransition', visibilityTransition);

    function visibilityTransition() {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element) {
            var scroll = angular.element(element.parent());
            var parent = angular.element(scroll.parent());

            scope.$watch(function() { return scroll[0].offsetTop; }, changeOpacity);
            //scope.$watch(function() {
            //    var rect = element[0].getBoundingClientRect();
            //    return rect.top - parent[0].offsetTop;
            //}, changeOpacity);

            changeOpacity();

            function changeOpacity(val) {
                var rect = element[0].getBoundingClientRect(),
                    top = rect.top - parent[0].offsetTop,
                    opacity = 1;
                if (isTopHidden()) {
                    opacity = ((top + rect.height) / rect.height);
                } else {
                    if (isBottomHidden()) {
                        opacity = (parent[0].clientHeight - top) / rect.height;
                    }
                }

                element.css('opacity', opacity);

                function isTopHidden() {
                    return top < 0;
                }

                function isBottomHidden() {
                    return (top + rect.height) > parent[0].clientHeight;
                }
            }
        }
    }

})();