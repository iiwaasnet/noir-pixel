(function () {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npVisibilityTransition', visibilityTransition);

    function visibilityTransition() {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            element.on('click', click);
            var scroll = angular.element(element.parent());
            var parent = angular.element(scroll.parent());
            scope.$watch(function () { return scroll.css('top'); }, test);

            function test(offsetTop) {
                var rect = element[0].getBoundingClientRect();
                var visible = rect.top - parent[0].offsetTop >= 0;
                    //&& (rect.top + rect.height - parent[0].offsetTop) <= parent[0].clientHeight;
                element.css('opacity', (visible ? 1 : 0.1));
            }

            function click() {
                var rect = element[0].getBoundingClientRect();
                var visible = rect.top - parent[0].offsetTop >= 0
                    && (rect.top + rect.height - parent[0].offsetTop) <= parent[0].clientHeight;
                alert(visible);
            }
        }
    }

})();