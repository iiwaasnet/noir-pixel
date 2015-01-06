(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npDropdownToggle', npDropdownToggle);

    function npDropdownToggle() {
        var dir = {
            restrict: 'A',
            link: link,
            require: '^npDropdown'
        };

        return dir;

        function link(scope, element, attrs, npDropdownCtrl) {
            var NG_HIDE_CLASS = 'ng-hide',
                closeDropdownAttr = 'np-close-dropdown';
            element.addClass(NG_HIDE_CLASS);
            element.css('position', 'absolute');
            element.on('click', onClick);

            function onClick(e) {
                if (!e.target.hasAttribute(closeDropdownAttr)) {
                    e.stopPropagation();
                }
            }
        }
    }
})();