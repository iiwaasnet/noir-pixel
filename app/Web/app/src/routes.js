angular.module('np').config(function($routeProvider, $locationProvider) {
    $routeProvider
        .when('/list', {
            templateUrl: '/app/src/list/List.html',
            controller: 'listController'
        });

    $locationProvider.html5Mode(true);

});