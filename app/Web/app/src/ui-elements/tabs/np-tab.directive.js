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
                parentState: '=',
                params: '='
            },
            link: link
        };

        return dir;

        function link(scope, element) {
            var selected = false;
            element.on('$destroy', cleanup);
            element.on('click', activateTab);
            var unsubscribe = $rootScope.$on('$stateChangeSuccess', onStateChange);
            

            activate();

            function activateTab(e) {
                e.stopPropagation();
                if (!selected) {
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
                if (~stateName.indexOf(scope.state) || ~stateName.indexOf(scope.parentState)) {
                    selected = true;
                    element.addClass('active');
                } else {
                    selected = false;
                    element.removeClass('active');
                }
            }

            function onStateChange(event, toState, toParams, fromState, fromParams) {
                updateTabState(toState.name);
            }

            function cleanup() {
                unsubscribe && unsubscribe();
                element.off('click', activateTab);
            }
        }
    }
})();