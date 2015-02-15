(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npSpinner', spinner);

    function spinner() {
        var dir = {
            restrict: 'AE',
            templateUrl: '/app/src/ui-elements/spinner/spinner.html'
        };

        return dir;
    }
})();