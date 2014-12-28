(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npDropdownToggle', npDropdownToggle);

    function npDropdownToggle() {
        var dir = {
            restrict: 'A'
        };

        return dir;
    }
})();