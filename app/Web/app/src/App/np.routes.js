(function() {
    'use strict';

    angular.module('np')
        .config(config);

    config.$injector = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController as homeCtrl'
            })
            .state('gallery', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController as homeCtrl'
        });

        $urlRouterProvider.otherwise('/');

        $locationProvider.html5Mode(true);
    }
})();