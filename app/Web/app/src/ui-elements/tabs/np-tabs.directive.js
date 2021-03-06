﻿(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npTabs', tabsDirective);

    function tabsDirective() {
        var dir = {
            restrict: 'A',
            templateUrl: '/app/src/ui-elements/tabs/tabs.html',
            scope : {
                npTabs: '='
            }
        };

        return dir;
    }
})();