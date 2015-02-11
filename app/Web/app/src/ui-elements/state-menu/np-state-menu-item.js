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
            scope.reset = reset;
            stateMenuCtrl.addItem(scope);
            element.on('click', set);
            element.on('$destroy', cleanup);


            function set(e) {
                e.stopPropagation();
                var isSet = element.hasClass('shake');
                stateMenuCtrl.resetAll();
                if (!isSet) {
                    element.on('click', set);
                    element.addClass('shake');
                }
            }

            function reset() {
                element.off('click', set);
                element.removeClass('shake');
            }

            function cleanup() {
                element.off('click', set);
            }
        }
    }
})();