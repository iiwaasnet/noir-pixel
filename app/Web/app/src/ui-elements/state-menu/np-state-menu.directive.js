(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npStateMenu', stateMenu);

    function stateMenu() {
        var dir = {
            restrict: 'A',
            controller: controller
        };

        return dir;

        function controller() {
            var ctrl = this,
                items = [];
            ctrl.resetAll = resetAll;
            ctrl.enableAll = enableAll;
            ctrl.addItem = addItem;

            function addItem(item) {
                items.push(item);
            }

            function resetAll() {
                angular.forEach(items, function(item) { item.reset(); });
            }

            function enableAll() {
                angular.forEach(items, function (item) { item.enable(); });
            }
        }
    }
})();