(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npDropdown', npDropdown);

    npDropdown.$inject = ['$document', '$animate'];

    function npDropdown($document, $animate) {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            var NG_HIDE_CLASS = 'ng-hide';
            var NG_HIDE_IN_PROGRESS_CLASS = 'ng-hide-animate';

            var toggle = element[0].querySelector('[np-dropdown-toggle]');
            if (toggle) {
                toggle = angular.element(toggle);
                //toggle.addClass(NG_HIDE_CLASS);

                element.css('display', 'inline-block');
                element.css('position', 'relative');
                element.on('$destroy', cleanup);
                element.on('click', toggleDropdown);

                $document.on('click', hideDropdown);
            }

            function dropdownVisible() {
                return toggle && !toggle.hasClass(NG_HIDE_CLASS);
            }

            function hideDropdown(e) {
                e.stopPropagation();
                $animate['addClass'](toggle, NG_HIDE_CLASS, {
                    tempClasses: NG_HIDE_IN_PROGRESS_CLASS
                });
                scope.$apply();
            }

            function toggleDropdown(e) {
                e.stopPropagation();
                $animate[dropdownVisible() ? 'addClass' : 'removeClass'](toggle, NG_HIDE_CLASS, {
                    tempClasses: NG_HIDE_IN_PROGRESS_CLASS
                });
                scope.$apply();
            }

            function cleanup() {
                $document.off('click', hideDropdown);
            }
        }
    }
})();