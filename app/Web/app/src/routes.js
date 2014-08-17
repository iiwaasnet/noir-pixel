angular.module('np').config(function($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: '/app/src/Home/Home.html',
            controller: 'homeController'
        })
        .when('/list', {
            templateUrl: '/app/src/List/List.html',
            controller: 'listController'
        })
    .otherwise({
        redirectTo: '/'
    });

    $locationProvider.html5Mode(true);

});