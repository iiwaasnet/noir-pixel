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
            var selectedClassName = 'selected';
            var disabledClassName = 'disabled';
            var subMenu = angular.element(element[0].querySelector('*[np-state-submenu]'));
            scope.reset = reset;
            scope.disable = disable;
            scope.enable = enable;

            stateMenuCtrl.addItem(scope);
            enable();
            element.on('$destroy', cleanup);

            function click(e) {
                e.stopPropagation();
                var isSet = element.hasClass(selectedClassName);
                stateMenuCtrl.resetAll();
                if (!isSet) {
                    set();
                } else {
                    stateMenuCtrl.enableAll();
                }
            }

            function set() {
                enable();
                element.addClass(selectedClassName);
                if (subMenu) {
                    subMenu.addClass(selectedClassName);
                }
            }

            function enable() {
                element.removeClass(disabledClassName);
                element.on('click', click);
            }

            function disable() {
                element.addClass(disabledClassName);
                element.off('click', click);
            }

            function reset() {
                disable();
                if (subMenu) {
                    subMenu.removeClass(selectedClassName);
                }
                element.removeClass(selectedClassName);
            }

            function cleanup() {
                element.off('click', click);
            }
        }
    }
})();