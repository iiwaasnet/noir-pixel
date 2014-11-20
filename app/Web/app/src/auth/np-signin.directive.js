(function () {
    'use strict';

    angular.module('np.auth')
        .directive('npSignin', npSignin);

    function npSignin() {
        var dir = {
            restrict: 'E',
            link: link
        };

        return dir;

        function link(scope, elm, attrs) {
        }
    }
})();