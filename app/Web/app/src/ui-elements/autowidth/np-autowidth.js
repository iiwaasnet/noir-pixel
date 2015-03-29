(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npAutowidth', autowidth);

    autowidth.$inject = ['$window', '$document'];

    function autowidth($window, $document) {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            var style = $window.document.defaultView.getComputedStyle(element[0], '');

            if (element[0].tagName === 'INPUT' && attrs.type.toUpperCase() === 'TEXT') {
                element.on('keyup update blur', updateWidth);
                element.on('$destroy', cleanup);
            }

            function updateWidth() {
                
                var span = angular.element('<span>');
                span.css({
                    'font-family': style.fontFamily,
                    'font-size': style.fontSize,
                    'font-weight': style.fontWeight,
                    'letter-spacing': style.letterSpacing,
                    'padding': style.padding,
                    'position': 'absolute',
                    'left': '-99999px',
                    'top': '-99999px',
                    'width': 'auto',
                    'white-space': 'nowrap'
                });
                span.text(element[0].value);

                angular.element($document.find('body')[0]).append(span);
                var width = span[0].getBoundingClientRect().width;

                element.css('width', width + 12 + 'px');
            }

            function cleanup() {
                element.off('keyup update blur', updateWidth);
            }
        }
    }
})();