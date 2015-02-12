(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npStateMenuItem', stateMenuItem);

    function stateMenuItem() {
        var dir = {
            restrict: 'A',
            require: '^npStateMenu',
            link: link,
            scope: {}
        };

        return dir;

        function link(scope, element, attrs, stateMenuCtrl) {
            var selectedClassName = 'selected',
                disabledClassName = 'disabled',
                subMenu = angular.element(element[0].querySelector('[np-state-submenu]')),
                resetItem = angular.element(element[0].querySelector('[np-state-menu-reset]')),
                setItem = angular.element(element[0].querySelector('[np-state-menu-set]'));
            scope.reset = reset;
            scope.enable = enable;

            stateMenuCtrl.addItem(scope);
            enable();
            element.on('$destroy', cleanup);

            function click(e) {
                e.stopPropagation();
                var isSet = setItem.hasClass(selectedClassName);
                stateMenuCtrl.resetAll();
                if (!isSet) {
                    set();
                } else {
                    stateMenuCtrl.enableAll();
                }
            }

            function set() {
                enable();
                setItem.addClass(selectedClassName);
                if (subMenu) {
                    subMenu.addClass(selectedClassName);
                }
            }

            function enable() {
                setItem.removeClass(disabledClassName);
                if (resetItem) {
                    resetItem.on('click', click);
                }
                setItem.on('click', click);
            }

            function reset() {
                setItem.addClass(disabledClassName);
                if (subMenu) {
                    subMenu.removeClass(selectedClassName);
                }
                setItem.removeClass(selectedClassName);

                if (resetItem) {
                    resetItem.off('click', click);
                }
                setItem.off('click', click);
            }

            function cleanup() {
                setItem.off('click', click);
                if (resetItem) {
                    resetItem.off('click', click);
                }
            }
        }
    }
})();