﻿(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npTab', tabDirective);

    tabDirective.$inject = ['$rootScope', '$state'];

    function tabDirective($rootScope, $state) {
        var dir = {
            restrict: 'A',
            templateUrl: '/app/src/ui-elements/tabs/tab.html',
            scope: {
                npTab: '=',
                text: '=',
                image: '=',
                state: '=',
                params: '=',
                beforeActivate: '='
            },
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            element.on('$destroy', cleanup);
            var unsubscribe = $rootScope.$on('$stateChangeSuccess', onStateChange);
            

            activate();

            function activateTab(e) {
                e.stopPropagation();
                if (!scope.npTab.selected && scope.beforeActivate()) {
                    $state.go(scope.state, scope.params);
                }
            }

            function activate() {
                var currentState = (currentState = $state)
                    && (currentState = currentState.current)
                    && (currentState = currentState.name);
                updateTabState(currentState);
            }

            function updateTabState(stateName) {
                if (scope.state === stateName) {
                    scope.npTab.selected = true;
                } else {
                    scope.npTab.selected = false;
                }
            }

            function onStateChange(event, toState, toParams, fromState, fromParams) {
                updateTabState(toState);
            }

            function cleanup() {
                if (unsubscribe) {
                    unsubscribe();
                }
                element.off('click', activateTab);
            }
        }
    }
})();