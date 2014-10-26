(function() {
    'use strict';

    angular.module('np')
        .run(run);

    run.$injector = ['$rootScope', '$state', 'Strings', 'ApplicationLogging'];

    function run($rootScope, $state, Strings, ApplicationLogging) {

        //$rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
        //    console.log('$stateChangeStart to ' + toState.to + '- fired when the transition begins. toState,toParams : \n', toState, toParams);
        //});
        //$rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams) {
        //    console.log('$stateChangeError - fired when an error occurs during transition.');
        //    console.log(arguments);
        //});
        //$rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        //    console.log('$stateChangeSuccess to ' + toState.name + '- fired once the state transition is complete.');
        //});
        // $rootScope.$on('$viewContentLoading',function(event, viewConfig){
        //   // runs on individual scopes, so putting it in "run" doesn't work.
        //   console.log('$viewContentLoading - view begins loading - dom not rendered',viewConfig);
        // });
        //$rootScope.$on('$viewContentLoaded', function (event) {
        //    console.log('$viewContentLoaded - fired after dom rendered', event);
        //});
        //$rootScope.$on('$stateNotFound', function (event, unfoundState, fromState, fromParams) {
        //    console.log('$stateNotFound ' + unfoundState.to + '  - fired when a state cannot be found by its name.');
        //    console.log(unfoundState, fromState, fromParams);
        //});


        try {
            $rootScope.$on('$stateChangeError', stateChangeError);
            //$rootScope.$on('$stateChangeSuccess', stateChangeSuccess);

            setDefaultLanguage();
        } catch (e) {
            ApplicationLogging.error(e);
        }

        function setDefaultLanguage() {
            Strings.setCurrentLanguage(Strings.getCurrentLanguage());
        }

        function stateChangeError(event, toState, toParams, fromState, fromParams, error) {
            if (error.status === 401) {
                event.preventDefault();
                //alert('Unauthorized!');
                if (!fromState.name) {
                    $state.go('notAuthorized', { redirectTo: toState.url });
                }
                //else {
                //    $state.go('notAuthorized', { backTo: fromState.name});
                //}
            }

        }

        function stateChangeSuccess(event, toState, toParams, fromState, fromParams) {
        }
    }
})();