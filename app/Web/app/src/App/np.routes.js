(function() {
    'use strict';

    angular.module('np')
        .config(config);

    config.$injector = ['$routeProvider', '$locationProvider'];

    function config($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: '/app/src/Home/Home.html',
                controller: 'HomeController as homeCtrl'
            })
            .when('/list', {
                templateUrl: '/app/src/List/List.html',
                controller: 'ListController as listCtrl'
            })
            .otherwise({
                redirectTo: '/'
            });

        $locationProvider.html5Mode(true);
    }
})();