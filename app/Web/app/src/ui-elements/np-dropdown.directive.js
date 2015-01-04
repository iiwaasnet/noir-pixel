(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npDropdown', npDropdown);

    npDropdown.$inject = ['$document', '$animate', '$rootScope'];

    function npDropdown($document, $animate, $rootScope) {
        var dir = {
            restrict: 'A',
            link: link,
            controller: ['$scope', controller]
        };

        return dir;

        function controller($scope) {
            var NG_HIDE_CLASS = 'ng-hide';
            var NG_HIDE_IN_PROGRESS_CLASS = 'ng-hide-animate';
            var ctrl = this;
            ctrl.toggleDropdown = toggleDropdown;
            ctrl.hideDropdown = hideDropdown;

            function toggleDropdown(toggle) {
                $scope.$evalAsync(function() {
                    $animate[dropdownVisible(toggle) ? 'addClass' : 'removeClass'](toggle, NG_HIDE_CLASS, {
                        tempClasses: NG_HIDE_IN_PROGRESS_CLASS
                    });
                });
            }

            function hideDropdown(toggle) {
                $scope.$evalAsync(function() {
                    $animate['addClass'](toggle, NG_HIDE_CLASS, {
                        tempClasses: NG_HIDE_IN_PROGRESS_CLASS
                    });
                });
            }

            function dropdownVisible(toggle) {
                return toggle && !toggle.hasClass(NG_HIDE_CLASS);
            }
        }

        function link(scope, element, attrs, ctrl) {
            var toggle = element[0].querySelector('[np-dropdown-toggle]'),
                unsubscribeRootScope;
            if (toggle) {
                toggle = angular.element(toggle);

                element.css('display', 'inline-block');
                element.css('position', 'relative');
                element.on('$destroy', cleanup);
                element.on('click', toggleDropdown);

                unsubscribeRootScope = $rootScope.$on('$stateChangeStart', function() { ctrl.hideDropdown(toggle); });
                $document.on('click', hideDropdown);
            }

            function hideDropdown(e) {
                e.stopPropagation();
                ctrl.hideDropdown(toggle);
            }

            function toggleDropdown(e) {
                e.stopPropagation();
                ctrl.toggleDropdown(toggle);
            }

            function cleanup() {
                $document.off('click', hideDropdown);
                if (unsubscribeRootScope) {
                    unsubscribeRootScope();
                }
            }
        }
    }
})();