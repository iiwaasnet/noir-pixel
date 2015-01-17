(function(){
    'use strict';

    angular.module('np.ui-elements')
        .directive('npAutofocus', autofocus);

    autofocus.$inject = ['$timeout'];

    function autofocus($timeout) {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element) {
            $timeout(function() {
                element[0].focus();
            }, 100);
        }
    }
})();