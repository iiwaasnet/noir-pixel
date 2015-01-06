(function() {
    'use strict';

    angular.module('np.ui-elements')
        .directive('npTab', tabDirective);

    tabDirective.$inject = ['$rootScope', '$state'];

    function tabDirective($rootScope, $state) {
        var dir = {
            restrict: 'A',
            scope: {
                npTab: '='
            },
            link: link
        };

        return dir;

        function link(scope, element, attrs) {
            element.on('$destroy', cleanup);
            var unsubscribe = $rootScope.$on('$stateChangeSuccess', onStateChange);

            activate();

            function activate() {
                var currentState = (currentState = $state)
                    && (currentState = currentState.current)
                    && (currentState = currentState.name);
                updateTabState(currentState);
            }

            function updateTabState(stateName) {
                if (scope.npTab.state === stateName) {
                    scope.npTab.active = true;
                } else {
                    scope.npTab.active = false;
                }
            }

            function onStateChange(event, toState, toParams, fromState, fromParams) {
                updateTabState(toState);
            }

            function cleanup() {
                if (unsubscribe) {
                    unsubscribe();
                }
            }
        }
    }
})();