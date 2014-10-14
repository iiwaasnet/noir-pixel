 (function() {
     'use strict';

     angular.module('np')
         .run(run);

     run.$injector = ['$rootScope', 'Strings', 'ApplicationLogging'];

     function run($rootScope, Strings, ApplicationLogging) {
         $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
             //if (curr.$$route && curr.$$route.resolve) {
             //    // Show a loading message until promises are not resolved
             //    $root.loadingView = true;
             //}
         });

         //try {
         //    Strings.init();
         //} catch (e) {
         //    ApplicationLogging.error(e);
         //}
     }
 })();