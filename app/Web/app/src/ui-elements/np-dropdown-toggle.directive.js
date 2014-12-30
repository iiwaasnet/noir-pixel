(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npDropdownToggle', npDropdownToggle);

    function npDropdownToggle() {
        var dir = {
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            var NG_HIDE_CLASS = 'ng-hide';
            element.addClass(NG_HIDE_CLASS);
            element.css('position', 'absolute');
        }
    }
})();