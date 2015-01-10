(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npTab', tabDirective);

    tabDirective.$inject = ['$rootScope', '$state'];

    function tabDirective($rootScope, $state) {
        var dir = {
            restrict: 'A',
            scope: {
                npTab: '=',
                state: '=',
                params: '='
            },
            link: link
        };

        return dir;

        function link(scope, element) {
            element.on('$destroy', cleanup);
            var unsubscribe = $rootScope.$on('$stateChangeSuccess', onStateChange);
            

            activate();

            function activateTab(e) {
                e.stopPropagation();
                if (!scope.npTab.selected) {
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
                    element.addClass('active');
                } else {
                    element.removeClass('active');
                }
            }

            function onStateChange(event, toState, toParams, fromState, fromParams) {
                updateTabState(toState.name);
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