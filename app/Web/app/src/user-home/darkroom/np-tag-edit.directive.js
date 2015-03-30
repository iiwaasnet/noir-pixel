﻿(function() {
    'use strict';

    angular.module('np.user-home')
        .directive('npTagEdit', tagEdit);

    function tagEdit() {
        var dir = {
            restrict: 'A',
            link: link,
            scope: {
                selected: '=selected'
            }
        };

        return dir;

        function link(scope, element) {
            updateState();
            element.on('$destroy', cleanup);
            element.on('click', updateState);

            function updateState() {
                debugger;
                scope.selected = !scope.selected;
                if (scope.selected) {
                    element.addClass('selected');
                } else {
                    element.removeClass('selected');
                }
            }

            function cleanup() {
                element.off('click', click);
            }
        }
    }
})();